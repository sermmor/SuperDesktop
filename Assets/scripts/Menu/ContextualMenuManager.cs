using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualMenuManager : MonoBehaviour
{
    public Color colorSelectedItem;
    public Color colorUnselectedItem;
    
    GameObject fileOptions;
    Vector3 mousePosition;
    Vector3 positionToShowMenu;
    public bool isOpen {get => this.gameObject.activeSelf; }

    void OnEnable()
    {
        positionToShowMenu = new Vector3(0, 0, transform.position.z);
        if (fileOptions == null)
            fileOptions = transform.Find("AllMenus/FileOptions").gameObject;
    }

    public void enableInMousePosition(bool enableFileMenu)
    {
        gameObject.SetActive(true);
        fileOptions.SetActive(enableFileMenu);

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionToShowMenu.x = mousePosition.x;
        positionToShowMenu.y = mousePosition.y;
        transform.position = positionToShowMenu;
    }

    public void close() => gameObject.SetActive(false);
}
