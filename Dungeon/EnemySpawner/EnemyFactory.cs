using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{

    public static EnemyBaseStat CreateEnemyStat(string type, int curLvl)
    {
        switch (type)
        {
            case "Common":
                return new CommonEnemy(curLvl);
                break;
            
            case "Medium":
                return new MediumEnemy(curLvl);
                break;
            
            case "Elite":
                return new EliteEnemy(curLvl);
                break;
            case "Boss":
                return new BossEnemy();
                break;
        }
        
        return null;
    }
}
