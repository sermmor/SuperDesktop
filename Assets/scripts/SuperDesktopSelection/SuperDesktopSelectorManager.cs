using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuperDesktopSelectorManager : MonoBehaviour
{
    public Dropdown superDesktopSelector;

    string[] dropdownOptions;

    void Awake()
    {
        dropdownOptions = (from option in superDesktopSelector.options select option.text).ToArray();
    }

    public void OpenSuperDesktopSelected()
    {
        if ("NEW DESKTOP".Equals(dropdownOptions[superDesktopSelector.value]))
        {
            // Create a new SuperDesktop.
            SceneManager.LoadScene(1);
        }
        else
        {
            // TODO: Load and open a SuperDesktop already created.
        }
    }
}
