using Firebase.Firestore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyBind
{
    Idle,
    BindNotSave
}

public class BindingSave : MonoBehaviour
{
    public static BindingSave Instance;

    public InputActionAsset actionAsset;
    public KeyBind keyBind;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadBindingsAndUpdateUI();

        BindingSave.Instance.keyBind = KeyBind.Idle;
    }

    public void SaveBindings()
    {
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        // 폴더가 존재하지 않는다면 생성
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

        Debug.Log("저장됨");
    }

    public void ReLoadBindings(string path)
    {
        string json = File.ReadAllText(path);
        var bindingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

        foreach (var actionMap in actionAsset.actionMaps)
        {
            if (actionMap.name == "Chat" || actionMap.name == "Login")
            {
                continue;
            }
            foreach (var action in actionMap)
            {
                action.Disable();
                if (action.name == "Look" || action.name == "Sprint")
                {
                    continue;
                }
                if (bindingsDictionary.ContainsKey(action.id.ToString()))
                {
                    var bindingList = bindingsDictionary[action.id.ToString()];

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            if (action.bindings[1].hasOverrides)
                            {
                                action.RemoveBindingOverride(1);
                                action.RemoveBindingOverride(2);
                            }
                            action.ApplyBindingOverride(1, bindingList[1]);
                            action.ApplyBindingOverride(2, bindingList[2]);
                        }
                        else if (i == 1)
                        {
                            if (action.bindings[4].hasOverrides)
                            {
                                action.RemoveBindingOverride(4);
                                action.RemoveBindingOverride(5);
                            }
                            action.ApplyBindingOverride(4, bindingList[4]);
                            action.ApplyBindingOverride(5, bindingList[5]);
                        }

                    }
                }
                action.Enable();
            }
        }
        print("로드됨");
    }

    public void ResetBindings()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath);

        var filePath = Path.Combine(folderPath, $"KeyBindData.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var bindingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            foreach (var actionMap in actionAsset.actionMaps)
            {
                if (actionMap.name == "Chat" || actionMap.name == "Login")
                {
                    continue;
                }
                foreach (var action in actionMap)
                {
                    action.Disable();
                    if (action.name == "Look" || action.name == "Sprint" || action.name == "Jump")
                    {
                        continue;
                    }
                    if (bindingsDictionary.ContainsKey(action.id.ToString()))
                    {
                        var bindingList = bindingsDictionary[action.id.ToString()];

                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 0)
                            {
                                if (action.bindings[1].hasOverrides)
                                {
                                    action.RemoveBindingOverride(1);
                                    action.RemoveBindingOverride(2);
                                }
                                action.ApplyBindingOverride(1, bindingList[1]);
                                action.ApplyBindingOverride(2, bindingList[2]);
                            }
                            else if (i == 1)
                            {
                                if (action.bindings[4].hasOverrides)
                                {
                                    action.RemoveBindingOverride(4);
                                    action.RemoveBindingOverride(5);
                                }
                                action.ApplyBindingOverride(4, bindingList[4]);
                                action.ApplyBindingOverride(5, bindingList[5]);
                            }
                        }
                    }
                    action.Enable();
                }
            }
            print("로드됨");
        }
    }

    public void LoadBindings()
    {
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        var filePath = Path.Combine(folderPath, $"KeyBindData.json");
        if (File.Exists(filePath))
        {
            ReLoadBindings(filePath);
        }
        else
        {
            ResetBindings();
        }
    }

    public void UpdateAllBindingButtonTexts()
    {
        // 모든 Keybinding 인스턴스에 대해 UpdateBindingButtonText 호출
        foreach (var keybinding in FindObjectsOfType<Keybinding>(true))
        {
            keybinding.UpdateBindingButtonText();
        }
    }

    public void LoadBindingsAndUpdateUI()
    {
        LoadBindings(); // 바인딩 로드
        UpdateAllBindingButtonTexts(); // UI 업데이트
    }
}
