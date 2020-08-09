﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    ContextualMenuManager contextualMenu;
    AutoScaleBackgroundToCamera autoScaleBackground;

    float[] _bounds;

    public float[] Bounds {get => _bounds; } // minX, maxX, minY, maxY;

    public bool isMouseAboveDesktopItem {get; set;}
    MenuCaller menuCaller;

    List<DesktopItem> allItemsInDesktop;

    List<FolderItem> allFolders;

    public List<string> allFoldersNames { get => (from folderItem in allFolders select folderItem.nameFile).ToList(); }

    private float iconScale = 1;

    void Awake()
    {
        allItemsInDesktop = new List<DesktopItem>();
        allFolders = new List<FolderItem>();
        menuCaller = new MenuCaller();
        contextualMenu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        isMouseAboveDesktopItem = false;
        _bounds = null;
        iconScale = 1;
        autoScaleBackground = GetComponent<AutoScaleBackgroundToCamera>();
        if (autoScaleBackground) {
            StartCoroutine(doActionsWhenAutoScaleBackgroundIsEnding());
        }
        // StartCoroutine(test());
    }

    // IEnumerator test() // TODO DELETE BEFORE
    // {
    //     yield return new WaitForSeconds(5f);
    //     // changeImagePath("C:\\UnityWorkspace\\SuperDesktop\\Assets\\sprites\\backgrounds\\default.jpg");
    //     changeImagePath("D:\\Imagenes\\Manga Wallpapers");
    //     setTimeToAutoChangeWallpaper(15f);
    // }

    public void changeSizeIcons(float newPercentage)
    {
        // newPercentage is a value between 0 and 1.
        // 0.5 percentage === 1 scale (x, y) => newScale (x, y) = newPercentage / 0.5f
        float newScaleBase = newPercentage / 0.5f;
        Vector3 newScale;

        foreach (DesktopItem item in allItemsInDesktop)
        {
            newScale = new Vector3(newScaleBase, newScaleBase, item.transform.localScale.z);
            item.transform.localScale = newScale;
        }
    }

    public void changeImagePath(string newImagePath)
    {
        // Check if path is a file or a directory.
        FileAttributes attr = File.GetAttributes(newImagePath);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            // Is directory.
            autoScaleBackground.changeImageList(Directory.EnumerateFiles(newImagePath).ToArray());
        }
        else
        {
            autoScaleBackground.changeImageList(new string[] {newImagePath});
        }
    }

    public void setTimeToAutoChangeWallpaper(float timeInSeconds)
    {
        autoScaleBackground.setTimeToAutoChangeWallpaper(timeInSeconds);
    }

    public void addItemToDeskop(DesktopItem desktopItem)
    {
        allItemsInDesktop.Add(desktopItem);

        if (desktopItem is FolderItem)
            allFolders.Add((FolderItem) desktopItem);
    }

    public FolderItem getFolderByName(string nameFolder)
    {
        foreach (FolderItem item in allFolders)
        {
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
            List<GameObject> toDelete = new List<GameObject>();
            foreach (GameObject goCandidate in folder.ItemList)
            {
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

    IEnumerator doActionsWhenAutoScaleBackgroundIsEnding() {
        if (autoScaleBackground.IsAutoScaling) {
            yield return null;
        }

        // Calculate all the screen limits.
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height / Screen.height * Screen.width;
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

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
}
