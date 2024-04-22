using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Consumable consumable;
    private GameObject dragVisual;
    private Consumable tempConsumable; // �ӽ÷� ������ ������ ������
    private DragType dragType;
    [SerializeField] private InventorySlot slot;
    [SerializeField] private List<GameObject> linkedQuick;

    [SerializeField] private TextMeshProUGUI countText;

    private void Start()
    {
        slot = this;
    }

    public override void UpdateSlotUI()
    {
        itemIcon.sprite = consumable.itemImage;
        countText.text = consumable.itemCount.ToString();

        if (itemIcon.sprite != null)
        {
            countText.gameObject.SetActive(true);
            itemIcon.gameObject.SetActive(true);
        }

        if (dragType == DragType.Idle && linkedQuick.Count > 0)
        {
            for (int i = 0; i < linkedQuick.Count; i++)
            {
                var quickSlot = linkedQuick[i].GetComponent<QuickSlot>();
                quickSlot.UpdateSlotUI();
            }
        }
    }

    public override void ClearSlot()
    {
        consumable = null;
        itemIcon.sprite = null;
        countText.text = null;
        countText.gameObject.SetActive(false);
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

    #region �巡�� ����
    public void OnEndDrag(PointerEventData eventData)
    {
        if (tempConsumable == null)
        {
            return;
        }

        if (dragVisual != null)
        {
            Destroy(dragVisual);
        }

        List<RaycastResult> hits = new List<RaycastResult>(); // ���콺 ������ �Ʒ��� "Slot" �±׸� ���� ������Ʈ�� �˻�
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            
            Slot slot = hit.Value.gameObject.GetComponent<Slot>(); // ��� ��ġ�� ���� ó��

            if (slot != null)
            {
                
                if (slot.slotType == SlotType.Item) // �κ��丮 ���� ��� ����: �������� �� ���Կ� �Ҵ�
                {
                    slot.AssignItem(tempConsumable, slotIndex, slot.slotIndex, this.slot.slotType, slot.slotType, tempConsumable.itemCount); 
                    
                    var linked = slot.GetComponent<InventorySlot>();

                    if (linkedQuick.Count > 0) // ���� �� �� ������ �ִٸ�
                    {
                        int count = linkedQuick.Count;
                        for (int i = 0; i < count; i++)
                        {
                            linkedQuick[0].GetComponent<QuickSlot>().slot = slot.gameObject;
                            linked.linkedQuick.Add(linkedQuick[0]);
                            linkedQuick.RemoveAt(0); // ���� �� �� ���� ����
                        }
                    }

                    slot.UpdateSlotUI();
                }
                else if (slot.slotType == SlotType.Quick) // �� ���� ��� ����: �������� �� ���Կ� ����
                {
                    linkedQuick.Add(slot.gameObject); // ���� �� �� ���� ����
                    consumable = tempConsumable;
                    slot.AssignSlot(gameObject);
                    slot.UpdateSlotUI(); 
                    consumable = tempConsumable;
                    AssignItem(tempConsumable, slotIndex, slotIndex, slotType, slotType, tempConsumable.itemCount);
                    UpdateSlotUI();
                }
            }
            else
            {
                consumable = tempConsumable;
                AssignItem(tempConsumable, slotIndex, slotIndex, slotType, slotType, tempConsumable.itemCount);
                UpdateSlotUI();
            }
        }
        else
        {
            consumable = tempConsumable; // ��� ����: ���� ���Կ� �������� �ٽ� �Ҵ�
            AssignItem(tempConsumable, slotIndex, slotIndex, slotType, slotType, tempConsumable.itemCount);
            UpdateSlotUI();
        }

        tempConsumable = null; // �ӽ� ������ �ʱ�ȭ
        dragType = DragType.Idle; // �巡�� ���� ���·� ����
    }
    #endregion

    #region �巡�� ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (consumable != null && consumable.itemImage != null)
        {
            dragType = DragType.Drag; // �巡�� �� ���·� ����
            tempConsumable = consumable;

            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(50, 50); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����

            Inventory.Instance.RemoveItem(consumable, slotIndex);
            ClearSlot(); // ���� Ŭ����
        }
    }
    #endregion

    #region ���� �Ҵ�
    public override void AssignItem(Consumable consumable, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType, int itemCount)
    {
        this.consumable = consumable;
        Inventory.Instance.AssignItemAtIndex(consumable, oldIndex, newIndex, oldSlotType, newSlotType, itemCount);
    }
    #endregion

    #region ���� ���� ������ ��ȯ
    public override Consumable GetItem()
    {
        return consumable;
    }
    #endregion

    public void AddLinked(GameObject quickSlot)
    {
        linkedQuick.Add(quickSlot);
    }

    public void RemoveLinked(GameObject quickSlot)
    {
        linkedQuick.Remove(quickSlot);
    }
}
