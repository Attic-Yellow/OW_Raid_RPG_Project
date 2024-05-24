using Photon.Pun;
using UnityEngine;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviourPun, IPunObservable
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Tooltip("Camera zoom sensitivity")]
        public float ZoomSensitivity = 2.0f;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private CinemachineVirtualCamera _virtualCamera;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        public float cameraSensitivity = 90;
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDSkill;

        private float upperLayerWeight = 0.5f;

#if ENABLE_INPUT_SYSTEM
        [SerializeField] private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        [SerializeField] private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool skilling = false;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            // get a reference to the Cinemachine Virtual Camera
            _virtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
            if (_virtualCamera == null)
            {
                Debug.LogError("PlayerFollowCamera에 CinemachineVirtualCamera 컴포넌트가 없습니다.");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GameObject.Find("PlayerInput").GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>(); // 
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (_animator.applyRootMotion == false)
                {
                    _hasAnimator = TryGetComponent(out _animator);
                    JumpAndGravity();
                    GroundedCheck();
                    Move();
                    ZoomCamera();
                }
            }
        }

        private void LateUpdate()
        {
            if (photonView.IsMine)
            {
                CameraRotation();
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDSkill = Animator.StringToHash("SkillNum");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            if (Input.GetMouseButton(1)) // Check if right mouse button is held down
            {
                rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, -90, 90);

                CinemachineCameraTarget.transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                CinemachineCameraTarget.transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            }
        }

        private void ZoomCamera()
        {
            if (_virtualCamera != null && Input.mouseScrollDelta.y != 0)
            {
                _virtualCamera.m_Lens.FieldOfView -= Input.mouseScrollDelta.y * ZoomSensitivity;
                _virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(_virtualCamera.m_Lens.FieldOfView, 20.0f, 100.0f);
            }
        }

        private void Move()
        {
            // Check if there is any movement input
            Vector3 move = new Vector3(_input.move.x, 0.0f, _input.move.y);
            if (move != Vector3.zero)
            {
                // Calculate target rotation based on input
                _targetRotation = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;

                if (_input.move.y >= 0)
                {
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                    // Rotate the character only if moving forward
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            _speed = Mathf.Lerp(_speed, targetSpeed * move.magnitude, Time.deltaTime * SpeedChangeRate);

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // Update animator
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _speed);
                _animator.SetFloat(_animIDMotionSpeed, move.magnitude);
            }
        }

        public bool MovingSkill(int num) //움직이면서 사용 가능한 스킬
        {
            if (_hasAnimator)
            {
                print($"movingSkill {num + 1}");
                _animator.SetInteger(_animIDSkill, num + 1);
                _animator.SetLayerWeight(1, 0.5f);
                return true;
            }

            return false;
        }

        public bool IdleSkill(int num)
        {
            if (_hasAnimator && Grounded)
            {
                skilling = true;
                _animator.SetLayerWeight(1, 0);
                _animator.applyRootMotion = true;
                print($"IdleSkill {num + 1}");
                _animator.SetInteger(_animIDSkill, num + 1);
                return true;
            }
            print("왜 false지?");
            return false;
            
        }

        public void SkillAniFinish()
        {
            skilling = false;
            _animator.applyRootMotion = false;
            _animator.SetInteger(_animIDSkill, 0);
            _animator.SetLayerWeight(1, 0);
        }

        public bool GetSkilling()
        {
            print(skilling);
            return skilling;
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public float ReturnCurrentAniTime() //현재 사용중인 애니메이션 총 시간 반환
        {
            return _animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public float GetAnimLayer(int layerNum) //애니메이터 레이어 weight 체크
        {
            return _animator.GetLayerWeight(layerNum);
        }

        public bool UseUpperLayer() //상체 레이어를 사용하고있는지 == 스킬을 사용하고잇는지
        {
            if (GetAnimLayer(1) > 0)
            {
                return false;
            }
            return true;
        }

        public void SettingLayerWeight(int layerNum, float weight)
        {
            _animator.SetLayerWeight(layerNum, weight);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
