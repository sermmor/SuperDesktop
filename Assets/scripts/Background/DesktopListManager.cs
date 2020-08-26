using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopListManager : MonoBehaviour
{
    const float durationDesktopMovement = .4f;
    System.Func<float, float> splineEasingHorizontal;
    System.Func<float, float> splineEasingVertical;

    public GameObject desktopPrefab;

    int totalOfColumns = 2;
    int currentIndexDesktop = 0;

    float desktopWidth;
    float desktopHeight;

    List<DesktopManager> desktopList = new List<DesktopManager>();

    int[][] desktopMapIndex = null;

    public ShowDesktopIndicatorManager DesktopListIndicator {get; set;}

    public List<DesktopManager> AllDesktops { get => desktopList; }
    public DesktopManager CurrentDesktopShowed { get => desktopList[currentIndexDesktop]; }
    public int CurrentDesktopShowedIndex { get => currentIndexDesktop; }

    public int NumberOfDesktop {
        get => desktopList.Count;
        set {
            if (value > desktopList.Count) {
                // Create the last (desktopList.Count - value) items.
                DesktopBigPreviewManager desktopBigPreviews = DesktopRootReferenceManager.getInstance().desktopBigPreviews;
                for (int i = desktopList.Count; i < value; i++)
                {
                    GameObject generated = GameObject.Instantiate<GameObject>(desktopPrefab);
                    generated.name = getNameDesktop(i);
                    generated.transform.position = getPositionDesktop(i);
                    generated.transform.parent = transform;
                    generated.SetActive(false);
                    desktopList.Add(generated.GetComponent<DesktopManager>());
                    desktopBigPreviews.createNewPreview(generated.transform.position, i);
                    desktopBigPreviews.enablePreview(i, false);
                }
                refleshIndexMap();
            } else if (value < desktopList.Count) {
                DesktopBigPreviewManager desktopBigPreviews = DesktopRootReferenceManager.getInstance().desktopBigPreviews;
                // Delete the last (desktopList.Count - value) items.
                if (currentIndexDesktop >= value) changeInmediatedlyToDesktop(value - 1);
                for (int i = desktopList.Count - 1; i >= value; i--)
                {
                    DesktopManager desktop = desktopList[i];
                    desktopList.Remove(desktop);
                    desktop.DestroyMe();
                    desktopBigPreviews.deletePreview(i);
                }
                refleshIndexMap();
            }
        }
    }

    public int NumberOfColumns {
        get => totalOfColumns;
        set {
            totalOfColumns = value;
            if (desktopList.Count > value)
                refleshAllPositionDesktopAndPreviews();
        }
    }

    
    void Awake()
    {
        int i = 0;
        Transform tDesktop = transform.Find(getNameDesktop(i));

        while (tDesktop != null)
        {
            desktopList.Add(tDesktop.GetComponent<DesktopManager>());
            i++;
            tDesktop = transform.Find(getNameDesktop(i));
        }

        calculateDesktopWidthAndHeight();
        refleshAllPositionDesktopAndPreviews();
        calculateConstantsAndSplines();
    }
    
    void calculateConstantsAndSplines()
    {
        // Points (time, relativePosition) using (durationDesktopMovement * percentageTime, desktopWidth * percentageWidth)
        splineEasingHorizontal = EasingBuilder.BuildAkimaSpline(new[] {
            new Vector2(0, 0),
            new Vector2(durationDesktopMovement * 0.05f, desktopWidth * 0.02f),
            new Vector2(durationDesktopMovement * 0.5f, desktopWidth * 0.3f),
            new Vector2(durationDesktopMovement * 0.7f, desktopWidth * 0.9f),
            new Vector2(durationDesktopMovement, desktopWidth)
        });

        splineEasingVertical = EasingBuilder.BuildAkimaSpline(new[] {
            new Vector2(0, 0),
            new Vector2(durationDesktopMovement * 0.05f, desktopHeight * 0.02f),
            new Vector2(durationDesktopMovement * 0.5f, desktopHeight * 0.3f),
            new Vector2(durationDesktopMovement * 0.7f, desktopHeight * 0.9f),
            new Vector2(durationDesktopMovement, desktopHeight)
        });
    }

    void refleshAllPositionDesktopAndPreviews()
    {
        DesktopBigPreviewManager desktopBigPreviews = DesktopRootReferenceManager.getInstance().desktopBigPreviews;
        desktopBigPreviews.clearAllPreviews();
        for (int i = 0; i < desktopList.Count; i++)
        {
            desktopList[i].transform.position = getPositionDesktop(i);
            desktopBigPreviews.createNewPreview(desktopList[i].transform.position, i);
            desktopList[i].gameObject.SetActive(currentIndexDesktop == i);
            // desktopBigPreviews.enablePreview(i, currentIndexDesktop != i);
            desktopBigPreviews.enablePreview(i, false);
        }
        refleshIndexMap();
    }

    void refleshIndexMap()
    {
        int[] lenghtOfRowsAndColumns = getRowAndColumn(desktopList.Count - 1);
        lenghtOfRowsAndColumns[0]++;
        lenghtOfRowsAndColumns[1] = totalOfColumns;
        desktopMapIndex = new int[lenghtOfRowsAndColumns[0]][]; // First dimension => ROW, second dimension => COL
        int counter = 0;
        int numberOfColumnsInIRow;
        for (int i = 0; i < lenghtOfRowsAndColumns[0]; i++)
        {
            numberOfColumnsInIRow = desktopList.Count - counter;
            numberOfColumnsInIRow = (numberOfColumnsInIRow < lenghtOfRowsAndColumns[1]) ? numberOfColumnsInIRow : lenghtOfRowsAndColumns[1];
            desktopMapIndex[i] = new int[numberOfColumnsInIRow];
            for (int j = 0; j < desktopMapIndex[i].Length; j++)
            {
                desktopMapIndex[i][j] = counter;
                counter++;
            }
        }

        if (DesktopListIndicator)
            DesktopListIndicator.reflesh(desktopMapIndex);
        else 
            ShowDesktopIndicatorManager.MarkToRefleshWhenCreated(desktopMapIndex);
    }

    int[] getRowAndColumn(int desktopIndex) => (new int[] {
        (int) Mathf.Floor(desktopIndex / totalOfColumns),
        (int) Mathf.Floor(desktopIndex % totalOfColumns)
    });

    string getNameDesktop(int desktopIndex) => $"Desktop_{desktopIndex}";

    Vector3 getPositionDesktop(int desktopIndex)
    {
        if (desktopIndex == 0) return Vector3.zero;

        float myColumn = Mathf.Floor(desktopIndex % totalOfColumns);
        float myRow = Mathf.Floor(desktopIndex / totalOfColumns);
        return new Vector3(
            desktopWidth * myColumn,
            -desktopHeight * myRow,
            0
        );
    }

    public void changeInmediatedlyToDesktop(int desktopIndex)
    {
        DesktopBigPreviewManager desktopBigPreviews = DesktopRootReferenceManager.getInstance().desktopBigPreviews;

        Vector3 desktopPosition = desktopList[desktopIndex].transform.position;
        Camera.main.gameObject.transform.position = new Vector3(desktopPosition.x, desktopPosition.y, Camera.main.transform.position.z);

        desktopList[desktopIndex].gameObject.SetActive(true);
        desktopList[currentIndexDesktop].gameObject.SetActive(false);
        
        desktopBigPreviews.enablePreview(desktopIndex, false);
        desktopBigPreviews.enablePreview(currentIndexDesktop, false);

        currentIndexDesktop = desktopIndex;
    }

    void calculateDesktopWidthAndHeight()
    {
        desktopHeight = Camera.main.orthographicSize * 2.0f;
        desktopWidth = desktopHeight / Screen.height * Screen.width;
    }

    KeyCode lastKeyArrowPushed;
    int nextIndexDesktop;
    void Update()
    {
        if (!DesktopRootReferenceManager.getInstance().isADialogOpened && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            lastKeyArrowPushed = getKeyArrowPushed();
            if (lastKeyArrowPushed != KeyCode.None)
            {
                nextIndexDesktop = getIndexNextDesktop(lastKeyArrowPushed);
                if (currentIndexDesktop != nextIndexDesktop)
                    changeFromCurrentToDesktop(nextIndexDesktop, lastKeyArrowPushed);
            }
        }
    }

    public void changeFromCurrentToDesktop(int desktopIndex, KeyCode keyPushed)
    {
        DesktopRootReferenceManager.getInstance().isADialogOpened = true;
        DesktopBigPreviewManager desktopBigPreviews = DesktopRootReferenceManager.getInstance().desktopBigPreviews;
        
        desktopBigPreviews.enablePreview(desktopIndex, true);
        desktopBigPreviews.enablePreview(currentIndexDesktop, true);

        Vector3 desktopPosition = desktopList[desktopIndex].transform.position;

        DesktopListIndicator.isEnableIndicator(true, desktopIndex);

        StartCoroutine(moveCameraToDirection(keyPushed, desktopList[currentIndexDesktop].transform.position, desktopPosition, () => {
            DesktopListIndicator.isEnableIndicator(false, desktopIndex);
            desktopBigPreviews.enablePreview(currentIndexDesktop, false);
            desktopList[currentIndexDesktop].gameObject.SetActive(false);

            desktopBigPreviews.enablePreview(desktopIndex, false);
            desktopList[desktopIndex].gameObject.SetActive(true);

            Camera.main.gameObject.transform.position = new Vector3(desktopPosition.x, desktopPosition.y, Camera.main.transform.position.z);
            currentIndexDesktop = desktopIndex;
            DesktopRootReferenceManager.getInstance().isADialogOpened = false;
        }));
    }

    // float getValueDirection(Vector2 direction, Vector2 toPlane) => (Mathf.Abs(direction.x) > 0) ? toPlane.x : toPlane.y;
    Vector3 newPositionCameraWithoutNaN = new Vector3();
    void assingToCameraProtectAgainstNaN(Vector3 newCameraPositon)
    {
        newPositionCameraWithoutNaN.x = float.IsNaN(newCameraPositon.x) ? Camera.main.gameObject.transform.position.x : newCameraPositon.x;
        newPositionCameraWithoutNaN.y = float.IsNaN(newCameraPositon.y) ? Camera.main.gameObject.transform.position.y : newCameraPositon.y;
        newPositionCameraWithoutNaN.z = newCameraPositon.z;
        
        Camera.main.gameObject.transform.position = newPositionCameraWithoutNaN;
    }

    IEnumerator moveCameraToDirection(KeyCode keyPushed, Vector3 positionCurrentDesktop, Vector3 positionNextDesktop, Action onCameraMoved)
    {
        Vector2 increment;

        if (keyPushed == KeyCode.UpArrow)
            increment = Vector2.up;
        else if (keyPushed == KeyCode.DownArrow)
            increment = Vector2.down;
        else if (keyPushed == KeyCode.RightArrow)
            increment = Vector2.right;
        else if (keyPushed == KeyCode.LeftArrow)
            increment = Vector2.left;
        else
            increment = Vector2.zero;

        Vector2 initialPosition = new Vector2(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y
        );

        Vector2 finalPosition = new Vector2(
            initialPosition.x + increment.x * desktopWidth,
            initialPosition.y + increment.y * desktopHeight
        );

        Vector3 currentMovement = new Vector3(0, 0, Camera.main.transform.position.z);
        bool isFinishedMovement = false;
        float currentTimeMovement = 0;
        Vector2 splineResult = new Vector2();

        while (!isFinishedMovement && currentTimeMovement < durationDesktopMovement)
        {
            splineResult.x = splineEasingHorizontal(currentTimeMovement);
            splineResult.y = splineEasingVertical(currentTimeMovement);
            currentMovement.x = initialPosition.x + increment.x * splineResult.x;
            currentMovement.y = initialPosition.y + increment.y * splineResult.y;
            assingToCameraProtectAgainstNaN(currentMovement);

            yield return null;
            currentTimeMovement += Time.deltaTime;
        }

        if (onCameraMoved != null)
            onCameraMoved();
    }

    bool isInNewPositionMoveCamera(Vector2 finalPosition, KeyCode keyPushed)
    {
        if (keyPushed == KeyCode.UpArrow)
            return finalPosition.y <= Camera.main.transform.position.y;
        else if (keyPushed == KeyCode.DownArrow)
            return finalPosition.y >= Camera.main.transform.position.y;
        else if (keyPushed == KeyCode.RightArrow)
            return finalPosition.x <= Camera.main.transform.position.x;
        else if (keyPushed == KeyCode.LeftArrow)
            return finalPosition.x >= Camera.main.transform.position.x;
        return true;
    }

    public int getIndexNextDesktop(KeyCode keyPushed)
    {
        int[] currentRowAndColumn = getRowAndColumn(currentIndexDesktop);

        if (keyPushed == KeyCode.UpArrow)
            currentRowAndColumn[0]--;
        else if (keyPushed == KeyCode.DownArrow)
            currentRowAndColumn[0]++;
        else if (keyPushed == KeyCode.RightArrow)
            currentRowAndColumn[1]++;
        else if (keyPushed == KeyCode.LeftArrow)
            currentRowAndColumn[1]--;

        return getIndexByRowAndColumn(currentRowAndColumn[0], currentRowAndColumn[1]);
    }

    int getIndexByRowAndColumn(int row, int column)
    {
        int newRow = row;
        int newColum = column;

        if (newRow > desktopMapIndex.Length - 1)
            newRow = 0;
        else if (newRow < 0)
        {
            newRow = desktopMapIndex.Length - 1;
            if (newColum > desktopMapIndex[newRow].Length - 1 && desktopMapIndex[newRow].Length < totalOfColumns) newRow--;
        }

        if (newColum > desktopMapIndex[newRow].Length - 1)
            newColum = 0;
        else if (newColum < 0)
            newColum = desktopMapIndex[newRow].Length - 1;

        return desktopMapIndex[newRow][newColum];
    }

    KeyCode getKeyArrowPushed()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow)) return KeyCode.UpArrow;
        else if (Input.GetKeyUp(KeyCode.RightArrow)) return KeyCode.RightArrow;
        else if (Input.GetKeyUp(KeyCode.DownArrow)) return KeyCode.DownArrow;
        else if (Input.GetKeyUp(KeyCode.LeftArrow)) return KeyCode.LeftArrow;
        return KeyCode.None;
    }
}
