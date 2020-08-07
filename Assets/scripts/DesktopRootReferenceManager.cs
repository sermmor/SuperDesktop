using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopRootReferenceManager : MonoBehaviour
{
    static DesktopRootReferenceManager _instance;
    public static DesktopRootReferenceManager getInstance() => DesktopRootReferenceManager._instance;

    // For general use or singleton collection.
    public ContextualMenuManager contextualMenuManager;
    public FolderController folderController;
    public DesktopManager currentDesktopShowed; // TODO: When change to desktop, assign the new Destop.
    public FileSpriteByType[] typeFileIconList;

    void Awake() => _instance = this;
}
