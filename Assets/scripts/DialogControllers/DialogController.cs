using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogController : MonoBehaviour
{
    protected bool isLaunchedForFistTime = true;

    public GameObject[] navigationList;
    protected MenuCaller whoIsCallMe;
    bool isDoingAceptDialog = false;

    bool isNavigationEnabled;
    int indexInTabNavigation = 0;

    void OnEnable()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(true);
        isDoingAceptDialog = false;
        isNavigationEnabled = navigationList.Length > 0;
        indexInTabNavigation = 0;
        EventSystem.current.SetSelectedGameObject(navigationList[0]);
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
        DesktopRootReferenceManager.getInstance().isADialogOpened = true;
        this.gameObject.SetActive(true);
    }

    public void AceptDialog()
    {
        if (!isDoingAceptDialog) {
            isDoingAceptDialog = true;
            doAceptDialog();
            CloseDialog();
        }
    }

    protected virtual void doAceptDialog() => DesktopRootReferenceManager.getInstance().autoSaver.MarkToSave = true;

    public virtual void OnEscapeButton() => CloseDialog();

    public void CloseDialog()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
        DesktopRootReferenceManager.getInstance().isADialogOpened = false;
        this.gameObject.SetActive(false);
    }

    protected virtual void clearFieldsDialog()
    {
        Debug.Log("FIELDS CLEARED!");
    }

    void updateFocus(GameObject focusElement)
    {
        int indexFocus = -1;
        for (int i = 0; i < navigationList.Length; i++)
        {
            if (navigationList[i] == focusElement)
            {
                indexFocus = i;
                break;
            }
        }

        if (indexFocus != -1 && indexInTabNavigation != indexFocus)
        {
            indexInTabNavigation = indexFocus;
            EventSystem.current.SetSelectedGameObject(navigationList[indexInTabNavigation]);
        }
    }

    void Update()
    {
        // Check current focus.
        updateFocus(EventSystem.current.currentSelectedGameObject);

        // Check next focus.
        if (isNavigationEnabled && Input.GetKeyUp(KeyCode.Tab))
        {
            if (isReverseNavigation())
                navigationPreviousElement();
            else
                navigationNextElement();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnEscapeButton();
        }
    }

    bool isReverseNavigation()
    {
        return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKey(KeyCode.RightShift);
    }

    void navigationPreviousElement()
    {
        indexInTabNavigation--;
        if (indexInTabNavigation < 0) indexInTabNavigation = navigationList.Length - 1;
        EventSystem.current.SetSelectedGameObject(navigationList[indexInTabNavigation]);
    }

    void navigationNextElement()
    {
        indexInTabNavigation++;
        if (indexInTabNavigation >= navigationList.Length) indexInTabNavigation = 0;
        EventSystem.current.SetSelectedGameObject(navigationList[indexInTabNavigation]);
    }
}
