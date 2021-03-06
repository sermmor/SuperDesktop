﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class FileItem : DesktopItem
{
    float defaultWidthFilePreviewIcon = 60;

    public string filePath;

    public string directoryFilePath { get => this._directoryFilePath; }
    string _directoryFilePath;

    string iconPath;
    public string IconPath {
        get => iconPath;
        set {
            iconPath = value;
            if (spriteFile == null || _directoryFilePath == null) return; // Icon will be changed when it's created!

            spriteFile.transform.localScale = Vector3.one;
            if (iconPath != null && !"".Equals(iconPath))
                setIconChoosedByUser();
            else
                setFileName(nameFile);

            DesktopRootReferenceManager.getInstance().autoSaver.MarkToSave = true;
        }
    }

    protected override void thingsToDoAfterStart()
    {
        if (filePath != null && !"".Equals(filePath)) {
            // Check if path is a file or a directory.
            FileAttributes attr = File.GetAttributes(filePath);
            if (attr.HasFlag(FileAttributes.Directory)) {
                _directoryFilePath = filePath;
                checkDirectoryPathExistence();
            } else {
                char separator = '\\';
                string[] pathSplited = filePath.Split('\\');
                if (pathSplited.Length == 1) {
                    separator = '/';
                    pathSplited = filePath.Split('/');
                }
                if (pathSplited.Length > 1) {
                    string[] directoryPathSplite = (from pathPiece in pathSplited
                        where pathPiece != pathSplited[pathSplited.Length - 1]
                        select pathPiece).ToArray();
                    // joint the directory path
                    _directoryFilePath = directoryPathSplite.Aggregate((acc, next) => (
                        acc == null ? next : $"{acc}{separator}{next}"
                    ));
                    checkDirectoryPathExistence();
                } else {
                    UnityEngine.Debug.LogError("A file cannot place in the root (but it will be in C:\\ or D:\\");
                }
            }

            // Check Icon.
            if (iconPath != null && !"".Equals(iconPath))
                setIconChoosedByUser();
        } else {
            UnityEngine.Debug.LogError($"Path of the file {this.nameFile} is null!");
        }
    }

    private void setIconChoosedByUser() => checkAndSetIconImageFile(iconPath);

    private void checkDirectoryPathExistence() {
        if (!Directory.Exists(_directoryFilePath)) {
            _directoryFilePath = null;
            UnityEngine.Debug.LogError($"Path of the file {this.nameFile} doesn't exist!");
        }
    }

    public override void setFileName(string nameFile)
    {
        base.setFileName(nameFile);
        checkAndSetIconImageFile(filePath);
    }

    void checkAndSetIconImageFile(string imageIconPath)
    {
        string extensionFile = TypeFileUtilities.getExtensionFile(imageIconPath);

        if (TypeFileUtilities.IsTypeImage(extensionFile) && !TypeFileUtilities.IsRareTypeImage(extensionFile))
        {
            try
            {
                spriteFile.sprite = SpriteLoaderUtility.LoadSprite(imageIconPath);
                setSpriteFileIconToDefaultWidthSize();
            }
            catch (System.Exception ex)
            {
                setSpriteGenericByFileName();
            }
        }
        else
        {
            setSpriteGenericByFileName();
        }
    }

    void setSpriteGenericByFileName() => spriteFile.sprite = TypeFileUtilities.getSpriteOfFile(filePath);

    Vector3 scaleCalculatedForAutoScaleIcon = new Vector3(0, 0, 0);
    void setSpriteFileIconToDefaultWidthSize() {
        float width = spriteFile.sprite.textureRect.width;
        float height = spriteFile.sprite.textureRect.height;
        float currentScale = height / width;

        // Get the world size.
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float scale = (defaultWidthFilePreviewIcon * currentScale) / height;
        
        scaleCalculatedForAutoScaleIcon.x = scaleCalculatedForAutoScaleIcon.y = scale;
        scaleCalculatedForAutoScaleIcon.z = spriteFile.transform.localScale.z;

        spriteFile.transform.localScale = scaleCalculatedForAutoScaleIcon;
    }

    protected override void doInLeftClick()
    {
        // Open file or directory (Explorer).
        Process.Start(filePath);
    }

    protected override void doInMiddleClick() { }
}
