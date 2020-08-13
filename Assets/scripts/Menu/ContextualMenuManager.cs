using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContextualMenuMode { DESKTOP, FILE, FOLDER, LINK, VIDEO_WIDGET, GROUP_ITEM_WIDGET };

public class ContextualMenuManager : MonoBehaviour
{
    public Color colorSelectedItem;
    public Color colorUnselectedItem;
    
    GameObject fileOptions;
    GameObject linkOptions;
    GameObject videoOptions;
    GameObject desktopOptions;
    GameObject folderOptions;
    GameObject groupItemsOptions;
    Vector3 mousePosition;
    Vector3 positionToShowMenu;
    public bool isOpen {get => this.gameObject.activeSelf; }

    MenuCaller _whoIsCallMe;
    public MenuCaller whoIsCallMe { get => this._whoIsCallMe; }

    void OnEnable()
    {
        positionToShowMenu = new Vector3(0, 0, transform.position.z);
        if (fileOptions == null)
            fileOptions = transform.Find("AllMenus/FileOptions").gameObject;
        if (desktopOptions == null)
            desktopOptions = transform.Find("AllMenus/DesktopOptions").gameObject;
        if (folderOptions == null)
            folderOptions = transform.Find("AllMenus/FolderOptions").gameObject;
        if (linkOptions == null)
            linkOptions = transform.Find("AllMenus/LinkOptions").gameObject;
        if (videoOptions == null)
            videoOptions = transform.Find("AllMenus/VideoOptions").gameObject;
        if (groupItemsOptions == null)
            groupItemsOptions = transform.Find("AllMenus/GroupItemsOptions").gameObject;
    }

    public void enableInMousePosition(MenuCaller whoIsCallMe, ContextualMenuMode mode)
    {
        this._whoIsCallMe = whoIsCallMe;
        gameObject.SetActive(true);
        desktopOptions.SetActive(mode == ContextualMenuMode.DESKTOP);
        fileOptions.SetActive(mode == ContextualMenuMode.FILE);
        linkOptions.SetActive(mode == ContextualMenuMode.LINK);
        folderOptions.SetActive(mode == ContextualMenuMode.FOLDER);
        videoOptions.SetActive(mode == ContextualMenuMode.VIDEO_WIDGET);
        groupItemsOptions.SetActive(mode == ContextualMenuMode.GROUP_ITEM_WIDGET);

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionToShowMenu.x = mousePosition.x;
        positionToShowMenu.y = mousePosition.y;
        transform.position = positionToShowMenu;
    }

    public void close() => gameObject.SetActive(false);
}
