using UnityEngine;

public class CreateGroupItemWidgetCtxtMenu : ContextualMenuItem
{
    public GameObject toInstantiate;
    
    Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doOnLeftClick()
    {
        GameObject generated = GameObject.Instantiate<GameObject>(toInstantiate);
        positionToPlaceNewItem.x = this.transform.position.x;
        positionToPlaceNewItem.y = this.transform.position.y;
        positionToPlaceNewItem.z = generated.transform.position.z;
        generated.transform.position = positionToPlaceNewItem;
        generated.GetComponent<GroupItemWidget>().desktopManager = whoIsCallMe.DesktopManagerCaller;

        DesktopRootReferenceManager.getInstance().autoSaver.MarkToSave = true;
        
    }
}
