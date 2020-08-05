using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualMenuItem : MonoBehaviour
{
    Color colorSelectedItem;
    Color colorUnselectedItem;
    SpriteRenderer sRenderer;

    void OnEnable()
    {
        if (colorSelectedItem == Color.clear)
        {
            ContextualMenuManager menu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            colorSelectedItem = menu.colorSelectedItem;
            colorUnselectedItem = menu.colorUnselectedItem;
        }

        if (sRenderer == null) sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.color = colorUnselectedItem;
    }

    void OnMouseOver()
    {
        sRenderer.color = colorSelectedItem;
    }

    void OnMouseExit()
    {
        sRenderer.color = colorUnselectedItem;
    }
}
