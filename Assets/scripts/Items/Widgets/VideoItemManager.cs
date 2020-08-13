using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoItemManager : DesktopItem
{
    Vector2 sizeVideo = new Vector2();
    public Action OnVideoLoaded;

    VideoPlayer videoPlayer;

    public string pathVideo;

    protected override void thingsToDoAfterStart()
    {
        AutoScaleColliderToSize();

        sizeVideo = new Vector2();
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = pathVideo;

        StartCoroutine(WaitUntilAutoScaleVideoInProportion());
    }

    IEnumerator WaitUntilAutoScaleVideoInProportion()
    {
        while (videoPlayer.width <= 0)
            yield return null;

        sizeVideo.x = videoPlayer.width;
        sizeVideo.y = videoPlayer.height;

        AutoScaleColliderToSize();

        if (OnVideoLoaded != null)
            OnVideoLoaded();
    }

    public void AutoScaleColliderToSize()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Vector2 newSize = collider.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        collider.size = newSize;
        collider.offset = new Vector2(0, 0);
    }
    
    protected override void doInLeftClick() {}
    protected override void doInMiddleClick() {}
}
