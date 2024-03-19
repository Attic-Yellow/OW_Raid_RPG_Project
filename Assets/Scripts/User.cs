using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
public class User : MonoBehaviour
{
    public enum Server
    {
        Server0,
        Server1,
        Server2,
        Server3
    }
    public Player currentChar; //현재 선택한 캐릭터 (게임매니저에서 관리?)
    private Dictionary<Server, List<GameObject>> charPrefabsDic = new Dictionary<Server,List<GameObject>>();

    public void AddCharacterPrefab(Server server, GameObject characterPrefab)
    {
        if (charPrefabsDic.ContainsKey(server))
        {
            List<GameObject> prefabList = charPrefabsDic[server];
            if (prefabList.Count < 4) // 최대 4개까지만 추가
            {
                prefabList.Add(characterPrefab);
            }
            else
            {
                Debug.LogWarning("생성 가능한 캐릭터 수가 초과");
            }
        }
        else
        {
            Debug.LogWarning("서버를 찾을 수 없음");
        }
    }
}
