using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpriteLoader
{

    public static Sprite[] LoadSpriteList(string[] pathList) => (from path in pathList select LoadSprite(path)).ToArray();

    public static Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;

        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        
        return null;
    }
}
