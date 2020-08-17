using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperLinkItem : JSONMapperDesktopItem
{
    public string urlPath;
    public string iconPath;

    public JSONMapperLinkItem(DesktopItem item) : base(item)
    {
        urlPath = ((LinkItem) item).urlPath;
        iconPath = ((LinkItem) item).IconPath;
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.LinkItemPrefab);
        // Position
        positionToPlaceNewItem.x = position.x;
        positionToPlaceNewItem.y = position.y;
        positionToPlaceNewItem.z = position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Item Propierties
        LinkItem item = generated.GetComponent<LinkItem>();
        item.desktopManager = desktopManager;
        item.nameFile = nameFile;
        item.urlPath = urlPath;
        item.IconPath = iconPath;
    }
}
