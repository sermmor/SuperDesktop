using UnityEngine;

public class MenuCaller
{
    public DesktopManager DesktopManagerCaller { get => this._desktopManagerCaller; }
    DesktopManager _desktopManagerCaller;
    public DesktopItem DesktopItemCaller { get => this._desktopItemCaller; }
    DesktopItem _desktopItemCaller;

    public void setCaller(MonoBehaviour mono)
    {
        this._desktopManagerCaller = null;
        this._desktopItemCaller = null;
        
        if (mono is DesktopManager)
            this._desktopManagerCaller = (DesktopManager) mono;
        else if (mono is DesktopItem)
            this._desktopItemCaller = (DesktopItem) mono;
    }
}
