using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGearUI : MonoBehaviour
{
    [SerializeField] private GameObject characterGearUI;
    [SerializeField] private List<GameObject> characterGears;
    [SerializeField] private List<EquippedSlot> weaponSlot;
    [SerializeField] private List<EquippedSlot> headSlot;
    [SerializeField] private List<EquippedSlot> bodySlot;
    [SerializeField] private List<EquippedSlot> handsSlot;
    [SerializeField] private List<EquippedSlot> legsSlot;
    [SerializeField] private List<EquippedSlot> feetSlot;
    [SerializeField] private List<EquippedSlot> auxiliarySlot;
    [SerializeField] private List<EquippedSlot> earringSlot;
    [SerializeField] private List<EquippedSlot> necklaceSlot;
    [SerializeField] private List<EquippedSlot> braceletSlot;
    [SerializeField] private List<EquippedSlot> ringSlot;

    [SerializeField]private Equipped equipped;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterGearUI = this;
    }

    private void Start()
    {
        equipped = Equipped.Instance;
        equipped.onChangeGear += ReadrawWeaponSlotUI;
        equipped.onChangeGear += ReadrawHeadSlotUI;
        equipped.onChangeGear += ReadrawBodySlotUI;
        equipped.onChangeGear += ReadrawHandsSlotUI;
        equipped.onChangeGear += ReadrawLegsSlotUI;
        equipped.onChangeGear += ReadrawFeetSlotUI;
        equipped.onChangeGear += ReadrawAuxiliarySlotUI;
        equipped.onChangeGear += ReadrawEarringSlotUI;
        equipped.onChangeGear += ReadrawNecklaceSlotUI;
        equipped.onChangeGear += ReadrawBraceletSlotUI;
        equipped.onChangeGear += ReadrawRingSlotUI;
        if (characterGearUI != null)
        {
            characterGearUI.SetActive(false);
        }

        if (characterGears.Count > 0)
        {
            GearsController(0);
        }
    }

    // 장비함 컨트롤러
    public void GearUIController()
    {
        if (characterGearUI != null)
        {
            characterGearUI.SetActive(!characterGearUI.activeInHierarchy);

            if (characterGearUI.activeSelf)
            {
                characterGearUI.transform.SetAsLastSibling();
            }
        }
    }

    // 장비함 장비별 버튼 컨트롤러
    public void GearsController(int index)
    {
        if (characterGears.Count > 0)
        {
            for (int i = 0; i < characterGears.Count; i++)
            {
                characterGears[i].gameObject.SetActive(i == index);
            }
        }
    }

    // 장비함 - 무기 칸 UI 최신화 메서드
    public void ReadrawWeaponSlotUI()
    {
        foreach (var slot in weaponSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.weapon.Count, weaponSlot.Count);
        for (int i = 0; i < count; i++)
        {
            weaponSlot[i].equipment = equipped.weapon[i];
            weaponSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 머리 칸 UI 최신화 메서드
    public void ReadrawHeadSlotUI()
    {
        foreach (var slot in headSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.head.Count, headSlot.Count);
        for (int i = 0; i < equipped.head.Count; i++)
        {
            headSlot[i].equipment = equipped.head[i];
            headSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 몸통 칸 UI 최신화 메서드
    public void ReadrawBodySlotUI()
    {
        foreach (var slot in bodySlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.weapon.Count, bodySlot.Count);
        for (int i = 0; i < equipped.body.Count; i++)
        {
            bodySlot[i].equipment = equipped.body[i];
            bodySlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 손 칸 UI 최신화 메서드
    public void ReadrawHandsSlotUI()
    {
        foreach (var slot in handsSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.hands.Count, handsSlot.Count);
        for (int i = 0; i < equipped.hands.Count; i++)
        {
            handsSlot[i].equipment = equipped.hands[i];
            handsSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 다리 칸 UI 최신화 메서드
    public void ReadrawLegsSlotUI()
    {
        foreach (var slot in legsSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.legs.Count, legsSlot.Count);
        for (int i = 0; i < equipped.legs.Count; i++)
        {
            legsSlot[i].equipment = equipped.legs[i];
            legsSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 신발 칸 UI 최신화 메서드
    public void ReadrawFeetSlotUI()
    {
        foreach (var slot in feetSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.feet.Count, feetSlot.Count);
        for (int i = 0; i < equipped.feet.Count; i++)
        {
            feetSlot[i].equipment = equipped.feet[i];
            feetSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 보조 도구 칸 UI 최신화 메서드
    public void ReadrawAuxiliarySlotUI()
    {
        foreach (var slot in auxiliarySlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.auxiliary.Count, auxiliarySlot.Count);
        for (int i = 0; i < equipped.auxiliary.Count; i++)
        {
            auxiliarySlot[i].equipment = equipped.auxiliary[i];
            auxiliarySlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 귀걸이 칸 UI 최신화 메서드
    public void ReadrawEarringSlotUI()
    {
        foreach (var slot in earringSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.earring.Count, earringSlot.Count);
        for (int i = 0; i < equipped.earring.Count; i++)
        {
            earringSlot[i].equipment = equipped.earring[i];
            earringSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 목걸이 칸 UI 최신화 메서드
    public void ReadrawNecklaceSlotUI()
    {
        foreach (var slot in necklaceSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.necklace.Count, necklaceSlot.Count);
        for (int i = 0; i < equipped.necklace.Count; i++)
        {
            necklaceSlot[i].equipment = equipped.necklace[i];
            necklaceSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 팔찌 칸 UI 최신화 메서드
    public void ReadrawBraceletSlotUI()
    {
        foreach (var slot in braceletSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.bracelet.Count, braceletSlot.Count);
        for (int i = 0; i < equipped.bracelet.Count; i++)
        {
            braceletSlot[i].equipment = equipped.bracelet[i];
            braceletSlot[i].UpdateSlotUI();
        }
    }

    // 장비함 - 반지 칸 UI 최신화 메서드
    public void ReadrawRingSlotUI()
    {
        foreach (var slot in ringSlot)
        {
            slot.ClearSlot();
        }
        int count = Mathf.Min(equipped.ring.Count, ringSlot.Count);
        for (int i = 0; i < equipped.ring.Count; i++)
        {
            ringSlot[i].equipment = equipped.ring[i];
            ringSlot[i].UpdateSlotUI();
        }
    }
}
