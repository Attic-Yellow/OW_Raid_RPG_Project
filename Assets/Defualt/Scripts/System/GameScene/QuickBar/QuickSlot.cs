using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class QuickSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public GameObject slot;
    private GameObject dragVisual;
    private GameObject tempSlot;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private GameObject coolDownImage;
    [SerializeField] private TextMeshProUGUI coolDownText;
    [SerializeField] private GameObject skillContent;

    private void Start()
    {
        ApplyHUDSettings();
    }

    #region 슬롯 UI 업데이트
    public override void UpdateSlotUI()
    {
        if (slot == null)
        {
            ClearSlot();
            return;
        }

        Slot linkSlot = slot.GetComponent<Slot>();

        switch (linkSlot.slotType)
        {
            case SlotType.Item:
                var s = slot.GetComponent<InventorySlot>();
                if (s.consumable != null)
                {
                    itemIcon.sprite = IconData.Instance.GetitemIcon(s.consumable.itemImage);
                    itemCountText.text = s.consumable.itemCount.ToString();
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    ClearSlot();
                }
                break;
            case SlotType.Equipment:
                // 아이템 또는 장비 슬롯 참조
                Equipment equipment = linkSlot.GetEquipment();
                if (equipment != null)
                {
                    itemIcon.sprite = IconData.Instance.GetitemIcon(equipment.itemImage);
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    ClearSlot();
                }
                break;
            case SlotType.Skill:
                Skill skill = linkSlot.GetSkill();
                if (skill != null)
                {
                    itemIcon.sprite = skill.skillIcon;
                    itemIcon.gameObject.SetActive(true);
                }
                else
                {
                    ClearSlot();
                }
                break;
        }
    }
    #endregion

    #region 슬롯 초기화
    public override void ClearSlot()
    {
        slot = null;
        itemIcon.sprite = null;
        itemCountText.text = "";
        itemIcon.gameObject.SetActive(false);
    }
    #endregion

    #region 드래그 시작
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // 마우스 위치로 시각적 표현 이동
        }
    }
    #endregion

    #region 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        if (tempSlot == null)
        {
            return;
        }

        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }
        // 마우스 포인터 아래의 "Slot" 태그를 가진 오브젝트만 검사
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // 드랍 위치의 슬롯 처리
            QuickSlot slot = hit.Value.gameObject.GetComponent<QuickSlot>();
            Slot allSlotType = hit.Value.gameObject.GetComponent<Slot>();

            if (tempSlot != null && slot != null && slot.slot != null && allSlotType.slotType == SlotType.Quick)
            {
                Slot linkSlot = tempSlot.GetComponent<Slot>();
                Slot hitLinkSlot = slot.slot.GetComponent<Slot>();
                
                if (linkSlot.slotType == SlotType.Item && hitLinkSlot.slotType == SlotType.Item)
                {
                    var invenSlot = slot.slot.GetComponent<InventorySlot>();
                    var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();
                    invenSlot.AddLinked(gameObject);
                    invenSlot.RemoveLinked(slot.gameObject);
                    thisInvenSlot.AddLinked(slot.gameObject);
                }
                else if (linkSlot.slotType == SlotType.Skill && hitLinkSlot.slotType == SlotType.Item)
                {
                    var invenSlot = slot.slot.GetComponent<InventorySlot>();
                    invenSlot.AddLinked(gameObject);
                    invenSlot.RemoveLinked(slot.gameObject);
                }
                else if (linkSlot.slotType == SlotType.Item && hitLinkSlot.slotType == SlotType.Skill)
                {
                    var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();
                    thisInvenSlot.AddLinked(slot.gameObject);
                }

                // 드랍 성공: 아이템을 새 슬롯에 할당
                this.slot = slot.slot;
                slot.AssignSlot(tempSlot.gameObject);
                slot.UpdateSlotUI();
            }
            else if (allSlotType.slotType == SlotType.Quick)
            {
                slot.AssignSlot(tempSlot);
                slot.UpdateSlotUI();
                var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();

                if (thisInvenSlot != null)
                {
                    thisInvenSlot.AddLinked(slot.gameObject);
                }
            }
            else if (allSlotType.slotType != SlotType.Quick)
            {
                this.slot = tempSlot;
                AssignSlot(tempSlot);
                UpdateSlotUI(); 
                var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();

                if (thisInvenSlot != null)
                {
                    thisInvenSlot.AddLinked(gameObject);
                }
            }
        }
        UpdateSlotUI();
        tempSlot = null;
    }
    #endregion

    #region 드래그 중
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot != null)
        {
            tempSlot = slot;
            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(FindObjectOfType<Canvas>().transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시

            Slot linkSlot = tempSlot.GetComponent<Slot>();

            if (linkSlot.slotType == SlotType.Item)
            {
                var invenSlot = slot.GetComponent<InventorySlot>();
                invenSlot.RemoveLinked(gameObject);
            }

            ClearSlot(); // 슬롯 클리어
        }
    }
    #endregion

    #region 퀵 슬롯 할당
    public override void AssignSlot(GameObject slot)
    {
        this.slot = slot;
        UpdateSlotUI();

        SlotDataList slotDataList = new SlotDataList();

        var data = GameManager.Instance.uiManager.gameSceneUI.quickSlotData.slotDataList;
        string name = gameObject.name.ToString();

        slotDataList.slotName = slot.transform.name;
        slotDataList.slotType = slot.GetComponent<Slot>().slotType.ToString();

        if (!data.ContainsKey(name))
        {
            data.Add(name, slotDataList);
        }
        else
        {
            data[name] = slotDataList;
        }

        SaveSlotData();
    }
    #endregion

    #region 슬롯 정보 Json 저장
    public void SaveSlotData()
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(GameManager.Instance.uiManager.gameSceneUI.quickSlotData, settings);
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        // 폴더가 존재하지 않는다면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, $"slotData.json");

        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, json);
    }
    #endregion

    #region 슬롯 정보 Json 불러오기
    public void ApplyHUDSettings()
    {
        var charName = CharacterData.Instance.characterData;
        string name = charName.ContainsKey("name") ? charName["name"].ToString() : "null";

        string folderPath = Path.Combine(Application.persistentDataPath, name);

        var filePath = Path.Combine(folderPath, $"slotData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            QuickSlotData slotData = JsonConvert.DeserializeObject<QuickSlotData>(json);

            string objectName = gameObject.name.ToString();
            var data = slotData.slotDataList[objectName];

            if (data.slotType == "Skill")
            {
                slot = skillContent.transform.Find(data.slotName).gameObject;
            }
            
            UpdateSlotUI();
        }
    }
    #endregion
}
