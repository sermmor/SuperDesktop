using System.Collections;
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

    void Start()
    {
        desktopManager.addItemToDeskop(this);
        menuCaller = new MenuCaller();
        contextualMenu = DesktopRootReferenceManager.getInstance().contextualMenuManager;
        IsMouseAboveDesktopItem = false;
        isDraging = false;
        isDragingInAction = false;
        positionMouseDragging = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        nameFileTextMesh = transform.Find("NameFileText").GetComponent<TextMesh>();
        spriteFile = transform.Find("FileIconSprite").GetComponent<SpriteRenderer>();
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
        nameFileTextMesh.text = nameFile;
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

    void moveGameObjectToMousePosition() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (desktopBounds == null) {
            desktopBounds = desktopManager.Bounds;
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
