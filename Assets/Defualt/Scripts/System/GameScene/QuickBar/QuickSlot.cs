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
                // ������ �Ǵ� ��� ���� ����
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

    #region �巡�� ����
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // ���콺 ��ġ�� �ð��� ǥ�� �̵�
        }
    }
    #endregion

    #region �巡�� ��
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
        // ���콺 ������ �Ʒ��� "Slot" �±׸� ���� ������Ʈ�� �˻�
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            // ��� ��ġ�� ���� ó��
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

                // ��� ����: �������� �� ���Կ� �Ҵ�
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

    #region �巡�� ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot != null)
        {
            tempSlot = slot;
            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����

            Slot linkSlot = tempSlot.GetComponent<Slot>();

            if (linkSlot.slotType == SlotType.Item)
            {
                var invenSlot = slot.GetComponent<InventorySlot>();
                invenSlot.RemoveLinked(gameObject);
            }

            ClearSlot(); // ���� Ŭ����
        }
    }
    #endregion

    #region �� ���� �Ҵ�
    public override void AssignSlot(GameObject slot)
    {
        this.slot = slot;
        UpdateSlotUI();
    }
    #endregion
}
