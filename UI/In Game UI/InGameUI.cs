using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;
    public IntEventChannel ZenUpdateEventChannel;
    public VoidEventChannel EnemyNumberUpdateEventChannel;
    public VoidEventChannel FloorUpdateEventChannel;
    public VoidEventChannel LevelUpdateEventChannel;

    [Header("InGame UI")] 
    public Text ZenCurrency;

    public Text EnemyNumber;
    
    public Text FloorNumber;
    
    public Text LevelNumber;
    private void Start()    
    {
        ZenUpdateEventChannel.RaiseIntEvent(0);
        EnemyNumberUpdateEventChannel.RaiseVoidEvent();
        FloorUpdateEventChannel.RaiseVoidEvent();
        LevelUpdateEventChannel.RaiseVoidEvent();
    }

    private void OnEnable()
    {
        ZenUpdateEventChannel.OnRaiseIntEvent += UpdateZen;
        EnemyNumberUpdateEventChannel.OnRaiseVoidEvent += UpdateEnemyNumber;
        FloorUpdateEventChannel.OnRaiseVoidEvent += UpdateFloorText;
        LevelUpdateEventChannel.OnRaiseVoidEvent += UpdatePlayerLevel;
    }

    private void OnDisable()
    {
        ZenUpdateEventChannel.OnRaiseIntEvent -= UpdateZen;
        EnemyNumberUpdateEventChannel.OnRaiseVoidEvent -= UpdateEnemyNumber;
        FloorUpdateEventChannel.OnRaiseVoidEvent -= UpdateFloorText;
        LevelUpdateEventChannel.OnRaiseVoidEvent -= UpdatePlayerLevel;
    }

    private void UpdateZen(int value)
    {
        PlayerStats.Zen += value;
        ZenCurrency.text = PlayerStats.Zen.ToString();
    }

    private void UpdateEnemyNumber()
    {
        EnemyNumber.text = "Enemy Left: " + EntitySpawnerManager.Instance.enemyList.Count.ToString();
    }

    private void UpdateFloorText()
    {
        FloorNumber.text = "Floor " + PlayerStats.CurrentLevel.ToString();
    }

    private void UpdatePlayerLevel()
    {
        LevelNumber.text = "Level " + PlayerStats.Level.ToString();
    }
}
