using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using UnityEngine;

public class FileItem : DesktopItem
{
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

    protected override void doInLeftClick()
    {
        // Open file or directory (Explorer).
        Process.Start(filePath);
    }

    protected override void doInMiddleClick() { }
}
