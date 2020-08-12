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
}
