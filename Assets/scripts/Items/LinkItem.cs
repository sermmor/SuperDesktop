using UnityEngine;

public class LinkItem : DesktopItem
{
    public string urlPath;

    protected override void thingsToDoAfterStart() {}

    protected override void doInLeftClick()
    {
        Application.OpenURL(urlPath);
    }

    protected override void doInMiddleClick() {}
}
