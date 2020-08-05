using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AutoScaleMode { MAXIMIZE, FULL};

public class AutoScaleBackgroundToCamera : MonoBehaviour
{
    public AutoScaleMode mode;
    public Sprite defaultWallpaper;
    public string[] backgroundPathLists;

    Sprite[] sprites;
    Vector3 baseScale;
    SpriteRenderer sr;

    bool isAutoScaleFinished = false;

    public bool IsAutoScaling { get => !isAutoScaleFinished; }

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
        changeImage(0);

        // StartCoroutine(testChangeSprite());  // TODO: ONLY FOR TESTING.
    }

    void fillSpriteList() {
        if (backgroundPathLists.Length > 0) {
            sprites = (
                from s in SpriteLoader.LoadSpriteList(backgroundPathLists)
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

    public void changeImage(int spriteIndex) {
        isAutoScaleFinished = false;
        sr.sprite = sprites[spriteIndex];
        scaleImage();
    }

    // int spriteCurrentIndex = 0; // TODO: ONLY FOR TESTING.
    // IEnumerator testChangeSprite()  // TODO: ONLY FOR TESTING.
    // {
    //     while (true) {
    //         yield return new WaitForSeconds(5f);
    //         spriteCurrentIndex++;
    //         if (spriteCurrentIndex >= sprites.Length)
    //         {
    //             spriteCurrentIndex = 0;
    //         }
    //         changeImage(spriteCurrentIndex);
    //     }
    // }

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
}
