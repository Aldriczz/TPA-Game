using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    public Slider ExpBar;
    public Slider EaseExpBar;
    public Text ExpText;
    public IntEventChannel SetupExpBarEvent;
    public IntEventChannel UpdateExpBarEvent;

    private int MaxExp;
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

    private void SetupUI(int _MaxExp)
    {
        MaxExp = _MaxExp;
        ExpBar.maxValue = _MaxExp;
        EaseExpBar.maxValue = _MaxExp;
        ExpText.text = $"{Player.Instance.stats.CurrentExperience}/{_MaxExp}";
    }

    private void UpdateUI(int CurrentExp)
    {
        ExpBar.value = CurrentExp;
        ExpText.text = $"{Player.Instance.stats.CurrentExperience}/{MaxExp}";
    }

    private void Update()
    {
        if (EaseExpBar.value != ExpBar.value)
        {
            EaseExpBar.value = Mathf.Lerp(EaseExpBar.value, ExpBar.value, 2f * Time.deltaTime);
        }
    }
}