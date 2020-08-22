using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelCtrl : MonoBehaviour
{
    const string newDesktopNameCode = "NEW DESKTOP";

    public GameObject creditsPanel;

    Dropdown desktopListDropdown;
    InputField inputNameNewDesktop;
    InputField inputNameNewDesktopDisabled;

    Button loadButton;
    Button deleteButton;

    string[] dropdownOptions;

    string[] allDesktopPaths;

    bool IsNewDesktopOptionSelected { get => newDesktopNameCode.Equals(dropdownOptions[desktopListDropdown.value]); }

    void Awake()
    {
        desktopListDropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        inputNameNewDesktop = transform.Find("inputNameNewDesktop").GetComponent<InputField>();
        inputNameNewDesktopDisabled = transform.Find("inputNameNewDesktopDisabled").GetComponent<InputField>();
        loadButton = transform.Find("BtAcept").GetComponent<Button>();
        deleteButton = transform.Find("BtDelete").GetComponent<Button>();

        allDesktopPaths = (from path in LoadDesktops.GetAllFilesPathToLoad() where path.Contains(".json") select path).ToArray();
        List<Dropdown.OptionData> optionsNameList = (from path in allDesktopPaths select new Dropdown.OptionData(getNameByPath(path))).ToList();

        if (optionsNameList.Count > 0)
        {
            desktopListDropdown.AddOptions(optionsNameList);
            desktopListDropdown.RefreshShownValue();
        }

        dropdownOptions = (from option in desktopListDropdown.options select option.text).ToArray();

        deleteButton.interactable = false;
    }

    char separator = '.';
    private string getNameByPath(string path)
    {
        string[] tempSplitPath;
        if (separator == '.')
        {
            separator = '\\';
            tempSplitPath = path.Split(separator);
            if (tempSplitPath.Length <= 1)
            {
                separator = '/';
                tempSplitPath = path.Split(separator);
            }
        } else {
            tempSplitPath = path.Split(separator);
        }

        return tempSplitPath[tempSplitPath.Length - 1].Replace(".json", "");
    }

    bool isInputNewDesktopEnabled = true;
    public void CheckIfNewDesktopSelected()
    {
        isInputNewDesktopEnabled = !IsNewDesktopOptionSelected;

        inputNameNewDesktop.readOnly = isInputNewDesktopEnabled;
        inputNameNewDesktop.gameObject.SetActive(!isInputNewDesktopEnabled);
        inputNameNewDesktopDisabled.gameObject.SetActive(isInputNewDesktopEnabled);

        inputNameNewDesktopDisabled.text = inputNameNewDesktop.text;

        deleteButton.interactable = isInputNewDesktopEnabled;
    }

    public void CheckIfSelectedExists()
    {
        if (!IsNewDesktopOptionSelected)
        {
            if (File.Exists(allDesktopPaths[desktopListDropdown.value - 1]))
            {
                deleteButton.interactable = true;
                loadButton.interactable = true;
            }
            else
            {
                deleteButton.interactable = false;
                loadButton.interactable = false;
            }

        }
    }

    public void OpenSuperDesktopSelected()
    {
        if (!IsNewDesktopOptionSelected)
        {
            DesktopRootReferenceManager.getInstance().alertLoading.SetActive(true);
            DesktopRootReferenceManager.getInstance().autoSaver.blockAllSaves(true);
            
            string jsonData = LoadDesktops.LoadData(allDesktopPaths[desktopListDropdown.value - 1]);
            JSONMapperDesktopListManager.parseJSONToDesktopListManager(jsonData);

            DesktopRootReferenceManager.getInstance().autoSaver.nameDesktop = dropdownOptions[desktopListDropdown.value];
            DesktopRootReferenceManager.getInstance().autoSaver.blockAllSaves(false);
            DesktopRootReferenceManager.getInstance().alertLoading.SetActive(false);
        }
        else if (inputNameNewDesktop.text != null || "".Equals(inputNameNewDesktop.text))
        {
            DesktopRootReferenceManager.getInstance().autoSaver.nameDesktop = inputNameNewDesktop.text;
        }
        Destroy(creditsPanel);
        Destroy(gameObject);
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
    }

    public void deleteSuperDesktopSelected()
    {
        string nameDesktop = dropdownOptions[desktopListDropdown.value];
        if (!newDesktopNameCode.Equals(nameDesktop))
        {
            DesktopRootReferenceManager.getInstance().alertYesOrNotController.showYesOrNoAlert(
                $"DELETE {nameDesktop}?",
                () => {
                    File.Delete(allDesktopPaths[desktopListDropdown.value - 1]);
                    CheckIfSelectedExists();
                },
                () => {}
            );
        }
    }
}
