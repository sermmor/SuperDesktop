using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderController : MonoBehaviour
{
    Vector3 positionInitColumn;
    Vector3 positionEndColumn;

    Text folderTitle;
    GameObject allContent;
    FolderItem folderItem;

    public GameObject uiFolderPanel;

    public void showDialog(FolderItem folderItem)
    {
        this.folderItem = folderItem;
        gameObject.SetActive(true);
    }

    public void closeDialog()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
        uiFolderPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(true);
        uiFolderPanel.SetActive(true);
        if (folderTitle == null)
        {
            folderTitle = uiFolderPanel.transform.Find("FolderTitle").GetComponent<Text>();
            positionInitColumn = transform.Find("InitColumns").position;
            positionEndColumn = transform.Find("EndColumns").position;
            allContent = transform.Find("Content").gameObject;
            autoscaleWideScreen(GetComponent<SpriteRenderer>());
        }

        folderTitle.text = folderItem.nameFile;
        folderTitle.text = folderTitle.text.Replace("\\n", "");
        createFileElement();
    }

    void autoscaleWideScreen(SpriteRenderer sr)
    {
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / width,
            worldScreenHeight / height,
            transform.localScale.z
        );
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
        desktopItemCaller.transform.parent = DesktopRootReferenceManager.getInstance().CurrentDesktopShowed.transform;

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
