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
    public GameObject colliderBackgroundForDialogs;
    public DesktopListManager desktopListManager;
    public GameObject temporalBackground;
    public DesktopBigPreviewManager desktopBigPreviews;
    public FileSpriteByType[] typeFileIconList;
    public DesktopManager CurrentDesktopShowed { get => desktopListManager.CurrentDesktopShowed; }

    void Awake() => _instance = this;
}
