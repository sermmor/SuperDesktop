using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    ContextualMenuManager contextualMenu {get => DesktopRootReferenceManager.getInstance().contextualMenuManager; }
    public AutoScaleBackgroundToCamera autoScaleBackground;

    float[] _bounds = null;

    public float[] Bounds {get => _bounds; } // minX, maxX, minY, maxY;

    public bool isMouseAboveDesktopItem {get; set;}
    MenuCaller menuCaller;

    List<DesktopItem> allItemsInDesktop;

    List<FolderItem> allFolders;

    public List<string> allFoldersNames { get => (from folderItem in allFolders select folderItem.nameFile).ToList(); }

    float _iconScale = 1;
    public float IconRealScale { get => _iconScale; set => _iconScale = value; }
    public float IconScalePercentage { get => _iconScale * 0.5f; } // 1 scale (x, y) == 0.5 percentage => newScale (x, y) = _iconScale * 0.5f
    public string WallpaperImagePath { get => autoScaleBackground.WallpaperImagePath; }
    public float SecondsToChangeWallpaper { get => autoScaleBackground.SecondsToAutoChangeWallpaper; }

    public AutoScaleMode ModeWallpaper { get => autoScaleBackground.mode; }

    public List<DesktopItem> AllItemsInDesktop { get => allItemsInDesktop; }
    public List<FolderItem> AllFolders { get => allFolders; }

    void OnEnable()
    {
        if (autoScaleBackground == null)
        {
            allItemsInDesktop = new List<DesktopItem>();
            allFolders = new List<FolderItem>();
            menuCaller = new MenuCaller();
            isMouseAboveDesktopItem = false;
            _bounds = null;
            _iconScale = 1;
            autoScaleBackground = GetComponent<AutoScaleBackgroundToCamera>();
            if (autoScaleBackground) {
                StartCoroutine(doActionsWhenAutoScaleBackgroundIsEnding());
            }
        }
    }

    public void changeModeWallpaper(AutoScaleMode modeWallpaper) => autoScaleBackground.changeToMode(modeWallpaper);

    public void changeSizeIcons(float newPercentage)
    {
        // newPercentage is a value between 0 and 1.
        // 0.5 percentage === 1 scale (x, y) => newScale (x, y) = newPercentage / 0.5f
        float newScaleBase = newPercentage / 0.5f;
        _iconScale = newScaleBase;

        foreach (DesktopItem item in allItemsInDesktop)
            changeSizeIcons(item, newScaleBase);
    }

    void changeSizeIcons(DesktopItem item, float scaleIcon)
    {
        if (item == null || item is VideoItem || item is GroupItemWidget || item is ImageBackgroundItemWidget)
            return;
        
        Vector3 newScale = new Vector3(scaleIcon, scaleIcon, item.transform.localScale.z);
        item.transform.localScale = newScale;
    }

    public void changeImagePath(string newImagePath)
    {
        if (!"default".Equals(newImagePath))
        {
            // Check if path is a file or a directory.
            FileAttributes attr = File.GetAttributes(newImagePath);

            if (attr.HasFlag(FileAttributes.Directory)) // Is directory.
                autoScaleBackground.changeImageList(Directory.EnumerateFiles(newImagePath).ToArray());
            else
                autoScaleBackground.changeImageList(new string[] {newImagePath});
        }
        else
        {
            autoScaleBackground.changeImageList(new string[] {newImagePath});
        }

        DesktopRootReferenceManager.getInstance().desktopBigPreviews.changeBackgroundPreviewCurrentDesktop(newImagePath);
    }

    public void setTimeToAutoChangeWallpaper(float timeInSeconds)
    {
        autoScaleBackground.setTimeToAutoChangeWallpaper(timeInSeconds);
    }

    public void addItemToDeskop(DesktopItem desktopItem)
    {
        // desktopItem.transform.SetParent(transform);
        desktopItem.transform.SetParent(DesktopRootReferenceManager.getInstance().allIconsParent.transform);

        allItemsInDesktop.Add(desktopItem);

        changeSizeIcons(desktopItem, _iconScale);

        if (desktopItem is FolderItem)
            allFolders.Add((FolderItem) desktopItem);
    }

    public DesktopItem getItemByName(string nameItem)
    {
        foreach (DesktopItem item in allItemsInDesktop)
        {
            if (item == null || item is VideoItem || item is GroupItemWidget || item is ImageBackgroundItemWidget)
                continue;
            if (item.nameFile.Equals(nameItem))
                return item;
        }
        return null;
    }
    public FolderItem getFolderByName(string nameFolder)
    {
        foreach (FolderItem item in allFolders)
        {
            if (item == null) continue;
            if (item.nameFile.Equals(nameFolder))
                return item;
        }
        return null;
    }

    public void RemoveFromFolderAndPutInDesktop(DesktopItem item)
    {
        bool isItemDeleted = false;
        foreach (FolderItem folder in allFolders)
        {
            if (item == null) continue;
            List<GameObject> toDelete = new List<GameObject>();
            foreach (GameObject goCandidate in folder.ItemList)
            {
                if (goCandidate == null) continue;
                DesktopItem candidate = goCandidate.GetComponent<DesktopItem>();
                if (item.nameFile.Equals(candidate.nameFile))
                {
                    toDelete.Add(goCandidate);
                    isItemDeleted = true;
                }
            }
            
            // Remove candidates.
            foreach (GameObject goToDelete in toDelete)
                folder.ItemList.Remove(goToDelete);

            if (isItemDeleted)
                return;
        }
    }

    IEnumerator doActionsWhenAutoScaleBackgroundIsEnding()
    {
        while (autoScaleBackground.IsAutoScaling) {
            yield return null;
        }

        forceToCalculateBounds();
    }

    public void forceToCalculateBounds()
    {
        // Calculate all the screen limits.
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height / Screen.height * Screen.width;
        // float x = Camera.main.transform.position.x;
        // float y = Camera.main.transform.position.y;
        float x = transform.position.x;
        float y = transform.position.y;

        _bounds = new float[]{
            x - (width / 2), // minX
            (width / 2) + x, // maxX
            y - (height / 2), // minY
            (height / 2) + y // maxY
        };
    }

    void Update()
    {
        if (!isMouseAboveDesktopItem && Input.GetMouseButtonUp(1))
            doInRightClick();
        else if (contextualMenu.isOpen && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2)))
            contextualMenu.close();
    }

    void doInRightClick()
    {
        if (contextualMenu != null)
        {
            menuCaller.setCaller(this);
            contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.DESKTOP);
        }
    }

    public void DestroyMe()
    {
        foreach (DesktopItem item in allItemsInDesktop)
            Destroy(item.gameObject);

        Destroy(gameObject);
    }
}
