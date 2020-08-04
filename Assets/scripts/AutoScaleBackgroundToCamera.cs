using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AutoScaleMode { MAXIMIZE, FULL};

public class AutoScaleBackgroundToCamera : MonoBehaviour
{
    public AutoScaleMode mode;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;
        
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
        Vector2 newScale = getScaleForSpriteAutoScale(sr);
        putMode(newScale);
    }

    Vector2 getScaleForSpriteAutoScale(SpriteRenderer sr) {
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        return new Vector2(worldScreenWidth / width, worldScreenHeight / height);
    }

    void putMode(Vector2 newScale) {
        switch(mode) {
            case AutoScaleMode.MAXIMIZE:
                maximize(newScale);
            break;
            case AutoScaleMode.FULL:
                full(newScale);
            break;
        }
    }

    void maximize(Vector2 newScale) {
        transform.localScale = newScale;
    }

    void full(Vector2 newScale) {
        float maxScale = Mathf.Max(newScale.x, newScale.y);
        transform.localScale = new Vector3(maxScale, maxScale, transform.localScale.z);
    }
}
