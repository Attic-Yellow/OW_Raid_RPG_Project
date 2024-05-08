using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region 소지함 UI 오브젝트
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private List<GameObject> numberButton;
    [SerializeField] private List<GameObject> inventorySlotAreas;
    [SerializeField] private List<InventorySlot> inventorySlots;
    #endregion

    [SerializeField] private bool isStarted;
    [SerializeField] private LinkState linkState;
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        GameManager.Instance.uiManager.gameSceneUI.inventoryUI = this;
    }

    #region 소지함UI 스타트 메서드
    private void Start()
    {
        isStarted = true;
        inventory = Inventory.Instance;
        inventory.onChangeItem += ReadrawSlotUI;

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }

        InventorySlotsAreasController(0);

        ReadrawSlotUI();
        isStarted = false;
    }
    #endregion

    #region 소지함 컨트롤러 메서드
    // 소지함 활성화/비활성화 컨트롤러
    public void InventoryController()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);

            if (inventoryUI.activeSelf)
            {
                inventoryUI.transform.SetAsLastSibling();
            }
        }
    }

    // 소지함 영역 선택 컨트롤러
    public void InventorySlotsAreasController(int index)
    {
        if (inventorySlotAreas.Count > 0)
        {
            for (int i = 0; i < inventorySlotAreas.Count; i++)
            {
                inventorySlotAreas[i].SetActive(i == index);
            }
        }

        if (numberButton.Count > 0)
        {
            for (int i = 0; i < numberButton.Count; i++)
            {
                var img = numberButton[i].gameObject.GetComponent<Image>();

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

    #region 소지함 UI 최신화 메서드
    // 소지함 UI 최신화 메서드
    public void ReadrawSlotUI()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearSlot();
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            inventorySlots[i].consumable = inventory.items[i];
            inventorySlots[i].UpdateSlotUI();
        }

        if (!isStarted)
        {
            StartCoroutine(UpLoad());
        }
    }
    #endregion

    #region 소지함 데이터 업로드
    private IEnumerator UpLoad()
    {
        linkState = LinkState.UpLoad;

        UpLoadAsync();

        yield return new WaitUntil(() => (linkState == LinkState.Idle));
    }

    private async void UpLoadAsync()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        var charInfo = GameManager.Instance.dataManager.characterData.characterData;

        await FirebaseManager.Instance.UpLoadInventory(user.UserId, user.Email, charInfo["server"].ToString(), charInfo["charId"].ToString());

        linkState = LinkState.Idle;
    }
    #endregion
}
