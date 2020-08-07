using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class FileItem : DesktopItem
{
    const float DEFAULT_WIDTH_FILE = 55;

    public string filePath;

    public string directoryFilePath { get => this._directoryFilePath; }
    string _directoryFilePath;

    bool isWindowsOS = true;

    protected override void thingsToDoAfterStart()
    {
        if (filePath != null) {
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
                    isWindowsOS = false;
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
        } else {
            UnityEngine.Debug.LogError($"Path of the file {this.nameFile} is null!");
        }
    }

    private void checkDirectoryPathExistence() {
        if (!Directory.Exists(_directoryFilePath)) {
            _directoryFilePath = null;
            UnityEngine.Debug.LogError($"Path of the file {this.nameFile} doesn't exist!");
        }
    }

    public override void setFileName(string nameFile)
    {
        base.setFileName(nameFile);
        
        string extensionFile = CheckTypeFile.getExtensionFile(filePath);

        if (CheckTypeFile.IsTypeImage(extensionFile) && !CheckTypeFile.IsRareTypeImage(extensionFile))
        {
            try
            {
                spriteFile.sprite = SpriteLoader.LoadSprite(filePath);
                setSpriteFileIconToDefaultWidthSize();
            }
            catch (System.Exception ex)
            {
                setSpriteByFileName();
            }
        }
        else
        {
            setSpriteByFileName();
        }
    }

    void setSpriteByFileName() => spriteFile.sprite = CheckTypeFile.getSpriteOfFile(filePath);

    Vector3 scaleCalculatedForAutoScaleIcon = new Vector3(0, 0, 0);
    void setSpriteFileIconToDefaultWidthSize() {
        float width = spriteFile.sprite.textureRect.width;
        float height = spriteFile.sprite.textureRect.height;
        float currentScale = height / width;

        // Get the world size.
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        
        float scale = (DEFAULT_WIDTH_FILE * currentScale) / height;
        
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
