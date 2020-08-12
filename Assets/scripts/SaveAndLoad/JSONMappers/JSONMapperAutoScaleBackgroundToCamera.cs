
[System.Serializable]
public class JSONMapperAutoScaleBackgroundToCamera
{
    public AutoScaleMode mode;
    public string[] backgroundPathList;

    public float secondsToAutoChangeWallpaper;
    
    public JSONMapperAutoScaleBackgroundToCamera(AutoScaleBackgroundToCamera autoscaler)
    {
        mode = autoscaler.mode;
        backgroundPathList = autoscaler.backgroundPathList;
        secondsToAutoChangeWallpaper = autoscaler.SecondsToAutoChangeWallpaper;
    }
}
