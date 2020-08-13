
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
        if (
            !(whoIsCallMe.DesktopItemCaller is FolderItem)
            && whoIsCallMe.DesktopItemCaller.NameFolderParent != null
            && !"".Equals(whoIsCallMe.DesktopItemCaller.NameFolderParent)
        ) {
            DesktopManager desktop = DesktopRootReferenceManager.getInstance().CurrentDesktopShowed;
            FolderItem folder = desktop.getFolderByName(whoIsCallMe.DesktopItemCaller.NameFolderParent);
            folder.removeFromFolder(whoIsCallMe.DesktopItemCaller.gameObject);
        }

        GameObject.Destroy(whoIsCallMe.DesktopItemCaller.gameObject);
        
        base.doAceptDialog();
    }

    protected override void clearFieldsDialog() { }
}