using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickBar : MonoBehaviour
{
    [SerializeField] private GameObject quickBar;
    [SerializeField] private GameObject dragArea;
    [SerializeField] private Image[] sizeButton;
    
    public void ChangeQuickBarSize(int index)
    {
        var rect = quickBar.GetComponent<RectTransform>();
        var rectDrag = dragArea.GetComponent<RectTransform>();

        switch (index)
        {
            case 0:
                rect.sizeDelta = new Vector2(850, 80);
                rectDrag.sizeDelta = new Vector2(850, 80);
                break;
            case 1:
                rect.sizeDelta = new Vector2(430, 150);
                rectDrag.sizeDelta = new Vector2(430, 150);
                break;
            case 2:
                rect.sizeDelta = new Vector2(290, 220);
                rectDrag.sizeDelta = new Vector2(290, 220);
                break;
            case 3:
                rect.sizeDelta = new Vector2(220, 290);
                rectDrag.sizeDelta = new Vector2(220, 290);
                break;
            case 4:
                rect.sizeDelta = new Vector2(150, 430);
                rectDrag.sizeDelta = new Vector2(150, 430);
                break;
            case 5:
                rect.sizeDelta = new Vector2(80, 850);
                rectDrag.sizeDelta = new Vector2(80, 850);
                break;
        }

        ChangeButtonColor(index);
    }

    private void ChangeButtonColor(int index)
    {
        if (sizeButton.Length > 0)
        {
            for (int i = 0; i < sizeButton.Length; i++)
            {
                sizeButton[i].color = index == i ? Color.yellow : Color.white;
            }
        }
    }
}
