using UnityEngine;
using UnityEngine.UI;

public class CreateNewFileDialogCtrl : DialogController
{
    public GameObject toInstantiate;

    InputField nameFile;
    InputField path;

    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            nameFile = transform.Find("InputNameFile").gameObject.GetComponent<InputField>();
            path = transform.Find("InputPath").gameObject.GetComponent<InputField>();
        }
    }


    Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doAceptDialog()
    {
        if (path.text.Contains("http://") || path.text.Contains("https://")) return;

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
        item.transform.localScale = new Vector3(
            DesktopRootReferenceManager.getInstance().currentDesktopShowed.IconRealScale,
            DesktopRootReferenceManager.getInstance().currentDesktopShowed.IconRealScale,
            item.transform.localScale.z
        );
    }

    protected override void clearFieldsDialog()
    {
        nameFile.text = "";
        path.text = "";
    }

    public void GetTitleTextByPath()
    {
        if (nameFile.text == null || nameFile.text.Length == 0)
        {
            string nameFileWithoutExtension = "";
            char separator = '\\';
            string[] pathSplited = path.text.Split(separator);
            if (pathSplited.Length == 1) {
                separator = '/';
                pathSplited = path.text.Split(separator);
            }

            if (pathSplited.Length > 1) {
                string[] splitByExtension = pathSplited[pathSplited.Length - 1].Split('.');
                if (splitByExtension.Length == 2)
                {
                    nameFileWithoutExtension = splitByExtension[0];
                }
                else
                {
                    nameFileWithoutExtension = pathSplited[pathSplited.Length - 1];
                }
            }
            nameFile.text = nameFileWithoutExtension;
        }
    }
}
