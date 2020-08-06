using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    protected MenuCaller whoIsCallMe;

    public void showDialog(MenuCaller whoIsCallMe) {
        this.whoIsCallMe = whoIsCallMe;
        this.gameObject.SetActive(true);
    }

    public void AceptDialog()
    {
        doAceptDialog();
        clearFieldsDialog();
        CloseDialog();
    }

    protected virtual void doAceptDialog()
    {
        Debug.Log("DIALOG ACEPTED!");
    }

    public void CloseDialog() {
        clearFieldsDialog();
        this.gameObject.SetActive(false);
    }

    protected virtual void clearFieldsDialog()
    {
        Debug.Log("FIELDS CLEARED!");
    }
}
