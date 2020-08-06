using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewFileDialogCtrl : DialogController
{
    public GameObject toInstantiate;

    InputField nameFile;
    InputField path;

    ContextualMenuManager contextualMenuManager;

    void OnEnable()
    {
        if (contextualMenuManager == null)
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        if (nameFile == null)
            nameFile = transform.Find("InputNameFile").gameObject.GetComponent<InputField>();
        if (path == null)
            path = transform.Find("InputPath").gameObject.GetComponent<InputField>();
    }


    Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doAceptDialog()
    {
        GameObject generated = GameObject.Instantiate<GameObject>(toInstantiate);
        // Position
        positionToPlaceNewItem.x = contextualMenuManager.transform.position.x;
        positionToPlaceNewItem.y = contextualMenuManager.transform.position.y;
        positionToPlaceNewItem.z = generated.transform.position.z;
        generated.transform.position = positionToPlaceNewItem;
        // Parent
        generated.transform.SetParent(whoIsCallMe.DesktopManagerCaller.transform);
        // Item Propierties
        FileItem item = generated.GetComponent<FileItem>();
        item.desktopManager = whoIsCallMe.DesktopManagerCaller;
        item.nameFile = nameFile.text;
        item.filePath = path.text;
    }

    protected override void clearFieldsDialog()
    {
        nameFile.text = "";
        path.text = "";
    }
}
