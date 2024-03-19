using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System;

public class DataManager : MonoBehaviour
{
   public static DataManager instance;
   private Dictionary<string, PlayerSaveData> playerDataDic = new Dictionary<string, PlayerSaveData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        DestroyImmediate(gameObject);
    }

    private void Start()
    {

 


        /* PlayerSaveData playerSaveData = new PlayerSaveData(); 
         string jsonData = JsonUtility.ToJson(playerSaveData);//딕셔너리 불가 , 직접생성한 클래스는   [Serializable] 

         PlayerSaveData playerSaveData1 = JsonUtility.FromJson<PlayerSaveData>(jsonData);*/



        /*GameObject obj = new GameObject(); //모노비헤이어를 상속받는 클래스
        obj.AddComponent<Player>();
        string jsonData = JsonUtility.ToJson(obj.GetComponent<Player>());

        GameObject obj2 = new GameObject();
        JsonUtility.FromJsonOverwrite(jsonData, obj2.AddComponent<Player>());*/

    }
    [Serializable]
    public class PlayerSaveData
    {
        public float power;
        public float luck;

    }


    public void SavePlayerData(string charName, Player player)
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.power = player.Power;
        saveData.luck = player.Luck;

        playerDataDic[charName] = saveData;

        FileStream stream = new FileStream(Application.dataPath + "/playerData.json", FileMode.OpenOrCreate);
        string jsonData = JsonConvert.SerializeObject(playerDataDic);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        stream.Write(data, 0, data.Length);
        stream.Close();
    }

    public void LoadPlayerData()
    {
        FileStream stream = new FileStream(Application.dataPath + "/playerData.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        playerDataDic = JsonConvert.DeserializeObject<Dictionary<string, PlayerSaveData>>(jsonData);

        ApplyPlayerData();
    }

    private void ApplyPlayerData()
    {
        // 저장된 데이터를 플레이어에게 적용하는 코드 작성
        foreach (KeyValuePair<string, PlayerSaveData> entry in playerDataDic)
        {
            string charName = entry.Key;
            PlayerSaveData saveData = entry.Value;
            //TODO : 해당 캐릭터를 찾아서 넣어주기
      
        }
    }
}
