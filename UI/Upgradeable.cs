using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    public UpgradeableSO UpgradeableInfo;
    public IntEventChannel UpgradeHealthEventChannel;
    public IntEventChannel UpgradeAttackEventChannel;
    public IntEventChannel UpgradeDefenseEventChannel;
    public IntEventChannel UpgradeLuckEventChannel;
    public IntEventChannel UpgradeCritDmgEventChannel;
    public void UpgradeHealthButton()
    {
        UpgradeHealthEventChannel.RaiseIntEvent(10);
    }
    public void UpgradeAttackButton()
    {
        UpgradeAttackEventChannel.RaiseIntEvent(2);
    }
    public void UpgradeDefenseButton()
    {
        UpgradeDefenseEventChannel.RaiseIntEvent(2);
    }
    public void UpgradeLuckButton()
    {
        UpgradeLuckEventChannel.RaiseIntEvent(2);
    }
    public void UpgradeCritDmgButton()
    {
        UpgradeCritDmgEventChannel.RaiseIntEvent(5);
    }
}
