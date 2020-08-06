
using UnityEngine;
using UnityEngine.UI;

public class RenameFileDialogCtrl: DialogController
{
    InputField nameFile;
    
    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            nameFile = transform.Find("InputNameFile").gameObject.GetComponent<InputField>();
        }
    }

    protected override void doAceptDialog()
    {
        whoIsCallMe.DesktopItemCaller.setFileName(nameFile.text);
    }

    protected override void clearFieldsDialog()
    {
        if (whoIsCallMe != null && whoIsCallMe.DesktopItemCaller)
            nameFile.text = whoIsCallMe.DesktopItemCaller.nameFile.Replace("\n", "\\n");
        else
            nameFile.text = "";
    }
}