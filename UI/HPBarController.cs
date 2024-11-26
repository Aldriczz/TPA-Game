using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Slider HPBar;
    public Slider EaseHPBar;
    public IntEventChannel SetupHpBarEvent;
    public IntEventChannel UpdateHpBarEvent;
    
    private void OnEnable()
    {
        SetupHpBarEvent.OnRaiseIntEvent += SetupUI;
        UpdateHpBarEvent.OnRaiseIntEvent += UpdateUI;
    }

    private void OnDisable()
    {
        SetupHpBarEvent.OnRaiseIntEvent -= SetupUI;
        UpdateHpBarEvent.OnRaiseIntEvent -= UpdateUI;
    }

    private void SetupUI(int MaxHp)
    {
        HPBar.maxValue = MaxHp;
        HPBar.value = MaxHp;
        EaseHPBar.maxValue = MaxHp;
        EaseHPBar.value = MaxHp;
    }

    private void UpdateUI(int CurrentHp)
    {
        HPBar.value = CurrentHp;
        
    }

    private void Update()
    {
        if (EaseHPBar.value != HPBar.value)
        {
            EaseHPBar.value = Mathf.Lerp(EaseHPBar.value, HPBar.value, 2f * Time.deltaTime);
        }
    }
}
