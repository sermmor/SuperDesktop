using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class JSONMapperDesktopManager
{
    public JSONMapperAutoScaleBackgroundToCamera autoScaleBackground;
    public JSONMapperFileItem[] allFilesInDesktop;
    public JSONMapperLinkItem[] allLinkInDesktop;
    public JSONMapperVideoItem[] allVideosInDesktop;
    public JSONMapperGroupItemWidget[] allGroupItemInDesktop;
    public JSONMapperWidgetImageItem[] allImageBackgroundInDesktop;
    public JSONMapperNoteWidgetItem[] allNotesInDesktop;
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
        allFilesInDesktop = (from item in allItemsInDesktop where (item is FileItem && item != null) select new JSONMapperFileItem(item)).ToArray();
        allLinkInDesktop = (from link in allItemsInDesktop where (link is LinkItem && link != null) select new JSONMapperLinkItem(link)).ToArray();
        allVideosInDesktop = (from video in allItemsInDesktop where (video is VideoItem && video != null) select new JSONMapperVideoItem(video)).ToArray();
        allGroupItemInDesktop = (from groupItem in allItemsInDesktop where (groupItem is GroupItemWidget && groupItem != null) select new JSONMapperGroupItemWidget(groupItem)).ToArray();
        allImageBackgroundInDesktop = (from image in allItemsInDesktop where (image is ImageBackgroundItemWidget && image != null) select new JSONMapperWidgetImageItem(image)).ToArray();
        allNotesInDesktop = (from note in allItemsInDesktop where (note is NoteItemWidget && note != null) select new JSONMapperNoteWidgetItem(note)).ToArray();
    }

    public void parseJSONToDesktop(DesktopManager desktopManager)
    {
        desktopManager.IconRealScale = iconRealScale;
        autoScaleBackground.parseJSONToAutoScaleBackground(desktopManager.autoScaleBackground);

        foreach (JSONMapperFileItem file in allFilesInDesktop) file.parseJSONToItem(desktopManager);
        foreach (JSONMapperLinkItem link in allLinkInDesktop) link.parseJSONToItem(desktopManager);
        foreach (JSONMapperGroupItemWidget widget in allGroupItemInDesktop) widget.parseJSONToItem(desktopManager);        
        foreach (JSONMapperVideoItem widget in allVideosInDesktop) widget.parseJSONToItem(desktopManager);
        foreach (JSONMapperWidgetImageItem widget in allImageBackgroundInDesktop) widget.parseJSONToItem(desktopManager);
        foreach (JSONMapperNoteWidgetItem widget in allNotesInDesktop) widget.parseJSONToItem(desktopManager);
        foreach (JSONMapperFolderItem folder in allFolders) folder.parseJsonToFolder(desktopManager);
    }

    JSONMapperFolderItem[] parseFolders(List<FolderItem> allFolderInDesktop)
    {
        return (from folder in allFolderInDesktop
            where folder != null
            select new JSONMapperFolderItem(folder)
        ).ToArray();
    }
}
