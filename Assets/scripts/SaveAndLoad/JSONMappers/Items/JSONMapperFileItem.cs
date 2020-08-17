using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperFileItem : JSONMapperDesktopItem
{
    public string filePath;
    public string directoryFilePath;
    public string iconPath;

    public JSONMapperFileItem(DesktopItem item) : base(item)
    {
        filePath = ((FileItem) item).filePath;
        directoryFilePath = ((FileItem) item).directoryFilePath;
        iconPath = ((FileItem) item).IconPath;
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.FileItemPrefab);
        // Position
        positionToPlaceNewItem.x = position.x;
        positionToPlaceNewItem.y = position.y;
        positionToPlaceNewItem.z = position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Item Propierties
        FileItem item = generated.GetComponent<FileItem>();
        item.desktopManager = desktopManager;
        item.nameFile = nameFile;
        item.filePath = filePath;
        item.IconPath = iconPath;
        item.transform.localScale = new Vector3(
            desktopManager.IconRealScale,
            desktopManager.IconRealScale,
            item.transform.localScale.z
        );
    }
}
