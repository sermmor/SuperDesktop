using UnityEngine;

public class StickElementCtxtMenu: ContextualMenuItem
{
    bool isInStickMode;
    TextMesh textMenu;

    protected override void doOnEnable()
    {
        if (textMenu == null)
            textMenu = transform.Find("Text").GetComponent<TextMesh>();
        
        if (((VideoItem) this.whoIsCallMe.DesktopItemCaller).isSticked)
        {
            isInStickMode = true;
            textMenu.text = "Unstick";
        }
        else
        {
            isInStickMode = false;
            textMenu.text = "Stick";
        }
    }

    protected override void doOnLeftClick()
    {
        ((VideoItem) this.whoIsCallMe.DesktopItemCaller).stickNow(!isInStickMode);
    }
}
