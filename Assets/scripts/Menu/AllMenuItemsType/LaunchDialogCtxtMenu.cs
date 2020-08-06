using UnityEngine;

public class LaunchDialogCtxtMenu: ContextualMenuItem
{
    public DialogController dialogToEnable;

    protected override void doOnLeftClick()
    {
        dialogToEnable.showDialog(whoIsCallMe);
    }
}
