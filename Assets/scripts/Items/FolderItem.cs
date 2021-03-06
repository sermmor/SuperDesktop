using System.Collections.Generic;
using UnityEngine;

public class FolderItem : DesktopItem
{
    public const float POSITION_X_TO_HIDE_ITEM = -10000;
    const int MAX_NUMBER_ITEM = 105;

    GameObject folderDialog;

    List<GameObject> _itemList = new List<GameObject>();

    public List<GameObject> ItemList { get => _itemList; }

    public List<string> ItemsToAddInStart { get; set; }

    protected override void thingsToDoAfterStart()
    {
        if (ItemsToAddInStart == null || ItemsToAddInStart.Count == 0) return;

        foreach (string nameSelected in ItemsToAddInStart)
        {
            DesktopItem itemDesktop = desktopManager.getItemByName(nameSelected);
            AddToFolder(itemDesktop.gameObject);
            FolderItem.HideItemFromDesktop(itemDesktop.gameObject);
        }
    }

    public bool AddToFolder(GameObject item)
    {
        bool folderFull = false;
        DesktopItem desktopItem = item.GetComponent<DesktopItem>();
        DesktopRootReferenceManager.getInstance().CurrentDesktopShowed.RemoveFromFolderAndPutInDesktop(desktopItem);
        if (item.GetComponent<FolderItem>() == null)
        {
            if (_itemList.Count < MAX_NUMBER_ITEM)
            {
                _itemList.Add(item);
                desktopItem.setFolderParentName(nameFile);
            }
            else 
                folderFull = true;
        }
        return folderFull;
    }

    public void removeFromFolder(GameObject item) => _itemList.Remove(item);

    public void HideAllItemFromDesktop()
    {
        foreach(GameObject item in _itemList)
        {
            FolderItem.HideItemFromDesktop(item);
        }
    }

    public static void HideItemFromDesktop(GameObject item)
    {
        item.transform.position = new Vector3(
            FolderItem.POSITION_X_TO_HIDE_ITEM,
            item.transform.position.y,
            item.transform.position.z
        );
    }

    protected override void doInLeftClick()
    {
        DesktopRootReferenceManager.getInstance().folderController.showDialog(this);
    }

    protected override void doInMiddleClick() {}
}
