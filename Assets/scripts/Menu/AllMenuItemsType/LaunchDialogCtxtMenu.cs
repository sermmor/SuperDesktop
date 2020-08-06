using UnityEngine;

public class LaunchDialogCtxtMenu: ContextualMenuItem
{
    public GameObject goDialogToEnable;

    DialogController dialogToEnable;

    protected override void doOnEnable()
    {
        dialogToEnable = goDialogToEnable.GetComponent<DialogController>();
    }

    protected override void doOnLeftClick()
    {
        if (dialogToEnable != null)
            dialogToEnable.showDialog(whoIsCallMe);
        else
            goDialogToEnable.SetActive(true);
    }
}
