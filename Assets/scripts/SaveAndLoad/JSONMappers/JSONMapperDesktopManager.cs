using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class JSONMapperDesktopManager
{
    public JSONMapperAutoScaleBackgroundToCamera autoScaleBackground;
    public JSONMapperFileItem[] allFilesInDesktop;
    public JSONMapperLinkItem[] allLinkInDesktop;
    public JSONMapperFolderItem[] allFolders;
    public float iconRealScale;

    public JSONMapperDesktopManager(DesktopManager desktop)
    {
        iconRealScale = desktop.IconRealScale;

        autoScaleBackground = new JSONMapperAutoScaleBackgroundToCamera(desktop.GetComponent<AutoScaleBackgroundToCamera>());

        parseItems(desktop.AllItemsInDesktop);

        allFolders = parseFolders(desktop.AllFolders);
    }

    void parseItems(List<DesktopItem> allItemsInDesktop)
    {
        JSONMapperDesktopItem[] listItems = new JSONMapperDesktopItem[allItemsInDesktop.Count];
        allFilesInDesktop = (from item in allItemsInDesktop where (item is FileItem) select new JSONMapperFileItem(item)).ToArray();
        allLinkInDesktop = (from link in allItemsInDesktop where (link is LinkItem) select new JSONMapperLinkItem(link)).ToArray();
    }

    JSONMapperFolderItem[] parseFolders(List<FolderItem> allFolderInDesktop)
    {
        JSONMapperFolderItem[] listItems = new JSONMapperFolderItem[allFolderInDesktop.Count];
        
        for (int i = 0; i < allFolderInDesktop.Count; i++)
            listItems[i] = new JSONMapperFolderItem(allFolderInDesktop[i]);

        return listItems;
    }
}
