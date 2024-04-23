using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData Instance;

    public List<Consumable> items = new List<Consumable>();
    public Dictionary<int, Equipment> equip = new Dictionary<int, Equipment>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadEquipmentData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadEquipmentData()
    {
        string assetBundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundlePath, "DataFolder"));

        if (assetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }

        TextAsset jsonFile = assetBundle.LoadAsset<TextAsset>("EquipmentData.json"); // "equipment_data.json"는 예제 JSON 파일명
        if (jsonFile != null)
        {
            Dictionary<string, Equipment> loadedData = JsonConvert.DeserializeObject<Dictionary<string, Equipment>>(jsonFile.text);
            foreach (var item in loadedData)
            {
                equip[int.Parse(item.Key)] = item.Value;
            }
        }
        else
        {
            Debug.LogError("Failed to load JSON file from AssetBundle!");
        }

        assetBundle.Unload(false);
    }
}
