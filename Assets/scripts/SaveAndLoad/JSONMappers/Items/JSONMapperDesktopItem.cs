[System.Serializable]
public class JSONMapperDesktopItem
{
    public string nameFile;

    public JSONMapperDesktopItem(DesktopItem item)
    {
        nameFile = item.nameFile;
    }

    public static JSONMapperDesktopItem createJSONMapperDesktopItem(DesktopItem item)
    {
        if (item is FileItem) return new JSONMapperFileItem(item);
        if (item is FolderItem) return new JSONMapperFolderItem(item);
        if (item is LinkItem) return new JSONMapperLinkItem(item);
        return null;
    }
}
