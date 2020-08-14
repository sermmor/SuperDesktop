using UnityEngine;

[System.Serializable]
public class JSONMapperWidgetImageItem : JSONMapperDesktopItem
{
    public string imagePath;
    public Vector3 scale;

    public JSONMapperWidgetImageItem(DesktopItem item) : base(item)
    {
        imagePath = ((ImageBackgroundItemWidget) item).ImagePath;
        scale = item.transform.localScale;
    }
}
