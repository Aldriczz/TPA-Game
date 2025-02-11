using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeableController : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO PlayerStats;
    [SerializeField] private UpgradeableSO UpgradeableInfo;
    [SerializeField] private Text ZenCurrency;
    [SerializeField] private Text AlertCurrencyText;
    [SerializeField] private Transform Upgradeable;
    [SerializeField] private Transform Description;
    
    [Header("Event Channel")]
    public IntEventChannel UpgradeHealthEventChannel;
    public IntEventChannel UpgradeAttackEventChannel;
    public IntEventChannel UpgradeDefenseEventChannel;
    public IntEventChannel UpgradeLuckEventChannel;
    public IntEventChannel UpgradeCritDmgEventChannel;
    public IntEventChannel ZenUpdateEventChannel;
    void Start()
    {
        ZenCurrency = ZenCurrency.GetComponent<Text>();
    }

    private void OnEnable()
    {
        UpgradeHealthEventChannel.OnRaiseIntEvent += UpgradeHealth;
        UpgradeAttackEventChannel.OnRaiseIntEvent += UpgradeAttack;
        UpgradeDefenseEventChannel.OnRaiseIntEvent += UpgradeDefense;
        UpgradeLuckEventChannel.OnRaiseIntEvent += UpgradeLuck;
        UpgradeCritDmgEventChannel.OnRaiseIntEvent += UpgradeCritDmg;
        ZenUpdateEventChannel.OnRaiseIntEvent += ZenUpdatePlus;
    }

    private void OnDisable()
    {
        UpgradeHealthEventChannel.OnRaiseIntEvent -= UpgradeHealth;
        UpgradeAttackEventChannel.OnRaiseIntEvent -= UpgradeAttack;
        UpgradeDefenseEventChannel.OnRaiseIntEvent -= UpgradeDefense;
        UpgradeLuckEventChannel.OnRaiseIntEvent -= UpgradeLuck;
        UpgradeCritDmgEventChannel.OnRaiseIntEvent -= UpgradeCritDmg;
        ZenUpdateEventChannel.OnRaiseIntEvent -= ZenUpdatePlus;
    }

    private void UpgradeHealth(int health)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.HealthUPCost)
        {
            AlertCurrencyText.text = "Don't have enough zen to upgrade!";
            return;
        }
        if (UpgradeableInfo.HealthUP >= 45)
        {
            AlertCurrencyText.text = "Max Level";
            return;
        }
        AlertCurrencyText.text = "";
        
        PlayerStats.MaxHealth += health;
        PlayerStats.CurrentHealth = PlayerStats.MaxHealth;
        
        ZenUpdateMinus(UpgradeableInfo.HealthUPCost);
        
        UpgradeableInfo.HealthUpgrade();
        Upgradeable.Find("Health Upgrade/Health Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.HealthUP} / 45";
        Description.Find("Health Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.MaxHealth} HP";
        Description.Find("Health Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.HealthUPCost} To Upgrade";
        AudioManager.Instance.PlayUpgradeSound();

    }
    
    private void UpgradeAttack(int attack)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.AttackUPCost)
        {
            AlertCurrencyText.text = "Don't have enough zen to upgrade!";
            return;
        }
        if (UpgradeableInfo.AttackUP >= 45)
        {
            AlertCurrencyText.text = "Max Level";
            return;
        }
        AlertCurrencyText.text = "";
        
        PlayerStats.Damage += attack;
        
        ZenUpdateMinus(UpgradeableInfo.AttackUPCost);
        
        UpgradeableInfo.AttackUpgrade();
        Upgradeable.Find("Attack Upgrade/Attack Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.AttackUP} / 45";
        Description.Find("Attack Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.Damage} ATK";
        Description.Find("Attack Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.AttackUPCost} To Upgrade";
        AudioManager.Instance.PlayUpgradeSound();

    }
    
    private void UpgradeDefense(int defense)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.DefenseUPCost)
        {
            AlertCurrencyText.text = "Don't have enough zen to upgrade!";
            return;
        }
        if (UpgradeableInfo.DefenseUP >= 45)
        {
            AlertCurrencyText.text = "Max Level";
            return;
        }
        AlertCurrencyText.text = "";
        PlayerStats.Defense += defense;
        
        ZenUpdateMinus(UpgradeableInfo.DefenseUPCost);
        
        UpgradeableInfo.DefenseUpgrade();
        Upgradeable.Find("Defense Upgrade/Defense Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.DefenseUP} / 45";
        Description.Find("Defense Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.Defense} DEF";
        Description.Find("Defense Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.DefenseUPCost} To Upgrade";
        AudioManager.Instance.PlayUpgradeSound();
    }
    
    private void UpgradeLuck(int luck)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.LuckUPCost)
        {
            AlertCurrencyText.text = "Don't have enough zen to upgrade!";
            return;
        }
        if (UpgradeableInfo.LuckUP >= 45)
        {
            AlertCurrencyText.text = "Max Level";
            return;
        }
        AlertCurrencyText.text = "";
        
        PlayerStats.CritChance += luck;
        
        ZenUpdateMinus(UpgradeableInfo.LuckUPCost);
        
        UpgradeableInfo.LuckUpgrade();
        Upgradeable.Find("Luck Upgrade/Luck Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.LuckUP} / 45";
        Description.Find("Luck Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.CritChance}% Rate";
        Description.Find("Luck Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.LuckUPCost} To Upgrade";
        AudioManager.Instance.PlayUpgradeSound();
    }
    
    private void UpgradeCritDmg(int critDmg)
    {
        if (PlayerStats.Zen <= UpgradeableInfo.CritDmgUPCost)
        {
            AlertCurrencyText.text = "Don't have enough zen to upgrade!";
            return;
        }
        if (UpgradeableInfo.CritDmgUp >= 45)
        {
            AlertCurrencyText.text = "Max Level";
            return;
        }
        AlertCurrencyText.text = "";
        
        PlayerStats.CritDamage += critDmg;
        
        ZenUpdateMinus(UpgradeableInfo.CritDmgUPCost);
        
        UpgradeableInfo.CritDmgUpgrade();
        Upgradeable.Find("Crit Dmg Upgrade/Crit Dmg Lvl").GetComponent<Text>().text = $"{UpgradeableInfo.CritDmgUp} / 45";
        Description.Find("Crit Dmg Desc/Current Stat Text").GetComponent<Text>().text = $"Current: {PlayerStats.CritDamage}% Damage";
        Description.Find("Crit Dmg Desc/Cost Upgrade").GetComponent<Text>().text = $"{UpgradeableInfo.CritDmgUPCost} To Upgrade";
        AudioManager.Instance.PlayUpgradeSound();
    }

    private void ZenUpdatePlus(int amount)
    {
        PlayerStats.Zen += amount;
        ZenCurrency.text = PlayerStats.Zen.ToString();
    }

    private void ZenUpdateMinus(int amount)
    {
        PlayerStats.Zen -= amount;
        ZenCurrency.text = PlayerStats.Zen.ToString();
    }
    
}
