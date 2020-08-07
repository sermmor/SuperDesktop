using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TypeFile { OTHER, TEXT, IMAGE, VIDEO, LINK, FOLDER };

[System.Serializable]
public class FileSpriteByType
{
    public TypeFile typeFile;
    public Sprite sprite;

    public static Sprite getSpriteByType(TypeFile typeFile)
    {
        if (DesktopRootReferenceManager.getInstance() != null)
        {
            return (from fileIcon in DesktopRootReferenceManager.getInstance().typeFileIconList
                where fileIcon.typeFile == typeFile
                select fileIcon).ToArray()[0].sprite;
        }
        else
        {
            // Show type other.
            return (from fileIcon in DesktopRootReferenceManager.getInstance().typeFileIconList
                where fileIcon.typeFile == TypeFile.OTHER
                select fileIcon).ToArray()[0].sprite;
        }
    }
}
