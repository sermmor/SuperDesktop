using System;
using UnityEngine;

[System.Serializable]
public class JSONMapperNoteWidgetItem: JSONMapperDesktopItem
{
    public bool isPinEnabled;

    public JSONMapperNoteWidgetItem(DesktopItem item) : base(item)
    {
        // Remember, in notes: text == nameFile.
        isPinEnabled = ((NoteItemWidget) item).IsPinEnabled;
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    public void parseJSONToItem(DesktopManager desktopManager)
    {
        GameObject generated = GameObject.Instantiate<GameObject>(LoadDesktops.Instance.NoteWidgetPrefab);
        // Position
        positionToPlaceNewItem.x = position.x;
        positionToPlaceNewItem.y = position.y;
        positionToPlaceNewItem.z = position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Item Propierties
        NoteItemWidget item = generated.GetComponent<NoteItemWidget>();
        item.desktopManager = desktopManager;
        item.NoteText = nameFile;
        item.IsPinEnabled = isPinEnabled;
    }
}