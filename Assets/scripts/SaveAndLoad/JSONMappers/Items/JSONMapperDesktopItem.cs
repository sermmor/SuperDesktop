using UnityEngine;

[System.Serializable]
public class JSONMapperDesktopItem
{
    public string nameFile;
    public Vector3 position;

    public JSONMapperDesktopItem(DesktopItem item)
    {
        nameFile = item.nameFile;
        position = item.transform.position;
    }
}
