﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopItem : MonoBehaviour
{
    Vector3 positionMouseDragging;
    Vector3 mousePosition;
    bool isDraging, isDragingInAction;
    
    float timeToBeginToDrag = .1f;
    Coroutine dragCoroutine = null;

    public DesktopManager desktopManager;
    float[] desktopBounds;

    public string nameFile;
    protected TextMesh nameFileTextMesh;
    protected SpriteRenderer spriteFile;

    ContextualMenuManager contextualMenu;
    MenuCaller menuCaller;
    bool IsMouseAboveDesktopItem;

    string nameFolderParent;

    public string NameFolderParent { get => nameFolderParent; }

    public float[] bounds { get {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        return new float[]{ bounds.size.x, bounds.size.y };
    } }

    void Start()
    {
        if (desktopManager == null)
            desktopManager = DesktopRootReferenceManager.getInstance().CurrentDesktopShowed;
        desktopManager.addItemToDeskop(this);
        menuCaller = new MenuCaller();
        contextualMenu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        IsMouseAboveDesktopItem = false;
        isDraging = false;
        isDragingInAction = false;
        positionMouseDragging = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
        Transform finded = transform.Find("NameFileText");
        if (finded) nameFileTextMesh = finded.GetComponent<TextMesh>();
        
        finded = transform.Find("FileIconSprite");
        if (finded) spriteFile = finded.GetComponent<SpriteRenderer>();
        
        setFileName(nameFile);
        thingsToDoAfterStart();
    }

    public virtual void setFileName(string nameFile)
    {
        this.nameFile = nameFile;
        if (nameFile.Contains("\\n"))
        {
            nameFile = nameFile.Replace("\\n", "\n");
        }
        if (nameFileTextMesh)
            nameFileTextMesh.text = nameFile;
    }

    public void setFolderParentName(string nameFolder = null) => this.nameFolderParent = nameFolder;

    public void AutoScaleColliderToSize()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 newSize = collider.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        collider.size = newSize;
        collider.offset = new Vector2(0, 0);
    }

    void OnMouseDrag()
    {
        if (!isDraging)
        {
            isDraging = true;
            isDragingInAction = false;
            dragCoroutine = StartCoroutine(draggingAction());
        }
    }

    IEnumerator draggingAction()
    {
        yield return new WaitForSeconds(timeToBeginToDrag);

        isDragingInAction = true;
        while (isDraging) {
            if (contextualMenu.isOpen) {
                contextualMenu.close();
            }
            moveGameObjectToMousePosition();
            yield return null;
        }
        isDragingInAction = false;
    }

    protected virtual void moveGameObjectToMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (desktopBounds == null) {
            desktopBounds = desktopManager.Bounds;
            if (desktopBounds == null) {
                desktopManager.forceToCalculateBounds();
                desktopBounds = desktopManager.Bounds;
            }
        }

        // We move the item only inside the desktop.
        bool isInLeftLimit = desktopBounds[0] < mousePosition.x;
        bool isInRightLimit = mousePosition.x < desktopBounds[1];
        bool isInDownLimit = desktopBounds[2] < mousePosition.y;
        bool isInUpLimit = mousePosition.y < desktopBounds[3];
        
        positionMouseDragging.x = mousePosition.x;
        positionMouseDragging.y = mousePosition.y;

        if (!isInLeftLimit) {
            positionMouseDragging.x = desktopBounds[0];
        } else if (!isInRightLimit) {
            positionMouseDragging.x = desktopBounds[1];
        }

        if (!isInDownLimit) {
            positionMouseDragging.y = desktopBounds[2];
        } else if (!isInUpLimit) {
            positionMouseDragging.y = desktopBounds[3];
        }

        positionMouseDragging.z = transform.position.z;
        transform.position = positionMouseDragging;
    }

    void OnMouseUp()
    {
        if (!isDragingInAction) {
            if (Input.GetMouseButtonUp(0))
            {
                if (contextualMenu.isOpen) contextualMenu.close();
                doInLeftClick();
            }
        }

        if (dragCoroutine != null) {
            StopCoroutine(dragCoroutine);
            dragCoroutine = null;
            DesktopRootReferenceManager.getInstance().autoSaver.MarkToSave = true;
        }

        isDragingInAction = false;
        isDraging = false;
    }

    void Update()
    {
        if (IsMouseAboveDesktopItem && Input.GetMouseButtonUp(1))
        {
            doInRightClick();
        }
        if (IsMouseAboveDesktopItem && Input.GetMouseButtonUp(2))
        {
            if (contextualMenu.isOpen) {
                contextualMenu.close();
            }
            doInMiddleClick();
        }
    }

    protected virtual void thingsToDoAfterStart() {}

    protected virtual void doInLeftClick()
    {
        Debug.Log("Pressed left click.");
    }

    protected virtual void doInRightClick()
    {
        if (contextualMenu != null)
        {
            menuCaller.setCaller(this);
            if (this is FolderItem)
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.FOLDER);
            else if (this is LinkItem)
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.LINK);
            else if (this is VideoItem)
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.VIDEO_WIDGET);
            else if (this is GroupItemWidget)
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.GROUP_ITEM_WIDGET);
            else if (this is ImageBackgroundItemWidget)
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.IMAGE_BACKGROUND_WIDGET);
            else if (this is NoteItemWidget)
            {
                menuCaller.setCaller(DesktopRootReferenceManager.getInstance().CurrentDesktopShowed);
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.DESKTOP);
            }
            else
                contextualMenu.enableInMousePosition(menuCaller, ContextualMenuMode.FILE);
        }
    }

    protected virtual void doInMiddleClick()
    {
        Debug.Log("Pressed middle click.");
    }

    void OnMouseOver()
    {
        if (!IsMouseAboveDesktopItem) {
            IsMouseAboveDesktopItem = true;
            desktopManager.isMouseAboveDesktopItem = true;
        }
    }

    void OnMouseExit()
    {
        IsMouseAboveDesktopItem = false;
        desktopManager.isMouseAboveDesktopItem = false;
    }
}
