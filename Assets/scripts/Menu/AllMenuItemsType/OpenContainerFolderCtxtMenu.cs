using System.IO;
using System.Diagnostics;

public class OpenContainerFolderCtxtMenu : ContextualMenuItem
{
    protected override void doOnLeftClick()
    {
        if (this.whoIsCallMe.DesktopItemCaller is FileItem)
        {
            Process.Start(
                ((FileItem) this.whoIsCallMe.DesktopItemCaller).directoryFilePath
            );
        }
    }
}