using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    ContextualMenuManager contextualMenu;
    AutoScaleBackgroundToCamera autoScaleBackground;

    float[] _bounds;

    public float[] Bounds {get => _bounds; } // minX, maxX, minY, maxY;

    public bool isMouseAboveDesktopItem {get; set;}
    MenuCaller menuCaller;

    List<DesktopItem> allItemsInDesktop;

    void Awake()
    {
        allItemsInDesktop = new List<DesktopItem>();
        menuCaller = new MenuCaller();
        contextualMenu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        isMouseAboveDesktopItem = false;
        _bounds = null;
        autoScaleBackground = GetComponent<AutoScaleBackgroundToCamera>();
        if (autoScaleBackground) {
            StartCoroutine(doActionsWhenAutoScaleBackgroundIsEnding());
        }
    }

    public void addItemToDeskop(DesktopItem desktopItem) => allItemsInDesktop.Add(desktopItem);

    IEnumerator doActionsWhenAutoScaleBackgroundIsEnding() {
        if (autoScaleBackground.IsAutoScaling) {
            yield return null;
        }

        // Calculate all the screen limits.
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height / Screen.height * Screen.width;
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

        _bounds = new float[]{
            x - (width / 2), // minX
            (width / 2) + x, // maxX
            y - (height / 2), // minY
            (height / 2) + y // maxY
        };
    }

    void Update()
    {
        if (!isMouseAboveDesktopItem && Input.GetMouseButtonUp(1))
            doInRightClick();
        else if (contextualMenu.isOpen && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2)))
            contextualMenu.close();
    }

    void doInRightClick()
    {
        if (contextualMenu != null)
        {
            menuCaller.setCaller(this);
            contextualMenu.enableInMousePosition(menuCaller, false);
        }
    }
}
