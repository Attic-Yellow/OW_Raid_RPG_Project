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
    private Consumable tempConsumable; // 임시로 저장할 아이템 데이터
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

    #region 드래그 시작
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition; // 마우스 위치로 시각적 표현 이동
        }
    }
    #endregion

    #region 드래그 종료
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

        List<RaycastResult> hits = new List<RaycastResult>(); // 마우스 포인터 아래의 "Slot" 태그를 가진 오브젝트만 검사
        EventSystem.current.RaycastAll(eventData, hits);
        RaycastResult? hit = hits.FirstOrDefault(h => h.gameObject.CompareTag("Slot"));

        if (hit.HasValue && hit.Value.gameObject != null)
        {
            
            Slot slot = hit.Value.gameObject.GetComponent<Slot>(); // 드랍 위치의 슬롯 처리

            if (slot != null)
            {
                
                if (slot.slotType == SlotType.Item) // 인벤토리 슬롯 드랍 성공: 아이템을 새 슬롯에 할당
                {
                    slot.AssignItem(tempConsumable, slotIndex, slot.slotIndex, this.slot.slotType, slot.slotType, tempConsumable.itemCount); 
                    
                    var linked = slot.GetComponent<InventorySlot>();

                    if (linkedQuick.Count > 0) // 연결 된 퀵 슬롯이 있다면
                    {
                        int count = linkedQuick.Count;
                        for (int i = 0; i < count; i++)
                        {
                            linkedQuick[0].GetComponent<QuickSlot>().slot = slot.gameObject;
                            linked.linkedQuick.Add(linkedQuick[0]);
                            linkedQuick.RemoveAt(0); // 연결 된 퀵 슬롯 삭제
                        }
                    }

                    slot.UpdateSlotUI();
                }
                else if (slot.slotType == SlotType.Quick) // 퀵 슬롯 드랍 성공: 아이템을 새 슬롯에 공유
                {
                    linkedQuick.Add(slot.gameObject); // 연결 된 퀵 슬롯 저장
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
            consumable = tempConsumable; // 드랍 실패: 원래 슬롯에 아이템을 다시 할당
            AssignItem(tempConsumable, slotIndex, slotIndex, slotType, slotType, tempConsumable.itemCount);
            UpdateSlotUI();
        }

        tempConsumable = null; // 임시 데이터 초기화
        dragType = DragType.Idle; // 드래그 종료 상태로 변경
    }
    #endregion

    #region 드래그 중
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (consumable != null && consumable.itemImage != null)
        {
            dragType = DragType.Drag; // 드래그 중 상태로 변경
            tempConsumable = consumable;

            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(50, 50); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시

            Inventory.Instance.RemoveItem(consumable, slotIndex);
            ClearSlot(); // 슬롯 클리어
        }
    }
    #endregion

    #region 슬롯 할당
    public override void AssignItem(Consumable consumable, int oldIndex, int newIndex, SlotType oldSlotType, SlotType newSlotType, int itemCount)
    {
        this.consumable = consumable;
        Inventory.Instance.AssignItemAtIndex(consumable, oldIndex, newIndex, oldSlotType, newSlotType, itemCount);
    }
    #endregion

    #region 현재 슬롯 아이템 반환
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
