using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoItem : DesktopItem
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
    
    protected override void doInLeftClick() {}
    protected override void doInMiddleClick() {}
}
