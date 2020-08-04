using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopItem : MonoBehaviour
{

    Vector3 positionMouseDragging;
    Vector3 cameraPosition;
    bool isDraging, isDragingInAction;
    
    float timeToBeginToDrag = .1f;
    Coroutine dragCoroutine = null;

    DesktopManager desktopManager;
    float[] desktopBounds;

    void Start()
    {
        isDraging = false;
        isDragingInAction = false;
        positionMouseDragging = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        desktopManager = DesktopRootReferenceManager.getInstance().desktopManager;
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
            moveGameObjectToMousePosition();
            yield return null;
        }
        isDragingInAction = false;
    }

    void moveGameObjectToMousePosition() {
        cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (desktopBounds == null) {
            desktopBounds = desktopManager.Bounds;
        }

        // We move the item only inside the desktop.
        if (
            desktopBounds[0] < cameraPosition.x && cameraPosition.x < desktopBounds[1] &&
            desktopBounds[2] < cameraPosition.y && cameraPosition.y < desktopBounds[3]
        ) {
            positionMouseDragging.x = cameraPosition.x;
            positionMouseDragging.y = cameraPosition.y;
            transform.position = positionMouseDragging;
        }
    }

    void OnMouseUp()
    {
        if (!isDragingInAction) {
            if (Input.GetMouseButtonUp(0))
            {
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
        if (Input.GetMouseButtonUp(1))
        {
            doInRightClick();
        }
        if (Input.GetMouseButtonUp(2))
        {
            doInMiddleClick();
        }
    }

    protected void doInLeftClick()
    {
        Debug.Log("Pressed left click.");
    }

    protected void doInRightClick()
    {
        Debug.Log("Pressed right click.");
    }

    protected void doInMiddleClick()
    {
        Debug.Log("Pressed left click.");
    }
}
