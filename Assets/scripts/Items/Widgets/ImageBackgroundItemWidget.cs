using System;
using System.Collections;
using UnityEngine;

public class ImageBackgroundItemWidget: DesktopItem
{
    const float defaultScaleWidthBackground = 0.08903134f;

    string _imagePath;

    public string ImagePath {
        get { return _imagePath; }
        set {
            if (spriteFile == null)
                spriteFile = GetComponent<SpriteRenderer>();
            _imagePath = value;
            Sprite sprite = SpriteLoaderUtility.LoadSprite(_imagePath);
            
            if (sprite)
            {
                spriteFile.sprite = sprite;
                setNewScale(defaultScaleWidthBackground);
                AutoScaleColliderToSize();
            }
        }
    }

    protected override void thingsToDoAfterStart()
    {
        if (spriteFile == null)
            ImagePath = _imagePath;
    }

    public void setNewScale(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
    }

    protected override void doInLeftClick() {}
    protected override void doInMiddleClick() {}
}

