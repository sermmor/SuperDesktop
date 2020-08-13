using UnityEngine;
using UnityEngine.UI;

public class ChangeIconFileDialogCtrl: DialogController
{
    InputField nameIconPath;
    
    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            nameIconPath = transform.Find("InputPathIcon").gameObject.GetComponent<InputField>();
        }
    }

    protected override void doAceptDialog()
    {
        if (whoIsCallMe.DesktopItemCaller is FileItem)
            ((FileItem) whoIsCallMe.DesktopItemCaller).IconPath = nameIconPath.text;
        else if (whoIsCallMe.DesktopItemCaller is LinkItem)
            ((LinkItem) whoIsCallMe.DesktopItemCaller).IconPath = nameIconPath.text;
        
        base.doAceptDialog();
    }

    protected override void clearFieldsDialog()
    {
        if (whoIsCallMe.DesktopItemCaller is FileItem)
            nameIconPath.text = ((FileItem) whoIsCallMe.DesktopItemCaller).IconPath;
        else if (whoIsCallMe.DesktopItemCaller is LinkItem)
            nameIconPath.text = ((LinkItem) whoIsCallMe.DesktopItemCaller).IconPath;
        else
            nameIconPath.text = "";
    }
}
