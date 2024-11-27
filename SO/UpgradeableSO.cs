using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeableSO" ,menuName="SO/UpgradeableSO")]
public class UpgradeableSO : ScriptableObject
{
    public int HealthUP;
    public int AttackUP;
    public int DefenseUP;
    public int LuckUP;
    public int CritDmgUp;

    public int HealthUPCost;
    public int AttackUPCost;
    public int DefenseUPCost;
    public int LuckUPCost;
    public int CritDmgUPCost;


    public void Reset()
    {
        HealthUP = 0;
        AttackUP = 0;
        DefenseUP = 0;
        LuckUP = 0;
        CritDmgUp = 0;
        
        HealthUPCost = 10;
        AttackUPCost = 10;
        DefenseUPCost = 10;
        LuckUPCost = 10;
        CritDmgUPCost = 10;
    }
    
    public void HealthUpgrade()
    {
        HealthUP++;
        HealthUPCost += 50;
        AttackUPCost += 10;
        DefenseUPCost += 10;
        LuckUPCost += 10;
        CritDmgUPCost += 10;
    }
    
    public void AttackUpgrade()
    {
        AttackUP++;
        HealthUPCost += 10;
        AttackUPCost += 50;
        DefenseUPCost += 10;
        LuckUPCost += 10;
        CritDmgUPCost += 10;
    }
    
    public void DefenseUpgrade()
    {
        DefenseUP++;
        HealthUPCost += 10;
        AttackUPCost += 10;
        DefenseUPCost += 50;
        LuckUPCost += 10;
        CritDmgUPCost += 10;
    }
    
    public void LuckUpgrade()
    {
        LuckUP++;
        HealthUPCost += 10;
        AttackUPCost += 10;
        DefenseUPCost += 10;
        LuckUPCost += 50;
        CritDmgUPCost += 10;
    }
    
    public void CritDmgUpgrade()
    {
        CritDmgUp++;
        HealthUPCost += 10;
        AttackUPCost += 10;
        DefenseUPCost += 10;
        LuckUPCost += 10;
        CritDmgUPCost += 50;
    }
}
