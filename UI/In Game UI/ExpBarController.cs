using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    public Slider ExpBar;
    public Slider EaseExpBar;
    public IntEventChannel SetupExpBarEvent;
    public IntEventChannel UpdateExpBarEvent;
    
    private void OnEnable()
    {
        SetupExpBarEvent.OnRaiseIntEvent += SetupUI;
        UpdateExpBarEvent.OnRaiseIntEvent += UpdateUI;
    }

    private void OnDisable()
    {
        SetupExpBarEvent.OnRaiseIntEvent -= SetupUI;
        UpdateExpBarEvent.OnRaiseIntEvent -= UpdateUI;
    }

    private void SetupUI(int MaxExp)
    {
        ExpBar.maxValue = MaxExp;
        EaseExpBar.maxValue = MaxExp;
    }

    private void UpdateUI(int CurrentExp)
    {
        ExpBar.value = CurrentExp;
    }

    private void Update()
    {
        if (EaseExpBar.value != ExpBar.value)
        {
            EaseExpBar.value = Mathf.Lerp(EaseExpBar.value, ExpBar.value, 2f * Time.deltaTime);
        }
    }
}