using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Slider HPBar;
    public Slider EaseHPBar;
    public Text HPText;
    public IntEventChannel SetupHpBarEvent;
    public IntEventChannel UpdateHpBarEvent;

    private int MaxHP;
    
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

    private void SetupUI(int _MaxHp)
    {
        HPBar.maxValue = _MaxHp;
        HPBar.value = _MaxHp;
        EaseHPBar.maxValue = _MaxHp;
        EaseHPBar.value = _MaxHp;
        HPText.text = $"{_MaxHp} / {_MaxHp}";
        MaxHP = _MaxHp;
    }

    private void UpdateUI(int CurrentHp)
    {
        HPBar.value = CurrentHp;
        HPText.text = $"{CurrentHp} / {MaxHP}";
    }

    private void Update()
    {
        if (EaseHPBar.value != HPBar.value)
        {
            EaseHPBar.value = Mathf.Lerp(EaseHPBar.value, HPBar.value, 2f * Time.deltaTime);
        }
    }
}
