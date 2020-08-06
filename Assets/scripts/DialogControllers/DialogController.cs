using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogController : MonoBehaviour
{
    protected bool isLaunchedForFistTime = true;

    public GameObject[] navigationList;
    protected MenuCaller whoIsCallMe;

    bool isNavigationEnabled;
    int indexInTabNavigation = 0;

    void OnEnable()
    {
        isNavigationEnabled = navigationList.Length > 0;
        indexInTabNavigation = 0;
        doOnEnable();
        clearFieldsDialog();
        if (isLaunchedForFistTime) isLaunchedForFistTime = false;
    }

    protected virtual void doOnEnable()
    {
        Debug.Log("OnEnable DO IT!");
    }

    public void showDialog(MenuCaller whoIsCallMe) {
        this.whoIsCallMe = whoIsCallMe;
        this.gameObject.SetActive(true);
    }

    public void AceptDialog()
    {
        doAceptDialog();
        CloseDialog();
    }

    protected virtual void doAceptDialog()
    {
        Debug.Log("DIALOG ACEPTED!");
    }

    public void CloseDialog() {
        this.gameObject.SetActive(false);
    }

    protected virtual void clearFieldsDialog()
    {
        Debug.Log("FIELDS CLEARED!");
    }

    void Update()
    {
        if (isNavigationEnabled && Input.GetKeyUp(KeyCode.Tab))
        {
            if (
                Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKey(KeyCode.RightShift)
            )
            {
                indexInTabNavigation--;
                if (indexInTabNavigation < 0) indexInTabNavigation = navigationList.Length - 1;
            }
            else
            {
                indexInTabNavigation++;
                if (indexInTabNavigation >= navigationList.Length) indexInTabNavigation = 0;
            }
            EventSystem.current.SetSelectedGameObject(navigationList[indexInTabNavigation]);
        }
    }
}
