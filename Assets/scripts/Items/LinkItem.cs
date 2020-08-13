using UnityEngine;

public class LinkItem : DesktopItem
{
    float defaultWidthFilePreviewIcon = 60;

    public string urlPath;

    string iconPath;
    public string IconPath {
        get => iconPath;
        set {
            iconPath = value;
            if (iconPath != null && !"".Equals(iconPath)) setIconChoosedByUser();
        }
    }

    protected override void thingsToDoAfterStart() {
        // Check Icon.
        if (iconPath != null && !"".Equals(iconPath)) setIconChoosedByUser();
    }

    private void setIconChoosedByUser() => checkAndSetIconImageFile(iconPath);

    void checkAndSetIconImageFile(string imageIconPath)
    {
        string extensionFile = TypeFileUtilities.getExtensionFile(imageIconPath);

        if (TypeFileUtilities.IsTypeImage(extensionFile) && !TypeFileUtilities.IsRareTypeImage(extensionFile))
        {
            try
            {
                spriteFile.sprite = SpriteLoaderUtility.LoadSprite(imageIconPath);
                setSpriteFileIconToDefaultWidthSize();
            }
            catch (System.Exception ex) { }
        }
    }
    
    Vector3 scaleCalculatedForAutoScaleIcon = new Vector3(0, 0, 0);
    void setSpriteFileIconToDefaultWidthSize() {
        float width = spriteFile.sprite.textureRect.width;
        float height = spriteFile.sprite.textureRect.height;
        float currentScale = height / width;

        // Get the world size.
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float scale = (defaultWidthFilePreviewIcon * currentScale) / height;
        
        scaleCalculatedForAutoScaleIcon.x = scaleCalculatedForAutoScaleIcon.y = scale;
        scaleCalculatedForAutoScaleIcon.z = spriteFile.transform.localScale.z;

        spriteFile.transform.localScale = scaleCalculatedForAutoScaleIcon;
    }

    protected override void doInLeftClick()
    {
        Application.OpenURL(urlPath);
    }

    protected override void doInMiddleClick() {}
}
