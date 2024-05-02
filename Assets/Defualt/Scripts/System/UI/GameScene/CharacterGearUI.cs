using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGearUI : MonoBehaviour
{
    #region ����� UI ������Ʈ
    [SerializeField] private GameObject characterGearUI;
    [SerializeField] private List<GameObject> characterGearButton;
    [SerializeField] private List<GameObject> characterGears;
    #endregion

    #region ���� �� ��� ���� ����Ʈ
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
    #endregion

    [SerializeField]private Equipped equipped;

    public EquipmentType equipmentType;

    LinkState linkState = LinkState.Idle;
    private bool isStarted = false;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.characterGearUI = this;
    }

    #region ��ŸƮ �޼���
    private void Start()
    {
        isStarted = true;

        equipped = Equipped.Instance;
        equipped.onChangeGear += ReadrawEquipSlotUI;

        if (characterGearUI != null)
        {
            characterGearUI.SetActive(false);
        }

        if (characterGears.Count > 0)
        {
            GearsController(0);
        }

        ResetEquipSlotUI();

        isStarted = false;
    }
    #endregion

    private List<EquippedSlot> GetEquipSlotList(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon: return weaponSlot;
            case EquipmentType.Head: return headSlot;
            case EquipmentType.Body: return bodySlot;
            case EquipmentType.Hands: return handsSlot;
            case EquipmentType.Legs: return legsSlot;
            case EquipmentType.Feet: return feetSlot;
            case EquipmentType.Auxiliary: return auxiliarySlot;
            case EquipmentType.Earring: return earringSlot;
            case EquipmentType.Necklace: return necklaceSlot;
            case EquipmentType.Bracelet: return braceletSlot;
            case EquipmentType.Ring: return ringSlot;
            default: return null;
        }
    }

    #region ����� UI �ֽ�ȭ �޼���
    public void ReadrawEquipSlotUI()
    {
        List<EquippedSlot> slots = GetEquipSlotList(equipmentType);
        int count = Mathf.Min(equipped.GetEquipmentList(equipmentType).Count, slots.Count);

        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }

        for (int i = 0; i < count; i++)
        {
            slots[i].equipment = equipped.GetEquipmentList(equipmentType)[i];
            slots[i].UpdateSlotUI();
        }

        if (!isStarted)
        {
            StartCoroutine(UpLoad(equipmentType));
        }
    }
    #endregion

    public void ResetEquipSlotUI()
    {
        EquipmentType[] equipmentTypes = new EquipmentType[]
        {
            EquipmentType.Weapon, EquipmentType.Head, EquipmentType.Body, EquipmentType.Hands, EquipmentType.Legs, EquipmentType.Feet,
            EquipmentType.Auxiliary, EquipmentType.Earring, EquipmentType.Necklace, EquipmentType.Bracelet, EquipmentType.Ring
        };

        foreach (var equipmentType in equipmentTypes)
        {
            this.equipmentType = equipmentType;
            ReadrawEquipSlotUI();
        }
    }

    #region ����� UI ��Ʈ�ѷ�
    // ����� ��Ʈ�ѷ�
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

    // ����� ��� ��ư ��Ʈ�ѷ�
    public void GearsController(int index)
    {
        if (characterGears.Count > 0)
        {
            for (int i = 0; i < characterGears.Count; i++)
            {
                characterGears[i].gameObject.SetActive(i == index);
            }
        }

        if (characterGearButton.Count > 0)
        {
            for (int i = 0; i < characterGearButton.Count; i++)
            {
                var img = characterGearButton[i].gameObject.GetComponent<Image>();

                if (i == index)
                {
                    Color newColor = img.color;
                    newColor.a = 1f;
                    img.color = newColor;
                }
                else
                {
                    Color newColor = img.color;
                    newColor.a = 0.4f;
                    img.color = newColor;
                }
            }
        }
    }
    #endregion

    #region ����� ������ ���ε�
    private IEnumerator UpLoad(EquipmentType equipmentType)
    {
        linkState = LinkState.UpLoad;

        UpLoadAsync(equipmentType);

        yield return new WaitUntil(() => (linkState == LinkState.Idle));
    }

    private async void UpLoadAsync(EquipmentType equipmentType)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        var charInfo = GameManager.Instance.dataManager.characterData.characterData;

        await FirebaseManager.Instance.UpLoadEquipped(user.UserId, user.Email, charInfo["server"].ToString(), charInfo["charId"].ToString(), equipmentType);

        linkState = LinkState.Idle;
    }
    #endregion
}

#region (��) ����� ���� �� UI �ֽ�ȭ �޼���
//// ����� - ���� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawWeaponSlotUI()
//{
//    foreach (var slot in weaponSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.weapon.Count, weaponSlot.Count);
//    for (int i = 0; i < count; i++)
//    {
//        weaponSlot[i].equipment = equipped.weapon[i];
//        weaponSlot[i].UpdateSlotUI();
//    }

//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Weapon));
//    }
//}

//// ����� - �Ӹ� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawHeadSlotUI()
//{
//    foreach (var slot in headSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.head.Count, headSlot.Count);
//    for (int i = 0; i < equipped.head.Count; i++)
//    {
//        headSlot[i].equipment = equipped.head[i];
//        headSlot[i].UpdateSlotUI();
//    }

//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Head));
//    }
//}

//// ����� - ���� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawBodySlotUI()
//{
//    foreach (var slot in bodySlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.weapon.Count, bodySlot.Count);
//    for (int i = 0; i < equipped.body.Count; i++)
//    {
//        bodySlot[i].equipment = equipped.body[i];
//        bodySlot[i].UpdateSlotUI();
//    }

//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Body));
//    }
//}

//// ����� - �� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawHandsSlotUI()
//{
//    foreach (var slot in handsSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.hands.Count, handsSlot.Count);
//    for (int i = 0; i < equipped.hands.Count; i++)
//    {
//        handsSlot[i].equipment = equipped.hands[i];
//        handsSlot[i].UpdateSlotUI();
//    }

//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Hands));
//    }
//}

//// ����� - �ٸ� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawLegsSlotUI()
//{
//    foreach (var slot in legsSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.legs.Count, legsSlot.Count);
//    for (int i = 0; i < equipped.legs.Count; i++)
//    {
//        legsSlot[i].equipment = equipped.legs[i];
//        legsSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Legs));
//    }
//}

//// ����� - �Ź� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawFeetSlotUI()
//{
//    foreach (var slot in feetSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.feet.Count, feetSlot.Count);
//    for (int i = 0; i < equipped.feet.Count; i++)
//    {
//        feetSlot[i].equipment = equipped.feet[i];
//        feetSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Feet));
//    }
//}

//// ����� - ���� ���� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawAuxiliarySlotUI()
//{
//    foreach (var slot in auxiliarySlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.auxiliary.Count, auxiliarySlot.Count);
//    for (int i = 0; i < equipped.auxiliary.Count; i++)
//    {
//        auxiliarySlot[i].equipment = equipped.auxiliary[i];
//        auxiliarySlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Auxiliary));
//    }
//}

//// ����� - �Ͱ��� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawEarringSlotUI()
//{
//    foreach (var slot in earringSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.earring.Count, earringSlot.Count);
//    for (int i = 0; i < equipped.earring.Count; i++)
//    {
//        earringSlot[i].equipment = equipped.earring[i];
//        earringSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Earring));
//    }
//}

//// ����� - ����� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawNecklaceSlotUI()
//{
//    foreach (var slot in necklaceSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.necklace.Count, necklaceSlot.Count);
//    for (int i = 0; i < equipped.necklace.Count; i++)
//    {
//        necklaceSlot[i].equipment = equipped.necklace[i];
//        necklaceSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Necklace));
//    }
//}

//// ����� - ���� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawBraceletSlotUI()
//{
//    foreach (var slot in braceletSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.bracelet.Count, braceletSlot.Count);
//    for (int i = 0; i < equipped.bracelet.Count; i++)
//    {
//        braceletSlot[i].equipment = equipped.bracelet[i];
//        braceletSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Bracelet));
//    }
//}

//// ����� - ���� ĭ UI �ֽ�ȭ �޼���
//public void ReadrawRingSlotUI()
//{
//    foreach (var slot in ringSlot)
//    {
//        slot.ClearSlot();
//    }
//    int count = Mathf.Min(equipped.ring.Count, ringSlot.Count);
//    for (int i = 0; i < equipped.ring.Count; i++)
//    {
//        ringSlot[i].equipment = equipped.ring[i];
//        ringSlot[i].UpdateSlotUI();
//    }
//    if (!isStarted)
//    {
//        StartCoroutine(UpLoad(EquipmentType.Ring));
//    }
//}
#endregion
