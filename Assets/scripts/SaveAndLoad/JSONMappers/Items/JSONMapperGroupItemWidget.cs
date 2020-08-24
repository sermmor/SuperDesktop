using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperGroupItemWidget : JSONMapperDesktopItem
{
    public Vector3 scale;

    public JSONMapperGroupItemWidget(DesktopItem item) : base(item)
    {
        scale = item.transform.localScale;
    }

    Vector3 positionOrScaleToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.GroupItemWidgetPrefab);
        
        positionOrScaleToPlaceNewItem.x = position.x;
        positionOrScaleToPlaceNewItem.y = position.y;
        positionOrScaleToPlaceNewItem.z = position.z;
        generated.transform.position = positionOrScaleToPlaceNewItem;

        positionOrScaleToPlaceNewItem.x = scale.x;
        positionOrScaleToPlaceNewItem.y = scale.y;
        positionOrScaleToPlaceNewItem.z = scale.z;
        generated.transform.localScale = positionOrScaleToPlaceNewItem;

        generated.GetComponent<GroupItemWidget>().nameFile = nameFile;
        generated.GetComponent<GroupItemWidget>().desktopManager = desktopManager;
    }
}
