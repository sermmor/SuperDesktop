using UnityEngine;

[System.Serializable]
public class JSONMapperVideoItem : JSONMapperDesktopItem
{
    public string pathVideo;
    public Vector3 scale;

    public JSONMapperVideoItem(DesktopItem item) : base(item)
    {
        pathVideo = ((VideoItem) item).pathVideo;
        scale = item.transform.localScale;
    }
}
