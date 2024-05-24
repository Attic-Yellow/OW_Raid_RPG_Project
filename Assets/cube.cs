using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print($"trgger {other.gameObject}");
    }

  
}
