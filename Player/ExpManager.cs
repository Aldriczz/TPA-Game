using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance { get; private set; }
    
    [Header("Experience Curve")]
    public AnimationCurve expCurve;
    
    [Header("SO")]
    public PlayerStatsSO PlayerStats;
    public IntEventChannel SetupExpBarEventChannel;
    public IntEventChannel UpdateExpBarEventChannel;
    public VoidEventChannel LevelUpdateEventChannel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) { Instance = this; } else Destroy(this);
        
        PlayerStats.MaxExperience = (int)expCurve.Evaluate(PlayerStats.Level);
        SetupExpBarEventChannel.RaiseIntEvent(PlayerStats.MaxExperience);
        UpdateExpBarEventChannel.RaiseIntEvent(PlayerStats.CurrentExperience);
    }

    private void OnEnable()
    {
        AddExp(0);
    }

    public void AddExp(int exp)
    {
        PlayerStats.CurrentExperience += exp;
        while (PlayerStats.CurrentExperience >= PlayerStats.MaxExperience)
        {
            LevelUp();
        }
        UpdateExpBarEventChannel.RaiseIntEvent(PlayerStats.CurrentExperience);
    }


    private void LevelUp()
    {
        PlayerStats.Level++;
        PlayerStats.CurrentExperience = PlayerStats.CurrentExperience - PlayerStats.MaxExperience;
        PlayerStats.MaxExperience = (int)expCurve.Evaluate(PlayerStats.Level);
        SetupExpBarEventChannel.RaiseIntEvent(PlayerStats.MaxExperience);
        LevelUpdateEventChannel.RaiseVoidEvent();
    }
}
