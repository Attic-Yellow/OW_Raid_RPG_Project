using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ExtendedFlycam : MonoBehaviour
{

    /*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {

    }

    //void Update()
    //{
    //    rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
    //    rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
    //    rotationY = Mathf.Clamp(rotationY, -90, 90);

    //    transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
    //    transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

    //    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    //    {
    //        transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
    //        transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
    //    }
    //    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
    //    {
    //        transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
    //        transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
    //    }
    //    else
    //    {
    //        transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
    //        transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
    //    }


    //    if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
    //    if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

    //}

    private void OnCharacter(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.characterInfoUI.CharacterInfoUIController();
    }

    private void OnInventory(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.inventoryUI.InventoryController();
    }

    private void OnEquipped(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.characterGearUI.GearUIController();
    }

    private void OnSkill(InputValue value)
    {
        GameManager.Instance.uiManager.gameSceneUI.skillUI.SkillUIController();
    }

    public void OnQuickSlot11(InputValue value)
    {
        print("큇슬롯 1-1");
        if (GameManager.Instance.uiManager.gameSceneUI.quickSlotData.slotDataList.ContainsKey("Quick Slot 1 - 1"))
        {
            print("여기도 들어와?");
            SlotDataList slotData = GameManager.Instance.uiManager.gameSceneUI.quickSlotData.slotDataList["Quick Slot 1 - 1"];
            GameObject.Find(slotData.slotName).GetComponent<Slot>().Use();
        }
    }

    public void OnQuickSlot12(InputValue value)
    {

    }

    public void OnQuickSlot13(InputValue value)
    {

    }

    public void OnQuickSlot14(InputValue value)
    {

    }

    public void OnQuickSlot15(InputValue value)
    {

    }

    public void OnQuickSlot16(InputValue value)
    {

    }

    public void OnQuickSlot17(InputValue value)
    {

    }

    public void OnQuickSlot18(InputValue value)
    {

    }

    public void OnQuickSlot19(InputValue value)
    {

    }

    public void OnQuickSlot110(InputValue value)
    {

    }

    public void OnQuickSlot111(InputValue value)
    {

    }

    public void OnQuickSlot112(InputValue value)
    {

    }


    public void OnQuickSlot21(InputValue value)
    {

    }

    public void OnQuickSlot22(InputValue value)
    {

    }

    public void OnQuickSlot23(InputValue value)
    {

    }

    public void OnQuickSlot24(InputValue value)
    {

    }

    public void OnQuickSlot25(InputValue value)
    {

    }

    public void OnQuickSlot26(InputValue value)
    {

    }

    public void OnQuickSlot27(InputValue value)
    {

    }

    public void OnQuickSlot28(InputValue value)
    {

    }

    public void OnQuickSlot29(InputValue value)
    {

    }

    public void OnQuickSlot210(InputValue value)
    {

    }

    public void OnQuickSlot211(InputValue value)
    {

    }

    public void OnQuickSlot212(InputValue value)
    {

    }


    public void OnQuickSlot31(InputValue value)
    {

    }

    public void OnQuickSlot32(InputValue value)
    {

    }

    public void OnQuickSlot33(InputValue value)
    {

    }

    public void OnQuickSlot34(InputValue value)
    {

    }

    public void OnQuickSlot35(InputValue value)
    {

    }

    public void OnQuickSlot36(InputValue value)
    {

    }

    public void OnQuickSlot37(InputValue value)
    {

    }

    public void OnQuickSlot38(InputValue value)
    {

    }

    public void OnQuickSlot39(InputValue value)
    {

    }

    public void OnQuickSlot310(InputValue value)
    {

    }

    public void OnQuickSlot311(InputValue value)
    {

    }

    public void OnQuickSlot312(InputValue value)
    {

    }


    public void OnQuickSlot41(InputValue value)
    {

    }

    public void OnQuickSlot42(InputValue value)
    {

    }

    public void OnQuickSlot43(InputValue value)
    {

    }

    public void OnQuickSlot44(InputValue value)
    {

    }

    public void OnQuickSlot45(InputValue value)
    {

    }

    public void OnQuickSlot46(InputValue value)
    {

    }

    public void OnQuickSlot47(InputValue value)
    {

    }

    public void OnQuickSlot48(InputValue value)
    {

    }

    public void OnQuickSlot49(InputValue value)
    {

    }

    public void OnQuickSlot410(InputValue value)
    {

    }

    public void OnQuickSlot411(InputValue value)
    {

    }

    public void OnQuickSlot412(InputValue value)
    {

    }


    public void OnQuickSlot51(InputValue value)
    {

    }

    public void OnQuickSlot52(InputValue value)
    {

    }

    public void OnQuickSlot53(InputValue value)
    {

    }

    public void OnQuickSlot54(InputValue value)
    {

    }

    public void OnQuickSlot55(InputValue value)
    {

    }

    public void OnQuickSlot56(InputValue value)
    {

    }

    public void OnQuickSlot57(InputValue value)
    {

    }

    public void OnQuickSlot58(InputValue value)
    {

    }

    public void OnQuickSlot59(InputValue value)
    {

    }

    public void OnQuickSlot510(InputValue value)
    {

    }

    public void OnQuickSlot511(InputValue value)
    {

    }

    public void OnQuickSlot512(InputValue value)
    {

    }

    public void OnQuickSlot61(InputValue value)
    {

    }

    public void OnQuickSlot62(InputValue value)
    {

    }

    public void OnQuickSlot63(InputValue value)
    {

    }

    public void OnQuickSlot64(InputValue value)
    {

    }

    public void OnQuickSlot65(InputValue value)
    {

    }

    public void OnQuickSlot66(InputValue value)
    {

    }

    public void OnQuickSlot67(InputValue value)
    {

    }

    public void OnQuickSlot68(InputValue value)
    {

    }

    public void OnQuickSlot69(InputValue value)
    {

    }

    public void OnQuickSlot610(InputValue value)
    {

    }

    public void OnQuickSlot611(InputValue value)
    {

    }

    public void OnQuickSlot612(InputValue value)
    {

    }

    public void OnQuickSlot71(InputValue value)
    {

    }

    public void OnQuickSlot72(InputValue value)
    {

    }
    public void OnQuickSlot73(InputValue value)
    {

    }

    public void OnQuickSlot74(InputValue value)
    {

    }

    public void OnQuickSlot75(InputValue value)
    {

    }

    public void OnQuickSlot76(InputValue value)
    {

    }

    public void OnQuickSlot77(InputValue value)
    {

    }

    public void OnQuickSlot78(InputValue value)
    {

    }

    public void OnQuickSlot79(InputValue value)
    {

    }

    public void OnQuickSlot710(InputValue value)
    {

    }

    public void OnQuickSlot711(InputValue value)
    {

    }

    public void OnQuickSlot712(InputValue value)
    {

    }

    public void OnQuickSlot81(InputValue value)
    {

    }

    public void OnQuickSlot82(InputValue value)
    {

    }

    public void OnQuickSlot83(InputValue value)
    {

    }

    public void OnQuickSlot84(InputValue value)
    {

    }

    public void OnQuickSlot85(InputValue value)
    {

    }

    public void OnQuickSlot86(InputValue value)
    {

    }

    public void OnQuickSlot87(InputValue value)
    {

    }

    public void OnQuickSlot88(InputValue value)
    {

    }

    public void OnQuickSlot89(InputValue value)
    {

    }

    public void OnQuickSlot810(InputValue value)
    {

    }

    public void OnQuickSlot811(InputValue value)
    {

    }

    public void OnQuickSlot812(InputValue value)
    {

    }

    public void OnQuickSlot91(InputValue value)
    {

    }

    public void OnQuickSlot92(InputValue value)
    {

    }

    public void OnQuickSlot93(InputValue value)
    {

    }

    public void OnQuickSlot94(InputValue value)
    {

    }

    public void OnQuickSlot95(InputValue value)
    {

    }

    public void OnQuickSlot96(InputValue value)
    {

    }

    public void OnQuickSlot97(InputValue value)
    {

    }

    public void OnQuickSlot98(InputValue value)
    {

    }

    public void OnQuickSlot99(InputValue value)
    {

    }

    public void OnQuickSlot910(InputValue value)
    {

    }

    public void OnQuickSlot911(InputValue value)
    {

    }

    public void OnQuickSlot912(InputValue value)
    {

    }

    public void OnQuickSlot101(InputValue value)
    {

    }

    public void OnQuickSlot102(InputValue value)
    {

    }

    public void OnQuickSlot103(InputValue value)
    {

    }

    public void OnQuickSlot104(InputValue value)
    {

    }

    public void OnQuickSlot105(InputValue value)
    {

    }

    public void OnQuickSlot106(InputValue value)
    {

    }

    public void OnQuickSlot107(InputValue value)
    {

    }

    public void OnQuickSlot108(InputValue value)
    {

    }


    public void OnQuickSlot109(InputValue value)
    {

    }

    public void OnQuickSlot1010(InputValue value)
    {

    }

    public void OnQuickSlot1011(InputValue value)
    {

    }

    public void OnQuickSlot1012(InputValue value)
    {

    }



}