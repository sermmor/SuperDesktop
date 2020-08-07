using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopRootReferenceManager : MonoBehaviour
{
    static DesktopRootReferenceManager _instance;
    public static DesktopRootReferenceManager getInstance() => DesktopRootReferenceManager._instance;

    // For general use or singleton collection.
    public ContextualMenuManager contextualMenuManager;
    public FileSpriteByType[] typeFileIconList;

    void Awake() => _instance = this;
}
