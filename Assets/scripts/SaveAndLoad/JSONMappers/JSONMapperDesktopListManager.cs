using System.Collections.Generic;
using UnityEngine;

public class JSONMapperDesktopListManager
{
    public int totalColumns;

    public JSONMapperDesktopManager[] desktopList;

    public string mapDesktopListManagerToJSON()
    {
        List<DesktopManager> allDesktops = DesktopRootReferenceManager.getInstance().desktopListManager.AllDesktops;
        totalColumns = DesktopRootReferenceManager.getInstance().desktopListManager.NumberOfColumns;
        desktopList = new JSONMapperDesktopManager[allDesktops.Count];

        for (int i = 0; i < desktopList.Length; i++)
        {
            desktopList[i] = new JSONMapperDesktopManager(allDesktops[i]);
        }
        return JsonUtility.ToJson(this, true);
    }
}
