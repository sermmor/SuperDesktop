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


    private bool _isSticked;
    public bool isSticked {get => _isSticked; set => _isSticked = value;}

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

    public void stickNow(bool isStick)
    {
        _isSticked = isStick;
        DesktopRootReferenceManager.getInstance().autoSaver.MarkToSave = true;
    }

    protected override void moveGameObjectToMousePosition()
    {
        if (!_isSticked)
            base.moveGameObjectToMousePosition();
    }
    
    protected override void doInLeftClick() {}
    protected override void doInMiddleClick() {}
}
