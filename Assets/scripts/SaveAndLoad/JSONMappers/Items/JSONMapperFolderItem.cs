using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONMapperFolderItem : JSONMapperDesktopItem
{
    public string[] itemList;

    public JSONMapperFolderItem(DesktopItem folder) : base(folder)
    {
        List<GameObject> originalItemList = ((FolderItem) folder).ItemList;
        itemList = new string[originalItemList.Count];

        DesktopItem item;
        for (int i = 0; i < originalItemList.Count; i++)
        {
            item = originalItemList[i].GetComponent<DesktopItem>();
            itemList[i] = item.nameFile;
        }
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    public void parseJsonToFolder(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.FolderItemPrefab);
        // Position
        positionToPlaceNewItem.x = position.x;
        positionToPlaceNewItem.y = position.y;
        positionToPlaceNewItem.z = position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Item Propierties
        FolderItem folder = generated.GetComponent<FolderItem>();
        folder.desktopManager = desktopManager;
        folder.nameFile = nameFile;

        // Put all items in folder.
        folder.ItemsToAddInStart = new List<string>();

        foreach (string nameSelected in itemList)
            folder.ItemsToAddInStart.Add(nameSelected);
    }
}
