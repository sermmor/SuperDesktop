using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveToFolderDialogCtr : DialogController
{
    Dropdown folderListDropdown;
    ContextualMenuManager contextualMenuManager;

    protected override void doOnEnable()
    {
        if (isLaunchedForFistTime)
        {
            contextualMenuManager = DesktopRootReferenceManager.getInstance().contextualMenuManager;
            folderListDropdown = transform.Find("DropdownFolder").gameObject.GetComponent<Dropdown>();
        }
        List<Dropdown.OptionData> newOptions = filterNewOptions(
            folderListDropdown.options,
            DesktopRootReferenceManager.getInstance().CurrentDesktopShowed.allFoldersNames
        );
        if (newOptions.Count > 0)
        {
            folderListDropdown.AddOptions(newOptions);
            folderListDropdown.RefreshShownValue();
        }
    }

    List<Dropdown.OptionData> filterNewOptions(List<Dropdown.OptionData> currentOptions, List<string> allOptions)
    {
        Func<string, bool> isInCurrentOptionsList = (string name) =>
            (from option in currentOptions where name.Equals(option.text) select option).ToArray().Length > 0;

        return (from folderName in allOptions
            where !isInCurrentOptionsList(folderName)
            select new Dropdown.OptionData(folderName)).ToList();
    }

    // Vector3 positionToPlaceNewItem = new Vector3();
    protected override void doAceptDialog()
    {
        string nameSelected = folderListDropdown.options[folderListDropdown.value].text;
        FolderItem item = DesktopRootReferenceManager.getInstance().CurrentDesktopShowed.getFolderByName(nameSelected);
        if (folderListDropdown.value == 0)
        {
            DesktopRootReferenceManager.getInstance().CurrentDesktopShowed.RemoveFromFolderAndPutInDesktop(whoIsCallMe.DesktopItemCaller);
            DesktopRootReferenceManager.getInstance().folderController.RemoveFromFolderAndPutInDesktop(whoIsCallMe.DesktopItemCaller);
        }
        else
        {
            item.AddToFolder(whoIsCallMe.DesktopItemCaller.gameObject);
            FolderItem.HideItemFromDesktop(whoIsCallMe.DesktopItemCaller.gameObject);
        }
        
        base.doAceptDialog();
    }
    protected override void clearFieldsDialog() {
        folderListDropdown.value = 0;
    }    
}
