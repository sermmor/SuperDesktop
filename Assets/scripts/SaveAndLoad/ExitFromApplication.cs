using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFromApplication : MonoBehaviour
{
    void Update()
    {
        if (!DesktopRootReferenceManager.getInstance().isADialogOpened && Input.GetKeyUp(KeyCode.Escape))
        {
            DesktopRootReferenceManager.getInstance().alertYesOrNotController.showYesOrNoAlert(
                $"YOU WANT TO EXIT?",
                () => {
                    DesktopRootReferenceManager.getInstance().autoSaver.forceToSave();
                    Application.Quit();
                },
                () => {}
            );
        }
    }
}
