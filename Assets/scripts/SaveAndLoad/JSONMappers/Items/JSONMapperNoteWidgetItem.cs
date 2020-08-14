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
}