using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperVideoItem : JSONMapperDesktopItem
{
    public string pathVideo;
    public Vector3 scale;

    public bool isSticked;

    public JSONMapperVideoItem(DesktopItem item) : base(item)
    {
        pathVideo = ((VideoItem) item).pathVideo;
        isSticked = ((VideoItem) item).isSticked;
        scale = item.transform.localScale;
    }

    Vector3 positionOrScaleToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.VideoItemPrefab);
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
        VideoItem item = generated.GetComponent<VideoItem>();
        item.desktopManager = desktopManager;
        item.nameFile = nameFile;
        item.pathVideo = pathVideo;
        item.isSticked = isSticked;
    }
}
