using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] WeaponType wType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !other.CompareTag("Player"))
        {
            print("µ•πÃ¡ˆ¡‹");
            Player player = GameManager.Instance.currentPlayerObj.GetComponent<Player>();
            other.GetComponent<Monster>().TakeDamage(GameManager.Instance.currentPlayerObj, player.Power, player.PPhy, 0, 0);

        }
    }
  
  
}
