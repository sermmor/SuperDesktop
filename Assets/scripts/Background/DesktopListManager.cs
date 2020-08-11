using System.Collections.Generic;
using UnityEngine;

public class DesktopListManager : MonoBehaviour
{
    public GameObject desktopPrefab;

    int totalOfColumns = 2;
    int currentIndexDesktop = 0;

    float desktopWidth;
    float desktopHeight;

    List<DesktopManager> desktopList = new List<DesktopManager>();

    int[][] desktopMapIndex = null;

    public DesktopManager CurrentDesktopShowed { get => desktopList[currentIndexDesktop]; }
    public int NumberOfDesktop {
        get => desktopList.Count;
        set {
            if (value > desktopList.Count) {
                // Create the last (desktopList.Count - value) items.
                for (int i = desktopList.Count; i < value; i++)
                {
                    GameObject generated = GameObject.Instantiate<GameObject>(desktopPrefab);
                    generated.name = getNameDesktop(i);
                    generated.transform.position = getPositionDesktop(i);
                    generated.transform.parent = transform;
                    generated.SetActive(false);
                    desktopList.Add(generated.GetComponent<DesktopManager>());
                }
                refleshIndexMap();
            } else if (value < desktopList.Count) {
                // Delete the last (desktopList.Count - value) items.
                if (currentIndexDesktop >= value) changeInmediatedlyToDesktop(value - 1);
                for (int i = desktopList.Count - 1; i >= value; i--)
                {
                    DesktopManager desktop = desktopList[i];
                    desktopList.Remove(desktop);
                    desktop.DestroyMe();
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
                refleshAllPositionDesktop();
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
        refleshAllPositionDesktop();
    }

    void refleshAllPositionDesktop()
    {
        for (int i = 0; i < desktopList.Count; i++)
        {
            desktopList[i].transform.position = getPositionDesktop(i);
            desktopList[i].gameObject.SetActive(currentIndexDesktop == i);
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

        // drawMap(); // ! DEBUG, DELETE THIS LINE
    }

    // void drawMap() // ! DEBUG, DELETE THIS METHOD
    // {
    //     for (int i = 0; i < desktopMapIndex.Length; i++)
    //         for (int j = 0; j < desktopMapIndex[i].Length; j++)
    //             Debug.Log($"POSITION row = {i}, col = {j} => value = {desktopMapIndex[i][j]}");
    // }

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

        Vector3 desktopPosition = desktopList[desktopIndex].transform.position;
        Camera.main.gameObject.transform.position = new Vector3(desktopPosition.x, desktopPosition.y, Camera.main.transform.position.z);

        desktopList[desktopIndex].gameObject.SetActive(true);
        desktopList[currentIndexDesktop].gameObject.SetActive(false);
        currentIndexDesktop = desktopIndex;
    }

    void calculateDesktopWidthAndHeight()
    {
        desktopHeight = Camera.main.orthographicSize * 2.0f;
        desktopWidth = desktopHeight / Screen.height * Screen.width;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // TODO: Use a keyboard shortcut to change of desktop, we use the method "changeFromCurrentToDesktop(int desktopIndex)".
            KeyCode lastKeyArrowPushed = getKeyArrowPushed();
            if (lastKeyArrowPushed != KeyCode.None)
            {
                int nextIndex = getIndexNextDesktop(lastKeyArrowPushed);
                if (currentIndexDesktop != nextIndex)
                    changeFromCurrentToDesktop(nextIndex);
            }
        }
    }

    public void changeFromCurrentToDesktop(int desktopIndex)
    {
        Vector3 desktopPosition = desktopList[desktopIndex].transform.position;
        Camera.main.gameObject.transform.position = new Vector3(desktopPosition.x, desktopPosition.y, Camera.main.transform.position.z);

        desktopList[desktopIndex].gameObject.SetActive(true);
        // TODO: Maximize the sprite of currentIndexDesktop and desktopIndex while movement and when finished change both to full mode.
        
        // TODO: Do the effect of movement from currentIndexDesktop to desktopIndex, and then put both in full mode (quit maximize) and 
        // TODO: do the following lines.

        desktopList[currentIndexDesktop].gameObject.SetActive(false);
        currentIndexDesktop = desktopIndex;
    }

    public int getIndexNextDesktop(KeyCode keyPushed)
    {
        int[] currentRowAndColumn = getRowAndColumn(currentIndexDesktop);

        if (keyPushed == KeyCode.UpArrow)
            currentRowAndColumn[0]++;
        else if (keyPushed == KeyCode.DownArrow)
            currentRowAndColumn[0]--;
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
