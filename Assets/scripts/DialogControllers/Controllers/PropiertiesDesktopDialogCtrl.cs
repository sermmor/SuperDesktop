using UnityEngine;
using UnityEngine.UI;

public class PropiertiesDesktopDialogCtrl : DialogController
{
    readonly float[] secondsToChangeWallpaper = new float[] { 0, 15 * 60, 30 * 60, 60 * 60 };

    float defaultIconSize;
    string defaultWallpaperPath;
    int defaultIndexMinutesToChangeWallpaper;
    int defaultNColumnsDesktop;
    int defaultNDesktop;

    Slider sliderIconSize;
    InputField inputWallpaperPath;
    Dropdown listMinutesToChangeWallpaper;
    InputField textNColumnsDesktop;
    InputField textNDesktop;

    DesktopManager currentDesktopManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            currentDesktopManager = DesktopRootReferenceManager.getInstance().currentDesktopShowed;
            sliderIconSize = transform.Find("SliderIconSize").GetComponent<Slider>();
            inputWallpaperPath = transform.Find("InputPathWallpaper").GetComponent<InputField>();
            listMinutesToChangeWallpaper = transform.Find("ListTimeAutoWallpaperChange").GetComponent<Dropdown>();
            textNColumnsDesktop = transform.Find("InputNColumDesktop").GetComponent<InputField>();
            textNDesktop = transform.Find("InputNDesktop").GetComponent<InputField>();
        }
    }

    public void onChangeSliderSizeIcons() => currentDesktopManager.changeSizeIcons(sliderIconSize.value);
    public void onChangeWallpaperPath() => currentDesktopManager.changeImagePath(inputWallpaperPath.text);
    public void onChangeAutoChangeWallpaperTime()
    {
        float secondsAutoChangeWallpaper = fromDropdownIndexToSeconds(listMinutesToChangeWallpaper.value);
        currentDesktopManager.setTimeToAutoChangeWallpaper(secondsAutoChangeWallpaper);
    }

    protected override void doAceptDialog()
    {
        // TODO: Change values or conserve the changes (using DesktopManager new functions)!!! If changes are in realtime, force to save values or do nothing.
        // currentDesktopManager.changeImagePath(inputWallpaperPath.text);
        // currentDesktopManager.changeSizeIcons(sliderIconSize.value);

        // float secondsAutoChangeWallpaper = fromDropdownIndexToSeconds(listMinutesToChangeWallpaper.value);
        // currentDesktopManager.setTimeToAutoChangeWallpaper(secondsAutoChangeWallpaper);
    }

    protected override void clearFieldsDialog()
    {
        sliderIconSize.value = defaultIconSize = currentDesktopManager.IconScalePercentage;
        inputWallpaperPath.text = defaultWallpaperPath = currentDesktopManager.WallpaperImagePath;
        
        defaultIndexMinutesToChangeWallpaper = fromSecondsToDropdownIndex(currentDesktopManager.SecondsToChangeWallpaper);
        listMinutesToChangeWallpaper.value = defaultIndexMinutesToChangeWallpaper;
    }

    public override void OnEscapeButton() => CancelDialog();

    public void CancelDialog()
    {
        currentDesktopManager.changeSizeIcons(defaultIconSize);
        currentDesktopManager.changeImagePath(defaultWallpaperPath);

        float secondsAutoChangeWallpaper = fromDropdownIndexToSeconds(defaultIndexMinutesToChangeWallpaper);
        currentDesktopManager.setTimeToAutoChangeWallpaper(secondsAutoChangeWallpaper);

        this.CloseDialog();
    }

    int fromSecondsToDropdownIndex(float seconds)
    {
        if (seconds < secondsToChangeWallpaper[1])
            return 0;
        else if (seconds < secondsToChangeWallpaper[2])
            return 1;
        else if (seconds < secondsToChangeWallpaper[3])
            return 2;
        else
            return 3;
    }

    float fromDropdownIndexToSeconds(int index)
    {
        if (0 < index && index < secondsToChangeWallpaper.Length)
            return secondsToChangeWallpaper[index];
        
        return secondsToChangeWallpaper[0];
    }
}