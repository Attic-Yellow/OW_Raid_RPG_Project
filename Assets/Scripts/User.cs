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
    public Player currentChar; //���� ������ ĳ���� (���ӸŴ������� ����?)
    private Dictionary<Server, List<GameObject>> charPrefabsDic = new Dictionary<Server,List<GameObject>>();

    public void AddCharacterPrefab(Server server, GameObject characterPrefab)
    {
        if (charPrefabsDic.ContainsKey(server))
        {
            List<GameObject> prefabList = charPrefabsDic[server];
            if (prefabList.Count < 4) // �ִ� 4�������� �߰�
            {
                prefabList.Add(characterPrefab);
            }
            else
            {
                Debug.LogWarning("���� ������ ĳ���� ���� �ʰ�");
            }
        }
        else
        {
            Debug.LogWarning("������ ã�� �� ����");
        }
    }
}
