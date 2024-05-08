using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public enum KeyBind
{
    Idle,
    BindNotSave
}

public class Keybinding : MonoBehaviour
{
    public InputActionAsset actionAsset;
    [SerializeField] private InputAction actionToRebind;
    [SerializeField] private string bindName;
    [SerializeField] private TMP_Text bindingButtonText;
    [SerializeField] private int bindingIndex;
    [SerializeField] private TextMeshProUGUI quickText;
    [SerializeField] private KeyBind keyBind;

    private void Start()
    {
        actionToRebind = actionAsset.FindAction(bindName, true);
        keyBind = KeyBind.Idle;
    }

    // Ű ����ε� ȣ��
    public void StartRebindingProcess()
    {
        keyBind = KeyBind.BindNotSave;

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

    // Ű ����ε� �ڷ�ƾ
    private IEnumerator RebindKey()
    {
        bool isComposite = false;
        string firstBinding = "";

        // ù ��° Ű ���ε�
        yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex, (binding) => {
            firstBinding = binding;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt))
            {
                isComposite = true;
            }
        }));

        if (isComposite)
        {
            yield return StartCoroutine(RebindingCoroutine(actionToRebind, bindingIndex + 1, _ => { })); // ���� Ű�� ���, �� ��° Ű ���ε� ����
        }
        else
        {
            // ���� Ű�� ���, ù ��° ���ε��� �� ��° Ű���� ����
            actionToRebind.ApplyBindingOverride(bindingIndex, firstBinding);
            actionToRebind.ApplyBindingOverride(bindingIndex + 1, firstBinding);
        }

        GameManager.Instance.SetIsRebinding(false);
        UpdateBindingButtonText();
    }

    // ���ε� �ؽ�Ʈ ������Ʈ
    public void UpdateBindingButtonText()
    {
        if (string.IsNullOrEmpty(bindName))
        {
            return; // bindNaem�� ��� ������ ���⼭ �Լ� ������ �ߴ�
        }

        InputAction action = actionAsset.FindAction(bindName, throwIfNotFound: false);
        if (action != null)
        {
            
            if (action.GetBindingDisplayString(bindingIndex) == action.GetBindingDisplayString(bindingIndex + 1)) // ���� ���ε��� ���
            {
                bindingButtonText.text = action.GetBindingDisplayString(bindingIndex);
                quickText.text = action.GetBindingDisplayString(bindingIndex);
            }
            else // ���� ���ε��� ���
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

    // ù ��° Ű ���ε� �ڷ�ƾ
    private IEnumerator RebindingCoroutine(InputAction action, int targetBindingIndex, Action<string> onBindingComplete)
    {
        action.Disable();
        var rebindOperation = action.PerformInteractiveRebinding(targetBindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete((op) => {
                if (op.selectedControl != null)
                {
                    // ���õ� ��Ʈ���� ������, �ش� ��θ� ��ȯ
                    onBindingComplete(op.selectedControl.path);
                }
                else
                {
                    // ���õ� ��Ʈ���� ������, null�� ��ȯ�Ͽ� �̸� ó���� �� �ֵ��� ��
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
        operation.Dispose(); // ����ε� �۾� ���ҽ� ����

        action.Enable(); // ����ε��� �Ϸ�Ǹ�, �׼��� �ٽ� Ȱ��ȭ
        bindingButtonText.text = action.GetBindingDisplayString(bindingIndex); // ��ư �ؽ�Ʈ ������Ʈ
        GameManager.Instance.SetIsRebinding(false); // ���� �Ŵ��� ���� ������Ʈ
    }

    public KeyBind GetBindState()
    {
        return keyBind;
    }

    public void SetBindState(KeyBind keyBind)
    {
        this.keyBind = keyBind;
    }
}
