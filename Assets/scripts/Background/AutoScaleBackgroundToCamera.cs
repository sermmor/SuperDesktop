using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum AutoScaleMode { MAXIMIZE, FULL};

public class AutoScaleBackgroundToCamera : MonoBehaviour
{
    public AutoScaleMode mode;
    public Sprite defaultWallpaper;
    public string[] backgroundPathList;

    Sprite[] sprites;
    Vector3 baseScale;

    SpriteRenderer sr;
    Vector3 lastScaleBackgroundCalculated;

    bool isAutoScaleFinished = false;

    public bool IsAutoScaling { get => !isAutoScaleFinished; }

    Coroutine changeSpriteByTimeCoroutine;

    float secondsToAutoChangeWallpaper = 15 * 60;

    public string WallpaperImagePath { get {
        if (backgroundPathList.Length == 0) return "default";
        if (backgroundPathList.Length == 1) return backgroundPathList[0];

        // Get image directory from path.
        char separator = '\\';
        string[] pathSplited = backgroundPathList[0].Split('\\');
        if (pathSplited.Length == 1) {
            separator = '/';
            pathSplited = backgroundPathList[0].Split('/');
        }

        string[] directoryPathSplite = (from pathPiece in pathSplited
                        where pathPiece != pathSplited[pathSplited.Length - 1]
                        select pathPiece).ToArray();
        
        return directoryPathSplite.Aggregate((acc, next) => (
            acc == null ? next : $"{acc}{separator}{next}"
        ));
    }}

    public float SecondsToAutoChangeWallpaper { get => secondsToAutoChangeWallpaper; }

    void Awake() => isAutoScaleFinished = false;

    void Start()
    {
        fillSpriteList();

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) {
            Debug.LogError("AutoScaleBackgroundToCamera needs a SpriteRenderer to work!");
            return;
        }
        
        baseScale = new Vector3(1, 1, transform.localScale.z);
        changeImageInCollectionByIndex(0);

        if (backgroundPathList.Length > 1)
            changeSpriteByTimeCoroutine = StartCoroutine(changeSpriteByTime());
    }

    public void changeImageList(string[] newBackgroundPathLists)
    {
        if (backgroundPathList.Length > 1)
            StopCoroutine(changeSpriteByTimeCoroutine);
        isAutoScaleFinished = false;

        backgroundPathList = newBackgroundPathLists;
        fillSpriteList();
        baseScale = new Vector3(1, 1, transform.localScale.z);
        changeImageInCollectionByIndex(0);

        if (backgroundPathList.Length > 1)
            changeSpriteByTimeCoroutine = StartCoroutine(changeSpriteByTime());
    }

    public void setTimeToAutoChangeWallpaper(float timeInSeconds)
    {
        if (backgroundPathList.Length > 1)
            StopCoroutine(changeSpriteByTimeCoroutine);

        secondsToAutoChangeWallpaper = timeInSeconds;
        
        if (timeInSeconds > 0)
        {
            if (backgroundPathList.Length > 1)
                changeSpriteByTimeCoroutine = StartCoroutine(changeSpriteByTime());
        }
    }

    void fillSpriteList() {
        if (backgroundPathList.Length > 0) {
            sprites = (
                from s in SpriteLoaderUtility.LoadSpriteList(backgroundPathList)
                where s != null
                select s
            ).ToArray();
            
            if (sprites == null || sprites.Length == 0) {
                sprites = new Sprite[] { defaultWallpaper };
            }
        } else {
            sprites = new Sprite[] { defaultWallpaper };
        }
    }

    public void changeImageInCollectionByIndex(int spriteIndex) {
        if (sprites.Length == 0) sprites = new Sprite[] { defaultWallpaper };
        if (sr == null) 
        {
            sr = GetComponent<SpriteRenderer>();
            baseScale = new Vector3(1, 1, transform.localScale.z);
        }
        isAutoScaleFinished = false;
        sr.sprite = sprites[spriteIndex];
        scaleImage();
    }

    int spriteCurrentIndex = 0;
    IEnumerator changeSpriteByTime()
    {
        while (true) {
            yield return new WaitForSeconds(secondsToAutoChangeWallpaper);
            spriteCurrentIndex++;
            if (spriteCurrentIndex >= sprites.Length)
            {
                spriteCurrentIndex = 0;
            }
            changeImageInCollectionByIndex(spriteCurrentIndex);
        }
    }

    void scaleImage() {
        transform.localScale = baseScale;
        Vector2 newScale = getScaleForSpriteAutoScale(sr);
        putMode(newScale);
        isAutoScaleFinished = true;
    }

    Vector2 scaleCalculatedForAutoScale = new Vector2(0, 0);
    Vector2 getScaleForSpriteAutoScale(SpriteRenderer sr)
    {
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        scaleCalculatedForAutoScale.x = worldScreenWidth / width;
        scaleCalculatedForAutoScale.y = worldScreenHeight / height;
        return scaleCalculatedForAutoScale;
    }

    void putMode(Vector2 newScale)
    {
        lastScaleBackgroundCalculated = newScale;
        switch(mode) {
            case AutoScaleMode.MAXIMIZE:
                maximize(newScale);
            break;
            case AutoScaleMode.FULL:
                full(newScale);
            break;
        }
    }

    Vector3 scaleCalculatedInMode = new Vector3(0, 0, 0);

    void maximize(Vector2 newScale)
    {
        scaleCalculatedInMode.x = newScale.x;
        scaleCalculatedInMode.y = newScale.y;
        scaleCalculatedInMode.z = transform.localScale.z;

        transform.localScale = newScale;
    }

    void full(Vector2 newScale)
    {
        float maxScale = Mathf.Max(newScale.x, newScale.y);

        scaleCalculatedInMode.x = maxScale;
        scaleCalculatedInMode.y = maxScale;
        scaleCalculatedInMode.z = transform.localScale.z;

        transform.localScale = scaleCalculatedInMode;
    }

    public void changeToMode(AutoScaleMode newMode)
    {
        mode = newMode;
        switch(mode) {
            case AutoScaleMode.MAXIMIZE:
                maximize(lastScaleBackgroundCalculated);
            break;
            case AutoScaleMode.FULL:
                full(lastScaleBackgroundCalculated);
            break;
        }
    }
}
