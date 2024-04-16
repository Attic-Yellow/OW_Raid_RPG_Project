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
    [SerializeField] private SkillSlot slot;
    private GameObject dragVisual;

    private void Start()
    {
        slot = this;
    }

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            // �ð��� ǥ�� ����
            dragVisual = new GameObject("Drag Visual");
            dragVisual.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform); // Canvas�� �θ�� ����
            Image visualImage = dragVisual.AddComponent<Image>();
            visualImage.sprite = itemIcon.sprite; // ���� ������ ������ �̹��� ���
            visualImage.rectTransform.sizeDelta = new Vector2(60, 60); // ũ�� ����
            visualImage.raycastTarget = false; // �̺�Ʈ ����ĳ��Ʈ ����
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
        {
            dragVisual.transform.position = Input.mousePosition;
        }
    }

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
                slot.AssignSlot(this.slot);
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

    public override Skill GetSkill()
    {
        return skill;
    }
}