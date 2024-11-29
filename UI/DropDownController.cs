using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropDownController : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown dropdown;
    public PlayerStatsSO PlayerStats;
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.options.Clear();
        dropdown.options.Add(new Dropdown.OptionData("Boss"));
        for (int i = 1; i <= PlayerStats.UnlockLevel; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData("Level " + i.ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleInputChange(int value)
    {
        PlayerStats.CurrentLevel = value;
    }
}
