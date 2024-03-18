using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class InitChar : MonoBehaviour
{
    [SerializeField] private Camera renderTextureCam;
    [SerializeField] private Transform target;
    [SerializeField] GameObject nickNameOverlay;
    private string nickname;
    public enum Job
    {
        ��Ŀ,
        ��������,
        ���Ÿ�����,
        ����

    }
   [SerializeField] private Job state;
    public Job State {  get { return state; } set { state = value; } }
    int jobsCount = Enum.GetNames(typeof(Job)).Length;

    private Dictionary<Enum, GameObject[]> charatorDic = new();

    [SerializeField] private GameObject[] tankerPrefabs;
    [SerializeField] private GameObject[] shortDealerPrefabs;
    [SerializeField] private GameObject[] longDealerPrefabs;
    [SerializeField] private GameObject[] healerPrefabs;

    [SerializeField] private TextMeshProUGUI genderText;
    [SerializeField] private TextMeshProUGUI jobText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TMP_InputField nicknameText;

    public string JobText { get { return jobText.text; } set { jobText.text = value; } }
    public string GenderText { get { return genderText.text; } set { genderText.text = value; } }
    public string NickNameText { get {return nicknameText.text; } }
    public string ErrorText { get { return errorText.text; } set {  errorText.text = value; } }

    private void Awake()
    {
        AddDic();
        State = Job.��Ŀ;
        JobText = State.ToString();
        GenderText = "man";
        ChangeTarget();
        DeleteBtn();
    }

    private void AddDic()
    {
        charatorDic.Add(Job.��Ŀ, tankerPrefabs);
        charatorDic.Add(Job.��������, shortDealerPrefabs);
        charatorDic.Add(Job.���Ÿ�����, longDealerPrefabs);
        charatorDic.Add(Job.����, healerPrefabs);
    }
   
    private void ChangeTarget()
    {

        // ���� ���¿� �ش��ϴ� ������ �迭�� ������
        GameObject[] selectedPrefabs = charatorDic[State];

        // ���õ� �������� �ε���
        int prefabIndex = GenderText == "man" ? 0 : 1;

        Transform camPosTransform = selectedPrefabs[prefabIndex].transform.Find("CamPos");
        if (camPosTransform != null)
        {
            target = camPosTransform;
            renderTextureCam.transform.parent = target;
            renderTextureCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);  
        }
        else
        {
            Debug.LogError("CamPos not found in the instantiated character prefab.");
        }
    }

    public void OnJobChange()
    {
        int nextState = (int)State + 1;
        if (nextState >= jobsCount)
        {
            nextState = 0;
        }

        State = (Job)nextState; // ���� ����
        JobText = State.ToString();
        ChangeTarget();
    }
    public void OnGenderChange() //���� ��ü ��ư�� ������ ��
    {
        GenderText = GenderText == "man" ? "woman" : "man";
        ChangeTarget();
    }
    public void NicknameOverlay()
    {
        if(nickNameOverlay != null && !nickNameOverlay.activeSelf)
        nickNameOverlay.SetActive(true);
    }
    public void DeleteBtn()
    {
        nickNameOverlay.SetActive(false);
    }
    
    public void InitNickName()
    {
       if(NickNameText.Length > 9 || NickNameText.Length < 2 )
        {
            ErrorText = "2���� �̻� 9���� �̳��� �Է��ϼ���";
            errorText.color = Color.red;
            nickname = null;
            return;
        }
        /*�Ȱ��� �г����� ���� �� ����*/
       
        nickname = NickNameText;
        errorText.color= Color.black;
        ErrorText = "��� ������ �г����Դϴ�";
        DeleteBtn();
    }

    public void InitCharBtn()
    {
      if(nickname != null)
        {
            //�����ϰ� �����ϴ� ����
            Debug.Log($"{nickname}���� �Ϸ�");
            return;
        }

        Debug.Log("ĳ���� �̸��� �������ּ���");
    }
}
