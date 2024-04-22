using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuickSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public GameObject slot;
    private GameObject dragVisual;
    private GameObject tempSlot;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private GameObject coolDownImage;
    [SerializeField] private TextMeshProUGUI coolDownText;

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

    public override void ClearSlot()
    {
        slot = null;
        itemIcon.sprite = null;
        itemCountText.text = "";
        itemIcon.gameObject.SetActive(false);
    }

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
                slot.AssignSlot(tempSlot.gameObject);
                slot.UpdateSlotUI();
                var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();
                thisInvenSlot.AddLinked(slot.gameObject);
            }
            else if (allSlotType.slotType != SlotType.Quick)
            {
                this.slot = tempSlot;
                AssignSlot(tempSlot.gameObject);
                UpdateSlotUI(); 
                var thisInvenSlot = tempSlot.GetComponent<InventorySlot>();
                thisInvenSlot.AddLinked(gameObject);
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
    }
    #endregion
}
