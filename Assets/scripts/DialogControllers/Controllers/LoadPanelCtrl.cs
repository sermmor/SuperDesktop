using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelCtrl : MonoBehaviour
{
    const string newDesktopNameCode = "NEW DESKTOP";
    Dropdown desktopListDropdown;

    string[] dropdownOptions;

    string[] allDesktopPaths;

    void Awake()
    {
        desktopListDropdown = transform.Find("Dropdown").GetComponent<Dropdown>();

        allDesktopPaths = (from path in LoadDesktops.GetAllFilesPathToLoad() where path.Contains(".json") select path).ToArray();
        List<Dropdown.OptionData> optionsNameList = (from path in allDesktopPaths select new Dropdown.OptionData(getNameByPath(path))).ToList();

        if (optionsNameList.Count > 0)
        {
            desktopListDropdown.AddOptions(optionsNameList);
            desktopListDropdown.RefreshShownValue();
        }

        dropdownOptions = (from option in desktopListDropdown.options select option.text).ToArray();
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

    public void OpenSuperDesktopSelected()
    {
        if (!newDesktopNameCode.Equals(dropdownOptions[desktopListDropdown.value]))
        {
            DesktopRootReferenceManager.getInstance().autoSaver.blockAllSaves(true);
            string jsonData = LoadDesktops.LoadData(allDesktopPaths[desktopListDropdown.value - 1]);
            JSONMapperDesktopListManager.parseJSONToDesktopListManager(jsonData);
            DesktopRootReferenceManager.getInstance().autoSaver.blockAllSaves(false);
        }
        Destroy(gameObject);
        DesktopRootReferenceManager.getInstance().colliderBackgroundForDialogs.SetActive(false);
    }
}
