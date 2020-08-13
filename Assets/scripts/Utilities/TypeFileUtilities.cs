using System.Linq;
using UnityEngine;

public static class TypeFileUtilities
{
    static bool isInTypeExtensionList(string[] extensionList, string extensionFile) {
        string extensionFileFixed = extensionFile.ToLower();
        
        string[] finded = (from extensionToCheck in extensionList
            where extensionToCheck.Equals(extensionFileFixed)
            select extensionToCheck).ToArray();

        return finded != null && finded.Length > 0;
    }

    static string[] textExtensions = new[]{"txt", "doc", "docx", "xls", "xlsx", "pdf", "epub", "odt", "ppt", "pptx"};
    public static bool IsTypeText(string extensionFile) => isInTypeExtensionList(textExtensions, extensionFile);

    static string[] rareImageExtensions = new[]{"gif", "ptg", "clip", "ico"};
    static string[] imageExtensions = new[]{"jpg", "jpeg", "png", "gif", "svg", "ptg", "clip", "ico"};
    public static bool IsTypeImage(string extensionFile) => isInTypeExtensionList(imageExtensions, extensionFile);
    public static bool IsRareTypeImage(string extensionFile) => isInTypeExtensionList(rareImageExtensions, extensionFile);

    static string[] videoExtensions = new[]{"avi", "mp4", "wmv", "flv", "mpg", "mpeg", "asx"};
    public static bool IsTypeVideo(string extensionFile) => isInTypeExtensionList(videoExtensions, extensionFile);

    static string[] linkExtensions = new[]{"slink"};
    public static bool IsTypeLink(string extensionFile) => isInTypeExtensionList(linkExtensions, extensionFile);

    static string[] folderExtensions = new[]{"sfolder"};
    public static bool IsTypeFolder(string extensionFile) => isInTypeExtensionList(folderExtensions, extensionFile);

    public static bool IsTypeNone(string extensionFile) => !IsTypeFolder(extensionFile) && !IsTypeLink(extensionFile) && !IsTypeVideo(extensionFile) && !IsTypeImage(extensionFile) && !IsTypeText(extensionFile);

    public static string getExtensionFile(string nameFileOrCompletedPath)
    {
        string[] splitedByPoints = nameFileOrCompletedPath.Split('.');
        return splitedByPoints[splitedByPoints.Length - 1].ToLower();
    }

    public static Sprite getSpriteOfFile(string nameFileOrCompletedPath)
    {
        string extensionFile = getExtensionFile(nameFileOrCompletedPath);

        if (IsTypeText(extensionFile)) {
            return FileSpriteByType.getSpriteByType(TypeFile.TEXT);
        } else if (IsTypeImage(extensionFile)) {
            return FileSpriteByType.getSpriteByType(TypeFile.IMAGE);
        } else if (IsTypeVideo(extensionFile)) {
            return FileSpriteByType.getSpriteByType(TypeFile.VIDEO);
        } else if (IsTypeLink(extensionFile)) {
            return FileSpriteByType.getSpriteByType(TypeFile.LINK);
        } else if (IsTypeFolder(extensionFile)) {
            return FileSpriteByType.getSpriteByType(TypeFile.FOLDER);
        } else if (nameFileOrCompletedPath.ToLower().Equals(extensionFile) || "".Equals(extensionFile) || extensionFile == null) {
            return FileSpriteByType.getSpriteByType(TypeFile.FILE_SYSTEM_FOLDER);
        }

        return FileSpriteByType.getSpriteByType(TypeFile.OTHER);
    }
}