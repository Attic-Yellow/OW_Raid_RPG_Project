using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class InitChar : MonoBehaviour
{
    [SerializeField] private Camera renderTextureCam;
    [SerializeField] private Transform target;
    [SerializeField] GameObject nickNameOverlay;
    private string nickname;
    public enum Job
    {
        탱커,
        근접딜러,
        원거리딜러,
        힐러

    }
   [SerializeField] private Job state;
    public Job State {  get { return state; } set { state = value; } }
    int jobsCount = Enum.GetNames(typeof(Job)).Length;

    private Dictionary<Enum, GameObject[]> charatorDic = new();

    [SerializeField] private GameObject[] tankerPrefabs;
    [SerializeField] private GameObject[] shortDealerPrefabs;
    [SerializeField] private GameObject[] longDealerPrefabs;
    [SerializeField] private GameObject[] healerPrefabs;

    [SerializeField] private TextMeshProUGUI genderText;
    [SerializeField] private TextMeshProUGUI jobText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TMP_InputField nicknameText;

    public string JobText { get { return jobText.text; } set { jobText.text = value; } }
    public string GenderText { get { return genderText.text; } set { genderText.text = value; } }
    public string NickNameText { get {return nicknameText.text; } }
    public string ErrorText { get { return errorText.text; } set {  errorText.text = value; } }

    private void Awake()
    {
        AddDic();
        State = Job.탱커;
        JobText = State.ToString();
        GenderText = "man";
        ChangeTarget();
        DeleteBtn();
    }

    private void AddDic()
    {
        charatorDic.Add(Job.탱커, tankerPrefabs);
        charatorDic.Add(Job.근접딜러, shortDealerPrefabs);
        charatorDic.Add(Job.원거리딜러, longDealerPrefabs);
        charatorDic.Add(Job.힐러, healerPrefabs);
    }
   
    private void ChangeTarget()
    {

        // 현재 상태에 해당하는 프리팹 배열을 가져옴
        GameObject[] selectedPrefabs = charatorDic[State];

        // 선택된 프리팹의 인덱스
        int prefabIndex = GenderText == "man" ? 0 : 1;

        Transform camPosTransform = selectedPrefabs[prefabIndex].transform.Find("CamPos");
        if (camPosTransform != null)
        {
            target = camPosTransform;
            renderTextureCam.transform.parent = target;
            renderTextureCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);  
        }
        else
        {
            Debug.LogError("CamPos not found in the instantiated character prefab.");
        }
    }

    public void OnJobChange()
    {
        int nextState = (int)State + 1;
        if (nextState >= jobsCount)
        {
            nextState = 0;
        }

        State = (Job)nextState; // 직업 변경
        JobText = State.ToString();
        ChangeTarget();
    }
    public void OnGenderChange() //성별 교체 버튼을 눌렀을 때
    {
        GenderText = GenderText == "man" ? "woman" : "man";
        ChangeTarget();
    }
    public void NicknameOverlay()
    {
        if(nickNameOverlay != null && !nickNameOverlay.activeSelf)
        nickNameOverlay.SetActive(true);
    }
    public void DeleteBtn()
    {
        nickNameOverlay.SetActive(false);
    }
    
    public void InitNickName()
    {
       if(NickNameText.Length > 9 || NickNameText.Length < 2 )
        {
            ErrorText = "2글자 이상 9글자 이내로 입력하세요";
            errorText.color = Color.red;
            nickname = null;
            return;
        }
        /*똑같은 닉네임이 있을 시 빠꾸*/
       
        nickname = NickNameText;
        errorText.color= Color.black;
        ErrorText = "사용 가능한 닉네임입니다";
        DeleteBtn();
    }

    public void InitCharBtn()
    {
      if(nickname != null)
        {
            //생성하고 저장하는 로직
            Debug.Log($"{nickname}생성 완료");
            return;
        }

        Debug.Log("캐릭터 이름을 설정해주세요");
    }
}
