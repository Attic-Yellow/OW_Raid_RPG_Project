using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SkillSlot : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillInfo;
    [SerializeField] private Skill skill;

    private GameObject dragVisual;
    public SkillSlot slot;

    #region 스킬 정보 UI 업데이트
    public void SkillInfo(Skill skill)
    {
        this.skill = skill;
        skillName.text = skill.skillName;
        itemIcon.sprite = skill.skillIcon;
        if (skill.useMana != 0)
        {
            skillInfo.text = $"레벨: {skill.useLevel} 사용 MP: {skill.useMana}";
        }
        else
        {
            skillInfo.text = $"레벨: {skill.useLevel}";
        }
    }
    #endregion

    #region 드래그 시작
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition;
        }
    }
    #endregion

    #region 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
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
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            DraggableUI draggableUI = slot.gameObject.GetComponentInParent<RectTransform>().gameObject.GetComponentInParent<DraggableUI>();
            draggableUI.SetAsLast();

            if (skill != null && slot != null)
            {
                // 드랍 성공: 아이템을 새 슬롯에 할당
                slot.AssignSlot(this.slot.gameObject);
                slot.UpdateSlotUI();
            }
            else
            {
                Destroy(dragVisual);
            }
        }
        else
        {
            Destroy(dragVisual);
        }
    }
    #endregion

    #region 드래그 중
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            // 시각적 표현 생성
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameManager.Instance.uiManager.gameSceneUI.canvas.transform); // Canvas를 부모로 설정
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // 현재 슬롯의 아이템 이미지 사용
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // 크기 조절
            visualImage.raycastTarget = false; // 이벤트 레이캐스트 무시
        }
    }
    #endregion

    #region 스킬 반환
    public override Skill GetSkill()
    {
        return skill;
    }

    public override void Use()
    {
        if (skill != null)
        {
            print($"{skill.skillName}");
             skill.UseSkill(skill.GetSkillID()); 
        }
        else
        {
            print("skill 없음");
        }
    }
    #endregion
}
