using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LoadDesktops
{
    public static string[] GetAllFilesPathToLoad()
    {
        string loadPath = Application.persistentDataPath;

        FileAttributes attr = File.GetAttributes(loadPath);

        if (attr.HasFlag(FileAttributes.Directory)) // Is directory.
            return Directory.EnumerateFiles(loadPath).ToArray();

        // Path.Combine(Application.persistentDataPath, $"{nameDesktop}.json");
        // Application.persistentDataPath

        return null;
    }

    public static string LoadData(string jsonPath)
    {
        return System.IO.File.ReadAllText(jsonPath);
    }
}
