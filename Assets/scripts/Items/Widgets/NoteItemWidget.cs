using UnityEngine;

public class NoteItemWidget: DesktopItem
{
    public string NoteText { get => nameFile; set => setFileName(value); }
    public bool IsPinEnabled { get => isShowingPin; set => setPinEnabled(value); }

    bool isShowingPin = true;
    CreateOrModifyNoteWidgetDialogCtrl dialogEditNote;

    MenuCaller menuCaller = new MenuCaller();

    protected override void thingsToDoAfterStart()
    {
        menuCaller.setCaller(DesktopRootReferenceManager.getInstance().CurrentDesktopShowed);
        dialogEditNote = DesktopRootReferenceManager.getInstance().noteDialog;
        spriteFile = transform.Find("pin").GetComponent<SpriteRenderer>();
        spriteFile.enabled = isShowingPin;
        nameFileTextMesh.text = nameFile.Replace("\\n", "\n");
    }

    void setPinEnabled(bool isEnabled)
    {
        isShowingPin = isEnabled;
        if (spriteFile)
            spriteFile.enabled = isShowingPin;
    }

    protected override void doInLeftClick()
    {
        dialogEditNote.NoteCalledby = this;
        dialogEditNote.showDialog(menuCaller);
    }

    protected override void doInMiddleClick() {}
    
}
