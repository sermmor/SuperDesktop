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
}
