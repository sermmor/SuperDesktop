using UnityEngine;
using UnityEngine.UI;

public class CreateNewLinkDialogCtrl: DialogController
{
    public GameObject toInstantiate;

    InputField nameLinkFile;
    InputField url;

    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            nameLinkFile = transform.Find("InputNameFile").gameObject.GetComponent<InputField>();
            url = transform.Find("InputUrl").gameObject.GetComponent<InputField>();
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
        LinkItem item = generated.GetComponent<LinkItem>();
        item.desktopManager = whoIsCallMe.DesktopManagerCaller;
        item.nameFile = nameLinkFile.text;
        item.urlPath = url.text;
        
        base.doAceptDialog();
    }

    protected override void clearFieldsDialog()
    {
        nameLinkFile.text = "";
        url.text = "";
    }

    public void GetTitleTextByURL()
    {
        if (nameLinkFile.text == null || nameLinkFile.text.Length == 0)
            nameLinkFile.text = URLUtilities.getTitleUrl(url.text);
    }
}