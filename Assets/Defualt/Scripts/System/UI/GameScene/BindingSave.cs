using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingSave : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private void Awake()
    {
        LoadBindingsAndUpdateUI();
    }

    public void SaveBindings()
    {
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        // ������ �������� �ʴ´ٸ� ����
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, $"KeyBindData.json");

        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var bindingsDictionary = new Dictionary<string, List<string>>();

        foreach (var actionMap in actionAsset.actionMaps)
        {
            foreach (var action in actionMap)
            {
                var bindingList = new List<string>();

                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    string effectivePath = binding.overridePath != null && binding.overridePath != "" ? binding.overridePath : binding.path;
                    bindingList.Add(effectivePath);
                }

                if (bindingList.Count > 0)
                {
                    bindingsDictionary[action.id.ToString()] = bindingList;
                }
            }
        }

        string bindingsJson = JsonConvert.SerializeObject(bindingsDictionary);

        File.WriteAllText(filePath, bindingsJson);

        Debug.Log("�����");
    }

    public void LoadBindings()
    {
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        var filePath = Path.Combine(folderPath, $"KeyBindData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var bindingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var actionMap in actionAsset.actionMaps)
            {
                foreach (var action in actionMap)
                {
                    if (bindingsDictionary.ContainsKey(action.id.ToString()))
                    {
                        var bindingList = bindingsDictionary[action.id.ToString()];

                        foreach (var binding in bindingList)
                        {
                            if (!string.IsNullOrEmpty(binding))
                            {
                                action.AddBinding(binding);
                            }
                        }
                    }
                }
            }
            print("�ε��");
        }
        else
        {
            filePath = Path.Combine(folderPath, $"DefaultKeyBindData.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var bindingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

                foreach (var actionMap in actionAsset.actionMaps)
                {
                    foreach (var action in actionMap)
                    {
                        if (bindingsDictionary.ContainsKey(action.id.ToString()))
                        {
                            var bindingList = bindingsDictionary[action.id.ToString()];

                            foreach (var binding in bindingList)
                            {
                                if (!string.IsNullOrEmpty(binding))
                                {
                                    action.AddBinding(binding);
                                }
                            }
                        }
                    }
                }
                print("�ε��");
            }
            else
            {
                print("�ε� ����");
            }
        }
    }

    public void UpdateAllBindingButtonTexts()
    {
        // ��� Keybinding �ν��Ͻ��� ���� UpdateBindingButtonText ȣ��
        foreach (var keybinding in FindObjectsOfType<Keybinding>(true))
        {
            keybinding.UpdateBindingButtonText();
        }
    }

    public void LoadBindingsAndUpdateUI()
    {
        LoadBindings(); // ���ε� �ε�
        UpdateAllBindingButtonTexts(); // UI ������Ʈ
    }
}
