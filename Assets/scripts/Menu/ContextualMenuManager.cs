using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualMenuManager : MonoBehaviour
{
    public Color colorSelectedItem;
    public Color colorUnselectedItem;
    
    GameObject fileOptions;
    GameObject desktopOptions;
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
    }

    public void enableInMousePosition(MenuCaller whoIsCallMe, bool enableFileMenu)
    {
        this._whoIsCallMe = whoIsCallMe;
        gameObject.SetActive(true);
        desktopOptions.SetActive(!enableFileMenu);
        fileOptions.SetActive(enableFileMenu);

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionToShowMenu.x = mousePosition.x;
        positionToShowMenu.y = mousePosition.y;
        transform.position = positionToShowMenu;
    }

    public void close() => gameObject.SetActive(false);
}
