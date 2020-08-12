using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AutoSaver : MonoBehaviour
{
    public string nameDesktop = "";
    public float secondsToSave = 15f;

    public bool MarkToSave { get; set; }

    string filePath { get => Path.Combine(Application.persistentDataPath, $"{nameDesktop}.json"); }

    JSONMapperDesktopListManager mapper = new JSONMapperDesktopListManager();
    
    void Start()
    {
        MarkToSave = false;
        mapper = new JSONMapperDesktopListManager();
        StartCoroutine(autoSaverRoutine());
        // MarkToSave = true; // TODO: DEBUG, DELETE THIS LINE!!!
    }

    IEnumerator autoSaverRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsToSave);
            if (MarkToSave) {
                saveNow();
                MarkToSave = false;
            }
        }
    }

    void saveNow()
    {
        string json = mapper.mapDesktopListManagerToJSON();
        // TODO: Save all list to JSON.
        System.IO.File.WriteAllText(filePath, json);
        // FileStream file;

        // if(File.Exists(filePath)) file = File.OpenWrite(filePath);
        // else file = File.Create(filePath);
        
        // file.Close();
        Debug.Log($"Saved in {filePath}");
    }
}

[System.Serializable]
public class Test {
    public int t1 = 42;
    public string nameTest = "saveTest";
}