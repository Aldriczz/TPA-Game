using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeableController : MonoBehaviour
{
    public PlayerStatsSO PlayerStats;
    public UpgradeableSO UpgradeableInfo;
    public Transform ZenCurrency;
    
    [Header("Event Channel")]
    public IntEventChannel UpgradeHealthEventChannel;
    public IntEventChannel UpgradeAttackEventChannel;
    public IntEventChannel UpgradeDefenseEventChannel;
    public IntEventChannel UpgradeLuckEventChannel;
    public IntEventChannel UpgradeCritDmgEventChannel;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        UpgradeHealthEventChannel.OnRaiseIntEvent += UpgradeHealth;
        UpgradeAttackEventChannel.OnRaiseIntEvent += UpgradeAttack;
        UpgradeDefenseEventChannel.OnRaiseIntEvent += UpgradeDefense;
        UpgradeLuckEventChannel.OnRaiseIntEvent += UpgradeLuck;
        UpgradeCritDmgEventChannel.OnRaiseIntEvent += UpgradeCritDmg;
    }

    private void OnDisable()
    {
        UpgradeHealthEventChannel.OnRaiseIntEvent -= UpgradeHealth;
        UpgradeAttackEventChannel.OnRaiseIntEvent -= UpgradeAttack;
        UpgradeDefenseEventChannel.OnRaiseIntEvent -= UpgradeDefense;
        UpgradeLuckEventChannel.OnRaiseIntEvent -= UpgradeLuck;
        UpgradeCritDmgEventChannel.OnRaiseIntEvent -= UpgradeCritDmg;
    }

    private void UpgradeHealth(int health)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.HealthUPCost || UpgradeableInfo.HealthUP >= 45)
        {
            return;
        }
        PlayerStats.MaxHealth += health;
        PlayerStats.CurrentHealth = PlayerStats.MaxHealth;
        
        PlayerStats.Zen -= UpgradeableInfo.HealthUPCost;
        UpgradeableInfo.HealthUpgrade();
        ZenCurrency.GetComponent<Text>().text = PlayerStats.Zen.ToString();
        transform.Find("Upgradeable/Health Upgrade/Health Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.HealthUP} / 45";
        transform.Find("Description/Health Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.MaxHealth} HP";
        transform.Find("Description/Health Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.HealthUPCost} To Upgrade";
    }
    
    private void UpgradeAttack(int attack)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.AttackUPCost || UpgradeableInfo.AttackUP >= 45)
        {
            return;
        }
        PlayerStats.Damage += attack;
        
        PlayerStats.Zen -= UpgradeableInfo.AttackUPCost;
        UpgradeableInfo.AttackUpgrade();
        ZenCurrency.GetComponent<Text>().text = PlayerStats.Zen.ToString();
        transform.Find("Upgradeable/Attack Upgrade/Attack Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.AttackUP} / 45";
        transform.Find("Description/Attack Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.Damage} ATK";
        transform.Find("Description/Attack Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.AttackUPCost} To Upgrade";
    }
    
    private void UpgradeDefense(int defense)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.DefenseUPCost || UpgradeableInfo.DefenseUP >= 45)
        {
            return;
        }
        PlayerStats.Defense += defense;
        
        PlayerStats.Zen -= UpgradeableInfo.DefenseUPCost;
        UpgradeableInfo.DefenseUpgrade();
        ZenCurrency.GetComponent<Text>().text = PlayerStats.Zen.ToString();
        transform.Find("Upgradeable/Defense Upgrade/Defense Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.DefenseUP} / 45";
        transform.Find("Description/Defense Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.Defense} DEF";
        transform.Find("Description/Defense Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.DefenseUPCost} To Upgrade";
    }
    
    private void UpgradeLuck(int luck)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.LuckUPCost || UpgradeableInfo.LuckUP >= 45)
        {
            return;
        }
        PlayerStats.CritChance += luck;
        
        PlayerStats.Zen -= UpgradeableInfo.LuckUPCost;
        UpgradeableInfo.LuckUpgrade();
        ZenCurrency.GetComponent<Text>().text = PlayerStats.Zen.ToString();
        transform.Find("Upgradeable/Luck Upgrade/Luck Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.LuckUP} / 45";
        transform.Find("Description/Luck Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.CritChance}% Rate";
        transform.Find("Description/Luck Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.LuckUPCost} To Upgrade";
    }
    
    private void UpgradeCritDmg(int critDmg)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.CritDmgUPCost || UpgradeableInfo.CritDmgUp >= 45)
        {
            return;
        }
        PlayerStats.CritDamage += critDmg;
        
        PlayerStats.Zen -= UpgradeableInfo.CritDmgUPCost;
        UpgradeableInfo.CritDmgUpgrade();
        ZenCurrency.GetComponent<Text>().text = PlayerStats.Zen.ToString();
        transform.Find("Upgradeable/Crit Dmg Upgrade/Crit Dmg Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.CritDmgUp} / 45";
        transform.Find("Description/Crit Dmg Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.CritDamage}% Damage";
        transform.Find("Description/Crit Dmg Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.CritDmgUPCost} To Upgrade";
    }
}
