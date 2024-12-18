using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerClickHandler/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public TabGroup tabGroup;

    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    tabGroup.OnTabEnter(this);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    tabGroup.OnTabExit(this);
    //}

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }


}
