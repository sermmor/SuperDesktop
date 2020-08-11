using System;
using System.Collections;
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
            } else if (value < desktopList.Count) {
                // Delete the last (desktopList.Count - value) items.
                if (currentIndexDesktop >= value) changeInmediatedlyToDesktop(value - 1);
                for (int i = desktopList.Count - 1; i >= value; i--)
                {
                    DesktopManager desktop = desktopList[i];
                    desktopList.Remove(desktop);
                    desktop.DestroyMe();
                }
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
    }

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
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height / Screen.height * Screen.width;
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

        float[] _bounds = new float[]{
            x - (width / 2), // minX
            (width / 2) + x, // maxX
            y - (height / 2), // minY
            (height / 2) + y // maxY
        };

        desktopWidth = (width / 2) + x - (x - (width / 2));
        desktopHeight = (height / 2) + y - (y - (height / 2));
    }

    public void changeFromCurrentToDesktop(int desktopIndex)
    {
        currentIndexDesktop = desktopIndex;

        Vector3 desktopPosition = desktopList[currentIndexDesktop].transform.position;
        Camera.main.gameObject.transform.position = new Vector3(desktopPosition.x, desktopPosition.y, Camera.main.transform.position.z);

        desktopList[desktopIndex].gameObject.SetActive(true);
        // TODO: Maximize the sprite of currentIndexDesktop and desktopIndex while movement and when finished change both to full mode.
        
        // TODO: Do the effect of movement from currentIndexDesktop to desktopIndex, and then put both in full mode (quit maximize) and 
        // TODO: do the following lines.

        desktopList[currentIndexDesktop].gameObject.SetActive(false);
        currentIndexDesktop = desktopIndex;
    }

    void Update()
    {
        // TODO: Use a keyboard shortcut to change of desktop, we use the method "changeFromCurrentToDesktop(int desktopIndex)".
    }
}
