using UnityEngine;
using UnityEngine.UI;

public class CreateOrModifyNoteWidgetDialogCtrl: DialogController
{
    public GameObject toInstantiate;

    string defaultTextValue;
    bool defaultIsShowingPin;

    InputField inputText;
    Toggle toggleIsShowPin;

    public NoteItemWidget NoteCalledby {get; set; } = null;

    bool IsInDialogCreatorCase { get => NoteCalledby == null; }
    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            inputText = transform.Find("InputText").gameObject.GetComponent<InputField>();
            toggleIsShowPin = transform.Find("ToggleIsShowPin").gameObject.GetComponent<Toggle>();
        }
    }

    public void OnEditNoteText()
    {
        if (!IsInDialogCreatorCase)
            NoteCalledby.NoteText = inputText.text;
    }

    public void OnEditIsShowPin()
    {
        if (!IsInDialogCreatorCase)
            NoteCalledby.IsPinEnabled = toggleIsShowPin.isOn;
    }

    Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doAceptDialog()
    {
        if (IsInDialogCreatorCase)
        {
            GameObject generated = GameObject.Instantiate<GameObject>(toInstantiate);
            // Position
            positionToPlaceNewItem.x = contextualMenuManager.transform.position.x;
            positionToPlaceNewItem.y = contextualMenuManager.transform.position.y;
            positionToPlaceNewItem.z = generated.transform.position.z;
            generated.transform.position = positionToPlaceNewItem;
            // Item Propierties
            NoteItemWidget item = generated.GetComponent<NoteItemWidget>();
            item.desktopManager = whoIsCallMe.DesktopManagerCaller;
            item.NoteText = inputText.text;
            item.IsPinEnabled = toggleIsShowPin.isOn;
        }
        else if ("".Equals(inputText.text)) 
        {
            Destroy(NoteCalledby.gameObject);
        }
        else
            NoteCalledby = null;
        
        base.doAceptDialog();
    }

    public void CancelDialog()
    {
        if (!IsInDialogCreatorCase)
        {
            NoteCalledby.NoteText = defaultTextValue;
            NoteCalledby.IsPinEnabled = defaultIsShowingPin;
            NoteCalledby = null;
        }
        this.CloseDialog();
    }

    protected override void clearFieldsDialog()
    {
        if (IsInDialogCreatorCase)
        {
            inputText.text = "";
            toggleIsShowPin.isOn = true;
        }
        else
        {
            inputText.text = defaultTextValue = NoteCalledby.NoteText;
            toggleIsShowPin.isOn = defaultIsShowingPin = NoteCalledby.IsPinEnabled;
        }
    } 

}
