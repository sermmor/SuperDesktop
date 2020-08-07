using UnityEngine;
using UnityEngine.UI;

public class CreateNewFolderDialogCtrl : DialogController
{
    public GameObject toInstantiate;

    InputField nameLinkFile;

    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            nameLinkFile = transform.Find("InputNameFolder").gameObject.GetComponent<InputField>();
        }
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doAceptDialog()
    {
        GameObject generated = GameObject.Instantiate<GameObject>(toInstantiate);
        // Position
        positionToPlaceNewItem.x = contextualMenuManager.transform.position.x;
        positionToPlaceNewItem.y = contextualMenuManager.transform.position.y;
        positionToPlaceNewItem.z = generated.transform.position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Parent
        generated.transform.SetParent(whoIsCallMe.DesktopManagerCaller.transform);
        // Item Propierties
        FolderItem item = generated.GetComponent<FolderItem>();
        item.desktopManager = whoIsCallMe.DesktopManagerCaller;
        item.nameFile = nameLinkFile.text;
    }

    protected override void clearFieldsDialog()
    {
        nameLinkFile.text = "";
    }
}
