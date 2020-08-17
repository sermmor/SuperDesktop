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
        gameObject.SetActive(true);
    }

    public void YesActionButton()
    {
        onYes();
        gameObject.SetActive(false);
    }

    public void NoActionButton()
    {
        onNo();
        gameObject.SetActive(false);
    }


}
