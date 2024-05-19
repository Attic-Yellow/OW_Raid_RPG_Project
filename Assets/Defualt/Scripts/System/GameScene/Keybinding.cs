using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;



public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    [SerializeField] private InputAction actionToRebind;
    [SerializeField] private string bindName;
    [SerializeField] private TMP_Text bindingButtonText;
    [SerializeField] private int bindingIndex;
    [SerializeField] private TextMeshProUGUI quickText;
    

    private void Start()
    {
        actionToRebind = actionAsset.FindAction(bindName, true);
    }

    // 키 재바인딩 호출
    public void StartRebindingProcess()
    {
        BindingSave.Instance.keyBind = KeyBind.BindNotSave;

        if (GameManager.Instance.GetIsRebinding())
        {
            return;
        }

        GameManager.Instance.SetIsRebinding(true);

        if (actionToRebind != null)
        {
            StartCoroutine(RebindKey());
        }
        else
        {
            print("Action to rebind not found.");
        }
    }

    // 키 재바인딩 코루틴
    private IEnumerator RebindKey()
    {
        bool isComposite = false;
        string firstBinding = "";

        // 첫 번째 키 바인딩
        yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex, (binding) => {
            firstBinding = binding;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
            {
                isComposite = true;
            }
        }));

        if (isComposite)
        {
            yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex + 1, _ => { })); // 복합 키의 경우, 두 번째 키 바인딩 시작
        }
        else
        {
            actionToRebind.Disable();
            // 단일 키의 경우, 첫 번째 바인딩을 두 번째 키에도 적용
            if (actionToRebind.bindings[bindingIndex].hasOverrides)
            {
                actionToRebind.RemoveBindingOverride(bindingIndex);
                actionToRebind.RemoveBindingOverride(bindingIndex + 1);
            }
            actionToRebind.ApplyBindingOverride(bindingIndex, firstBinding);
            actionToRebind.ApplyBindingOverride(bindingIndex + 1, firstBinding);
            print($"{actionToRebind.bindings[bindingIndex].path}, {actionToRebind.bindings[bindingIndex + 1].overridePath}");
            actionToRebind.Enable();
        }

        GameManager.Instance.SetIsRebinding(false);
        UpdateBindingButtonText();
    }

    // 바인딩 텍스트 업데이트
    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindName))
        {
            return; // bindNaem이 비어 있으면 여기서 함수 실행을 중단
        }

        InputAction action = actionAsset.FindAction(bindName, throwIfNotFound: false);
        if (action != null)
        {
            
            if (action.GetBindingDisplayString(bindingIndex) == action.GetBindingDisplayString(bindingIndex + 1)) // 복합 바인딩인 경우
            {
                bindingButtonText.text = action.GetBindingDisplayString(bindingIndex);
                quickText.text = action.GetBindingDisplayString(bindingIndex);
            }
            else // 단일 바인딩인 경우
            {
                if (action.GetBindingDisplayString(bindingIndex) == "Control")
                {
                    bindingButtonText.text = $"Ctrl + {action.GetBindingDisplayString(bindingIndex + 1)}";
                }
                else
                {
                    bindingButtonText.text = $"{action.GetBindingDisplayString(bindingIndex)} + {action.GetBindingDisplayString(bindingIndex + 1)}";
                }
                
                switch (action.GetBindingDisplayString(bindingIndex))
                {
                    case "Control":
                        quickText.text = $"c{action.GetBindingDisplayString(bindingIndex + 1)}";
                        break;
                    case "Ctrl":
                        quickText.text = $"c{action.GetBindingDisplayString(bindingIndex + 1)}";
                        break;
                    case "Shift":
                        quickText.text = $"s{action.GetBindingDisplayString(bindingIndex + 1)}";
                        break;
                    case "Alt":
                        quickText.text = $"a{action.GetBindingDisplayString(bindingIndex + 1)}";
                        break;
                }
            }
        }
        else
        {
            bindingButtonText.text = "";
        }
    }

    // 첫 번째 키 바인딩 코루틴
    private IEnumerator RebindingCoroutine(InputAction action, int targetBindingIndex, Action<string> onBindingComplete)
    {
        action.Disable();
        var rebindOperation = action.PerformInteractiveRebinding(targetBindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete((op) => {
                if (op.selectedControl != null)
                {
                    // 선택된 컨트롤이 있으면, 해당 경로를 반환
                    onBindingComplete(op.selectedControl.path);
                }
                else
                {
                    // 선택된 컨트롤이 없으면, null을 반환하여 이를 처리할 수 있도록 함
                    onBindingComplete(null);
                }
            })
            .Start();

        yield return new WaitUntil(() => rebindOperation.completed);

        rebindOperation.Dispose();
        action.Enable();
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation, InputAction action)
    {
        operation.Dispose(); // 재바인딩 작업 리소스 해제

        action.Enable(); // 재바인딩이 완료되면, 액션을 다시 활성화
        bindingButtonText.text = action.GetBindingDisplayString(bindingIndex); // 버튼 텍스트 업데이트
        GameManager.Instance.SetIsRebinding(false); // 게임 매니저 상태 업데이트
    }
}
