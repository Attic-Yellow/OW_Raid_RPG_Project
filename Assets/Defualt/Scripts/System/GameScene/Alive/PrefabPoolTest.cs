using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PrefabPoolTest : MonoBehaviour,IPunPrefabPool
{
    public void Destroy(GameObject gameObject)
    {
      
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
     return Instantiate(prefabId, position, rotation);
    }



}
