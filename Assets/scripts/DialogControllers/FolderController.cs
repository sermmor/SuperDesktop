using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderController : MonoBehaviour
{
    Vector3 positionInitColumn;
    Vector3 positionEndColumn;

    TextMesh folderTitle;
    GameObject allContent;
    FolderItem folderItem;

    public void showDialog(FolderItem folderItem)
    {
        this.folderItem = folderItem;
        gameObject.SetActive(true);
    }

    public void closeDialog()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(true);
        folderTitle = transform.Find("FolderTitle").GetComponent<TextMesh>();
        positionInitColumn = transform.Find("InitColumns").position;
        positionEndColumn = transform.Find("EndColumns").position;
        allContent = transform.Find("Content").gameObject;

        folderTitle.text = folderItem.nameFile;
        folderTitle.text = folderTitle.text.Replace("\\n", "");
        createFileElement();
    }

    void createFileElement()
	{
        GameObject newItem;
        float[] bounds = null;
        float nextPositionX = positionInitColumn.x;
        float nextPositionY = positionInitColumn.y;
		for (int i = 0; i < folderItem.ItemList.Count; i++)
		{
            newItem = folderItem.ItemList[i];
            if (bounds == null) bounds = calculateBounds(newItem);
            
            newItem.transform.position = new Vector3(
                nextPositionX,
                nextPositionY,
                allContent.transform.position.z
            );
            newItem.transform.parent = allContent.transform;
            nextPositionX += bounds[0];
            if (nextPositionX > positionEndColumn.x)
            {
                nextPositionX = positionInitColumn.x;
                nextPositionY -= bounds[1];
            }
		}
	}

    float[] calculateBounds(GameObject item)
    {
        float[] bounds = item.GetComponent<DesktopItem>().bounds;
        float textWidth = bounds[0] / 2f;
        float textHeight = bounds[1] / 2f;
        bounds[0] += textWidth;
        bounds[1] += textHeight;

        return bounds;
    }

    public void RemoveFromFolderAndPutInDesktop(DesktopItem desktopItemCaller) =>
        desktopItemCaller.transform.parent = DesktopRootReferenceManager.getInstance().currentDesktopShowed.transform;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            doBeforeClose();
            closeDialog();
        }
    }

    void doBeforeClose()
    {
        folderItem.HideAllItemFromDesktop();
    }
}
