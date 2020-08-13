using UnityEngine;

[System.Serializable]
public class JSONMapperGroupItemWidget : JSONMapperDesktopItem
{
    public Vector3 scale;

    public JSONMapperGroupItemWidget(DesktopItem item) : base(item)
    {
        scale = item.transform.localScale;
    }    
}
