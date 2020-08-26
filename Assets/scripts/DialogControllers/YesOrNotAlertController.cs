using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class YesOrNotAlertController : MonoBehaviour
{
    Text InfoTextLabel;
    Action onYes;
    Action onNo;

    bool wasColliderActiveBeforeShowAlert;

    void settingAllReferences()
    {
        if (InfoTextLabel == null)
            InfoTextLabel = transform.Find("InfoText").GetComponent<Text>();
    }

    public void showYesOrNoAlert(string textAlert, Action onYes, Action onNo)
    {
        settingAllReferences();
        InfoTextLabel.text = textAlert;
        this.onYes = onYes;
        this.onNo = onNo;
        wasColliderActiveBeforeShowAlert = DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.activeSelf;
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(true);
        gameObject.SetActive(true);
    }

    public void YesActionButton()
    {
        onYes();
        if (!wasColliderActiveBeforeShowAlert)
        {
            DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void NoActionButton()
    {
        onNo();
        if (!wasColliderActiveBeforeShowAlert)
        {
            DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
        }
        gameObject.SetActive(false);
    }


}
