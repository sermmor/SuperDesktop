using UnityEngine;
using UnityEngine.UI;

public class ChangeScaleVideoDialogCtrl: DialogController
{
    InputField inputWidth;
    InputField inputHeight;
    Dropdown isProporcionalList;

    ContextualMenuManager contextualMenuManager;
    Vector3 defaultScaleVideo = new Vector3();

    Vector3 newScaleVideo = new Vector3();

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            newScaleVideo = new Vector3(
                0,
                0, 
                whoIsCallMe.DesktopItemCaller.transform.localScale.z
            );
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            inputWidth = transform.Find("InputWidth").gameObject.GetComponent<InputField>();
            inputHeight = transform.Find("InputHeight").gameObject.GetComponent<InputField>();
            isProporcionalList = transform.Find("isProporcionalList").gameObject.GetComponent<Dropdown>();
        }
    }

    protected override void clearFieldsDialog()
    {
        defaultScaleVideo = new Vector3(
            whoIsCallMe.DesktopItemCaller.transform.localScale.x,
            whoIsCallMe.DesktopItemCaller.transform.localScale.y,
            whoIsCallMe.DesktopItemCaller.transform.localScale.z
        );
        inputWidth.text = whoIsCallMe.DesktopItemCaller.transform.localScale.x.ToString();
        inputHeight.text = whoIsCallMe.DesktopItemCaller.transform.localScale.y.ToString();
    }

    public void SetNewScalePosition()
    {
        if (isProporcionalList.value == 0) 
        {
            newScaleVideo.x = float.Parse(inputWidth.text);
            newScaleVideo.y = float.Parse(inputHeight.text);
        }
        else if (newScaleVideo.x != float.Parse(inputWidth.text))
        {
            newScaleVideo.y = newScaleVideo.y * newScaleVideo.x / float.Parse(inputWidth.text);
            inputHeight.text = newScaleVideo.y.ToString();
            newScaleVideo.x = float.Parse(inputWidth.text);
        }
        else
        {
            newScaleVideo.x = newScaleVideo.x * newScaleVideo.y / float.Parse(inputHeight.text);
            inputWidth.text = newScaleVideo.x.ToString();
            newScaleVideo.y = float.Parse(inputHeight.text);
        }
        whoIsCallMe.DesktopItemCaller.transform.localScale = newScaleVideo;
        whoIsCallMe.DesktopItemCaller.AutoScaleColliderToSize();
    }

    public void CancelDialog()
    {
        whoIsCallMe.DesktopItemCaller.transform.localScale = defaultScaleVideo;
        whoIsCallMe.DesktopItemCaller.AutoScaleColliderToSize();
        this.CloseDialog();
    }
}