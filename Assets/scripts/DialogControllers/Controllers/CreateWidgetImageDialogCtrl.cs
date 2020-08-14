using UnityEngine;
using UnityEngine.UI;

public class CreateWidgetImageDialogCtrl: DialogController
{
    public GameObject toInstantiate;

    InputField pathVideo;

    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            pathVideo = transform.Find("InputPath").gameObject.GetComponent<InputField>();
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
        // Item Propierties
        ImageBackgroundItemWidget item = generated.GetComponent<ImageBackgroundItemWidget>();
        item.desktopManager = whoIsCallMe.DesktopManagerCaller;
        item.ImagePath = pathVideo.text;
        item.nameFile = pathVideo.text;
        
        base.doAceptDialog();
    }

    protected override void clearFieldsDialog()
    {
        pathVideo.text = "";
    }     
}