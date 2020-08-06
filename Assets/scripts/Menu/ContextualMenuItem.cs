using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualMenuItem : MonoBehaviour
{
    Color colorSelectedItem;
    Color colorUnselectedItem;
    SpriteRenderer sRenderer;
    ContextualMenuManager menu;
    protected MenuCaller whoIsCallMe;

    void OnEnable()
    {
        whoIsCallMe = DesktopRootReferenceManager.getInstance().contextualMenuManager.whoIsCallMe;
        if (colorSelectedItem == Color.clear)
        {
            menu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            colorSelectedItem = menu.colorSelectedItem;
            colorUnselectedItem = menu.colorUnselectedItem;
        }

        if (sRenderer == null) sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.color = colorUnselectedItem;
    }

    void OnMouseUp()
    {
        if (menu.isOpen) menu.close();
        doOnLeftClick();
    }

    void OnMouseOver()
    {
        sRenderer.color = colorSelectedItem;
    }

    void OnMouseExit()
    {
        sRenderer.color = colorUnselectedItem;
    }

    protected virtual void doOnLeftClick()
    {
        Debug.Log("CLICKED IN MENU!!!");
    }
}
