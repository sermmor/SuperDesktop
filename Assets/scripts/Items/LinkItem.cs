using UnityEngine;

public class LinkItem : DesktopItem
{
    public string urlPath;

    protected override void thingsToDoAfterStart()
    {

    }

    public override void setFileName(string nameFile)
    {
        base.setFileName(nameFile);
        setSpriteByFileName();
    }

    void setSpriteByFileName() => spriteFile.sprite = FileSpriteByType.getSpriteByType(TypeFile.LINK);

    protected override void doInLeftClick()
    {
        Application.OpenURL(urlPath);
    }

    protected override void doInMiddleClick() { }
}
