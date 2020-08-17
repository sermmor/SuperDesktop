using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class AutoSaver : MonoBehaviour
{
    public string nameDesktop = "";
    public float secondsToSave = 15f;

    public bool MarkToSave { get; set; }
    public bool IsAllSavesBlock { get; set; }

    string filePath { get => Path.Combine(Application.persistentDataPath, $"{nameDesktop}.json"); }

    JSONMapperDesktopListManager mapper = new JSONMapperDesktopListManager();
    
    void Start()
    {
        IsAllSavesBlock = false;
        MarkToSave = false;
        mapper = new JSONMapperDesktopListManager();
        StartCoroutine(autoSaverRoutine());
    }

    IEnumerator autoSaverRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsToSave);
            if (MarkToSave && !IsAllSavesBlock) {
                saveNow();
                MarkToSave = false;
            }
        }
    }

    public void forceToSave()
    {
        MarkToSave = false;
        saveNow();
    }

    void saveNow()
    {
        string json = mapper.mapDesktopListManagerToJSON();
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log($"Saved in {filePath}");
    }

    public void blockAllSaves(bool isBlockAllSaves) => IsAllSavesBlock = isBlockAllSaves;
}
