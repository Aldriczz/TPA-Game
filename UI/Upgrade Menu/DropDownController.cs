using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropDownController : MonoBehaviour
{
    private Dropdown dropdown;
    [SerializeField] private PlayerStatsSO PlayerStats;
    [SerializeField] private VoidEventChannel UpdateAllUnlockLevelsEventChannel;

    private void OnEnable()
    {
        UpdateAllUnlockLevelsEventChannel.OnRaiseVoidEvent += UnlockAllLevels;
        UpdateAllUnlockLevelsEventChannel.OnRaiseVoidEvent += UpdateUnlockLevels;
    }
    
    private void OnDisable()
    {
        UpdateAllUnlockLevelsEventChannel.OnRaiseVoidEvent -= UnlockAllLevels;
        UpdateAllUnlockLevelsEventChannel.OnRaiseVoidEvent -= UpdateUnlockLevels;
    }

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        UpdateUnlockLevels();
    }

    private void UpdateUnlockLevels()
    {
        dropdown.options.Clear();
        dropdown.options.Add(new Dropdown.OptionData("Boss"));
        for (int i = 1; i <= PlayerStats.UnlockLevel; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData("Level " + i.ToString()));
        }
    }

    private void UnlockAllLevels()
    {
        PlayerStats.UnlockLevel = 101;
    }

    public void HandleInputChange(int value)
    {
        PlayerStats.CurrentLevel = value;
    }
}
