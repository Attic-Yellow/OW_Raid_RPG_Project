using Firebase.Firestore;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickBar : MonoBehaviour
{
    [SerializeField] private GameObject quickBar;
    [SerializeField] private GameObject dragArea;
    [SerializeField] private Slider slider;
    [SerializeField] private Image[] sizeButton;
    [SerializeField] private int selectedShape;
    [SerializeField] private int quickBarNum;

    private string defaultFilePath = "Assets/StreamingAssets/DefaultHUDData.json";

    private void Start()
    {
        // slider.onValueChanged.AddListener(delegate { ChangeQuickBarSize(); });
        UpdateHUDData(-1);
        ApplyHUDSettings();
    }

    public void ChangeQuickBarSize()
    {
        float quickBarSize = ((float)slider.value / 100);
        var rect = quickBar.GetComponent<RectTransform>();
        var rectDrag = dragArea.GetComponent<RectTransform>();

        rect.localScale = new Vector2(quickBarSize, quickBarSize);
        rectDrag.localScale = new Vector2(quickBarSize, quickBarSize);
    }

    public void ChangeQuickBarShape(int index)
    {
        var rect = quickBar.GetComponent<RectTransform>();
        var rectDrag = dragArea.GetComponent<RectTransform>();

        switch (index)
        {
            case 0:
                rect.sizeDelta = new Vector2(850, 80);
                rectDrag.sizeDelta = new Vector2(835, 65);
                break;
            case 1:
                rect.sizeDelta = new Vector2(430, 150);
                rectDrag.sizeDelta = new Vector2(415, 135);
                break;
            case 2:
                rect.sizeDelta = new Vector2(290, 220);
                rectDrag.sizeDelta = new Vector2(275, 205);
                break;
            case 3:
                rect.sizeDelta = new Vector2(220, 290);
                rectDrag.sizeDelta = new Vector2(205, 275);
                break;
            case 4:
                rect.sizeDelta = new Vector2(150, 430);
                rectDrag.sizeDelta = new Vector2(135, 415);
                break;
            case 5:
                rect.sizeDelta = new Vector2(80, 850);
                rectDrag.sizeDelta = new Vector2(65, 835);
                break;
        }

        ChangeButtonColor(index);
        UpdateHUDData(index);
    }

    private void ChangeButtonColor(int index)
    {
        if (sizeButton.Length > 0)
        {
            selectedShape = index;
            for (int i = 0; i < sizeButton.Length; i++)
            {
                sizeButton[i].color = index == i ? Color.yellow : Color.white;
            }
        }
    }

    public void UpdateHUDData(int index)
    {
        if (index == -1)
        {
            selectedShape = quickBarNum > 5 ? 5 : 0;
        }

        HUDDataList hudDataList = new HUDDataList();
        var transform = quickBar.transform.GetComponent<RectTransform>();
        hudDataList.positionX = transform.position.x;
        hudDataList.positionY = transform.position.y;
        hudDataList.size = slider.value;
        hudDataList.quickBarShape = selectedShape;
        bool active = quickBar.transform.GetChild(0).gameObject.activeInHierarchy;
        hudDataList.isActive = active;

        var data = GameManager.Instance.uiManager.gameSceneUI.hudController.HUDData.hudDataList;
        string name = quickBar.gameObject.name.ToString();

        if (!data.ContainsKey(name))
        {
            data.Add(name, hudDataList);
        }
        else
        {
            data[name] = hudDataList;
        }
    }

    public void SaveHUDData()
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(GameManager.Instance.uiManager.gameSceneUI.hudController.HUDData, settings);
        var filePath = Path.Combine(Application.persistentDataPath, "HUDData.json");

        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, json);
    }

    public void ApplyHUDSettings()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "HUDData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HUDData hudData = JsonConvert.DeserializeObject<HUDData>(json); 

            string name = quickBar.gameObject.name.ToString();
            var data = hudData.hudDataList[name];
            var quickBarTransform = quickBar.transform.GetComponent<RectTransform>();
            var dragArearTrans = dragArea.transform.GetComponent<RectTransform>();
            quickBarTransform.position = new Vector2(data.positionX, data.positionY);
            dragArearTrans.position = new Vector2(data.positionX, data.positionY);
            slider.value = data.size;
            ChangeQuickBarSize();
            ChangeQuickBarShape(data.quickBarShape);
            quickBar.SetActive(data.isActive);
        }
        else
        {
            UpdateHUDData(-2);
        }
    }
}
