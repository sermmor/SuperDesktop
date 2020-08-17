using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperWidgetImageItem : JSONMapperDesktopItem
{
    public string imagePath;
    public Vector3 scale;

    public JSONMapperWidgetImageItem(DesktopItem item) : base(item)
    {
        imagePath = ((ImageBackgroundItemWidget) item).ImagePath;
        scale = item.transform.localScale;
    }

    Vector3 positionOrScaleToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.WidgetImagePrefab);
        // Position
        positionOrScaleToPlaceNewItem.x = position.x;
        positionOrScaleToPlaceNewItem.y = position.y;
        positionOrScaleToPlaceNewItem.z = position.z;
        generated.transform.position = positionOrScaleToPlaceNewItem;

        positionOrScaleToPlaceNewItem.x = scale.x;
        positionOrScaleToPlaceNewItem.y = scale.y;
        positionOrScaleToPlaceNewItem.z = scale.z;
        generated.transform.localScale = positionOrScaleToPlaceNewItem;
        // Item Propierties
        ImageBackgroundItemWidget item = generated.GetComponent<ImageBackgroundItemWidget>();
        item.desktopManager = desktopManager;
        item.ImagePath = imagePath;
        item.nameFile = nameFile;
    }
}
