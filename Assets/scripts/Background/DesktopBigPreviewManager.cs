using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DesktopBigPreviewManager : MonoBehaviour
{
    public GameObject desktopBigPreviewPrefab;

    Dictionary<int, AutoScaleBackgroundToCamera> mapIndexPreview;

    int nextIndex;

    public void createNewPreview(Vector3 position, int indexPreview)
    {
        if (mapIndexPreview == null) createStructure();

        GameObject generated = GameObject.Instantiate<GameObject>(desktopBigPreviewPrefab);
        generated.transform.parent = transform;
        generated.transform.position = new Vector3(position.x, position.y, -8);
        generated.SetActive(true);
        mapIndexPreview.Add(nextIndex, generated.GetComponent<AutoScaleBackgroundToCamera>());
        nextIndex++;
    }

    public void deletePreview(int index)
    {
        Destroy(mapIndexPreview[index].gameObject);
        mapIndexPreview.Remove(index);
    }

    public void clearAllPreviews()
    {
        if (mapIndexPreview == null) return;

        int[] allIndexs = (from key in mapIndexPreview.Keys select key).ToArray();
        foreach (var index in allIndexs)
        {
            Destroy(mapIndexPreview[index].gameObject);
            mapIndexPreview.Remove(index);
        }
        mapIndexPreview.Clear();
        nextIndex = 0;
    }
    
    public void enablePreview(int indexPreview, bool isEnable)
    {
        mapIndexPreview[indexPreview].gameObject.SetActive(isEnable);
    }

    private void createStructure()
    {
        mapIndexPreview = new Dictionary<int, AutoScaleBackgroundToCamera>();
        nextIndex = 0;
    }

    public void changeBackgroundPreviewCurrentDesktop(string wallpaperPath)
    {
        int index = DesktopRootReferenceManager.getInstance().desktopListManager.CurrentDesktopShowedIndex;
        bool activeSelf = mapIndexPreview[index].gameObject.activeSelf;
        mapIndexPreview[index].gameObject.SetActive(true);

        changeBackgroundPreview(
            index,
            wallpaperPath
        );
        
        mapIndexPreview[index].gameObject.SetActive(activeSelf);
    }

    void changeBackgroundPreview(int index, string wallpaperPath)
    {
        AutoScaleBackgroundToCamera toChangeWallpaper = mapIndexPreview[index];

        if (!"default".Equals(wallpaperPath))
        {
            // Check if path is a file or a directory.
            FileAttributes attr = File.GetAttributes(wallpaperPath);

            if (attr.HasFlag(FileAttributes.Directory)) // Is directory.
                toChangeWallpaper.changeImageList(Directory.EnumerateFiles(wallpaperPath).ToArray());
            else
                toChangeWallpaper.changeImageList(new string[] {wallpaperPath});
        }
        else
        {
            toChangeWallpaper.changeImageList(new string[] {wallpaperPath});
        }
    }

}
