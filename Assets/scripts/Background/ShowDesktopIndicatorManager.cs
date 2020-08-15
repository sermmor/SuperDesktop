using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDesktopIndicatorManager : MonoBehaviour
{
    public GameObject desktopEnabledIndicator;
    public GameObject desktopDisabledIndicator;

    DesktopListManager desktopListManager;

    int desktopIndexSelected;
    
    void Start()
    {
        desktopListManager = DesktopRootReferenceManager.getInstance().desktopListManager;
        desktopListManager.DesktopListIndicator = this;
    }

    public void isEnableIndicator(bool isEnabled, int desktopIndex)
    {
        // TODO Enable this structure (gameObject.setActive(true)), put [desktopIndexSelected] to desktopDisableIndicator, and [desktopIndex] to desktopEnableIndicator.
        
        
        // THE NEXT LINE AT THE END OF THE METHOD!!!
        desktopIndexSelected = desktopIndex;
    }

    public void reflesh(int[][] desktopMapIndex)
    {
        // TODO Create all the visual the structure using desktopMapIndex, desktopEnabledIndicator and desktopDisabledIndicator.
    }
}
