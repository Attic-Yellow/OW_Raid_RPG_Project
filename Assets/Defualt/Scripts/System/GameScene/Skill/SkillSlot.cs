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

    #region ��ų ���� UI ������Ʈ
    public void SkillInfo(Skill skill)
    {
        this.skill = skill;
        skillName.text = skill.skillName;
        itemIcon.sprite = skill.skillIcon;
        if (skill.useMana != 0)
        {
            skillInfo.text = $"����: {skill.useLevel} ��� MP: {skill.useMana}";
        }
        else
        {
            skillInfo.text = $"����: {skill.useLevel}";
        }
    }
    #endregion

    #region �巡�� ����
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition;
        }
    }
    #endregion

    #region �巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {
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
            Slot slot = hit.Value.gameObject.GetComponent<Slot>();
            DraggableUI draggableUI = slot.gameObject.GetComponentInParent<RectTransform>().gameObject.GetComponentInParent<DraggableUI>();
            draggableUI.SetAsLast();

            if (skill != null && slot != null)
            {
                // ��� ����: �������� �� ���Կ� �Ҵ�
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

    #region �巡�� ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameManager.Instance.uiManager.gameSceneUI.canvas.transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����
        }
    }
    #endregion

    #region ��ų ��ȯ
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
            print("skill ����");
        }
    }
    #endregion
}
