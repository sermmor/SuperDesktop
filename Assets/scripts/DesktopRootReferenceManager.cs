using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopRootReferenceManager : MonoBehaviour
{
    static DesktopRootReferenceManager _instance;
    public static DesktopRootReferenceManager getInstance() => DesktopRootReferenceManager._instance;

    public DesktopManager desktopManager;

    void Awake() => _instance = this;
}
