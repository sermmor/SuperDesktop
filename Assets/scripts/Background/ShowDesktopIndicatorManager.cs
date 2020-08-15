using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDesktopIndicatorManager : MonoBehaviour
{
    static bool markToReflesh = false;
    static int[][] desktopMapIndexTempReference = null;

    public static void MarkToRefleshWhenCreated(int[][] desktopMapIndex)
    {
        markToReflesh = true;
        desktopMapIndexTempReference = desktopMapIndex;
    }

    static void ApplyMarkToRefleshWhenCreated(ShowDesktopIndicatorManager obj)
    {
        if (markToReflesh)
        {
            obj.reflesh(ShowDesktopIndicatorManager.desktopMapIndexTempReference);
            markToReflesh = false;
            desktopMapIndexTempReference = null;
        }
    }

    public GameObject desktopEnabledIndicator;
    public GameObject desktopDisabledIndicator;

    public Sprite spriteIndicatorEnabled;
    public Sprite spriteIndicatorDisabled;
    public Color colorIndicatorEnabled;
    public Color colorIndicatorDisabled;

    DesktopListManager desktopListManager;

    int desktopIndexSelected = 0;

    List<GameObject> indicatorsList = new List<GameObject>();
    
    void OnEnable()
    {
        if (indicatorsList == null) indicatorsList = new List<GameObject>();
        if (desktopListManager == null) desktopListManager = DesktopRootReferenceManager.getInstance().desktopListManager;
        if (desktopListManager.DesktopListIndicator == null) desktopListManager.DesktopListIndicator = this;
        
        positionForReflesh = new Vector3(0, 0, 0);
        ApplyMarkToRefleshWhenCreated(this);
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void isEnableIndicator(bool isEnabled, int desktopIndex)
    {
        gameObject.SetActive(isEnabled);
        
        if (isEnabled)
        {
            setIndicatorEnabled(indicatorsList[desktopIndexSelected], false);
            setIndicatorEnabled(indicatorsList[desktopIndex], true);

            desktopIndexSelected = desktopIndex;
        }
    }

    void setIndicatorEnabled(GameObject indicator, bool isEnabled)
    {
        if (isEnabled)
        {
            indicator.GetComponent<SpriteRenderer>().sprite = spriteIndicatorEnabled;
            indicator.GetComponent<SpriteRenderer>().color = colorIndicatorEnabled;
        }
        else
        {
            indicator.GetComponent<SpriteRenderer>().sprite = spriteIndicatorDisabled;
            indicator.GetComponent<SpriteRenderer>().color = colorIndicatorDisabled;
        }
    }

    const float marginIndicatorLeft = .375f;
    const float marginIndicatorRight = .375f;
    const float marginIndicatorUp = .375f;
    const float marginIndicatorDown = .375f;
    Vector3 positionForReflesh = new Vector3(0, 0, 0);
    public void reflesh(int[][] desktopMapIndex)
    {
        clearAllIndicators();
        int[] row;
        GameObject generated;

        float rowMarginAcc = 0;
        float colMarginAcc = 0;
        
        // TODO Create all the visual the structure using desktopMapIndex, desktopEnabledIndicator and desktopDisabledIndicator.
        for (int i = 0; i < desktopMapIndex.Length; i++)
        {
            colMarginAcc = 0;
            row = desktopMapIndex[i];

            for (int j = 0; j < row.Length; j++)
            {
                generated = (desktopMapIndex[i][j] != desktopIndexSelected)
                    ? GameObject.Instantiate<GameObject>(desktopDisabledIndicator)
                    : GameObject.Instantiate<GameObject>(desktopEnabledIndicator);
                generated.name = desktopMapIndex[i][j].ToString();
                generated.transform.SetParent(transform);

                positionForReflesh.x = generated.transform.position.x + colMarginAcc;
                positionForReflesh.y = generated.transform.position.y - rowMarginAcc;
                positionForReflesh.z = generated.transform.position.z;
                generated.transform.position = positionForReflesh;

                indicatorsList.Add(generated);
                colMarginAcc += (marginIndicatorLeft + marginIndicatorRight);
            }
            rowMarginAcc += (marginIndicatorDown + marginIndicatorUp);
        }
    }

    void clearAllIndicators()
    {
        if (indicatorsList.Count > 0)
        {
            for (int i = indicatorsList.Count - 1; i >= 0; i--)
                Destroy(indicatorsList[i]);
            indicatorsList.Clear();
        }
    }
}
