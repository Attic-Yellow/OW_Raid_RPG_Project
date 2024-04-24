using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconData : MonoBehaviour
{
    public static IconData Instance;

    public List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetitemIcon(string itemIcon)
    {
        return sprites.Find(x => x.name == itemIcon);
    }
}
