using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadDesktops: MonoBehaviour
{
    public GameObject FileItemPrefab;
    public GameObject FolderItemPrefab;
    public GameObject GroupItemWidgetPrefab;
    public GameObject LinkItemPrefab;
    public GameObject NoteWidgetPrefab;
    public GameObject VideoItemPrefab;
    public GameObject WidgetImagePrefab;

    private static LoadDesktops _instance;

    public static LoadDesktops Instance { get => _instance; }

    void Awake()
    {
        _instance = this;    
    }

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
