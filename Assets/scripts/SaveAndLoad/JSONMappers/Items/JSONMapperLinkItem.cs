using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONMapperLinkItem : JSONMapperDesktopItem
{
    public string urlPath;

    public JSONMapperLinkItem(DesktopItem item) : base(item)
    {
        urlPath = ((LinkItem) item).urlPath;
    }
}
