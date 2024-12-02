using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{

    public List<TabButtonUI> tabButtons;
    public TabButtonUI selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButtonUI button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtonUI>();
        }

        tabButtons.Add(button);
    }

    // Khi nút được nhấn
    public void OnTabSelected(TabButtonUI button)
    {
        selectedTab = button;
        //ResetTabs();
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
}
