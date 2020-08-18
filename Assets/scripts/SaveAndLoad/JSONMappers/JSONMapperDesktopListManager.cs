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

    public static void parseJSONToDesktopListManager(string jsonData)
    {
        JSONMapperDesktopListManager superDesktop = JsonUtility.FromJson<JSONMapperDesktopListManager>(jsonData);

        DesktopRootReferenceManager.getInstance().desktopListManager.NumberOfColumns = superDesktop.totalColumns;
        DesktopRootReferenceManager.getInstance().desktopListManager.NumberOfDesktop = superDesktop.desktopList.Length;
        
        DesktopManager desktop;
        for (int i = 0; i < superDesktop.desktopList.Length; i++)
        {
            desktop = DesktopRootReferenceManager.getInstance().desktopListManager.AllDesktops[i];
            
            superDesktop.desktopList[i].parseJSONToDesktop(desktop);
            DesktopRootReferenceManager.getInstance().desktopBigPreviews.changeBackgroundPreviewToDesktop(
                i,
                desktop.autoScaleBackground.backgroundPathList
            );
        }
    }
}
