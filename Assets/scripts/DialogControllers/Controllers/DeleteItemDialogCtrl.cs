
using UnityEngine;
using UnityEngine.UI;

public class DeleteItemDialogCtrl: DialogController
{
    InputField nameFile;
    
    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        }
    }

    protected override void doAceptDialog()
    {
        GameObject.Destroy(whoIsCallMe.DesktopItemCaller.gameObject);
        
        base.doAceptDialog();
    }

    protected override void clearFieldsDialog() { }
}