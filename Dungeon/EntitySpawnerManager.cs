using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntitySpawnerManager : MonoBehaviour
{
    public static EntitySpawnerManager Instance { get; private set; }
        
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private PlayerStatsSO PlayerStats;
    
    private DungeonGenerator dungeonGenerator;

    private int lengthMap;
    private int widthMap;
    
    [HideInInspector] 
    public List<EnemyStateMachine> enemyList = new List<EnemyStateMachine>();
    
    private string[] EnemyNames =
    {
        "AC",
        "AS",
        "BD",
        "BT",
        "CT",
        "FO",
        "GN",
        "GY",
        "HO",
        "KH",
        "MM",
        "MR",
        "MV",
        "NS",
        "OV",
        "PL",
        "RU",
        "TI",
        "VD",
        "VM",
        "WS",
        "WW",
        "YD"
    };
    // Start is called before the first frame update
    private void Start()
    {
        if(Instance == null) Instance = this; else Destroy(gameObject);
        
        dungeonGenerator = DungeonGenerator.Instance;
        lengthMap = dungeonGenerator.lengthMap;
        widthMap = dungeonGenerator.widthMap;
        InitEntity();
    }

    private void InitEntity()
    {
        if (PlayerStats.CurrentLevel == 0)
        {
            GenerateDecorations();
            SpawnBoss();
            PutPlayer(Player);
        }
        else
        {
            GenerateDecorations();
            SpawnEnemies();
            PutPlayer(Player);
        }
    }
    
    private void PutPlayer(GameObject player)
    {
        int x = Random.Range(1, lengthMap - 1);
        int y = Random.Range(1, widthMap - 1);

        while (dungeonGenerator.map[x, y] == '.' || dungeonGenerator.map[x, y] == '#')
        {
            x = Random.Range(1, lengthMap - 1);
            y = Random.Range(1, widthMap - 1);
        }
        
        player.transform.position = new Vector3(x, 0.75f, y);
    }
    
    private void GenerateDecorations()
    {
        for (int i = 1; i < lengthMap - 1; i++)
        {
            for (int j = 1; j < widthMap - 1; j++)
            {
                int r = Random.Range(0, 100);
                int randomRotation = Random.Range(0, 360);
                int fourRotationMultiplication = Random.Range(0, 4);
                if (dungeonGenerator.map[i, j] == ' ' && dungeonGenerator.map[i - 1, j] != '-' && dungeonGenerator.map[i + 1, j] != '-' && dungeonGenerator.map[i, j - 1] != '-' && dungeonGenerator.map[i, j + 1] != '-' 
                    && dungeonGenerator.map[i - 1, j] != '.' && dungeonGenerator.map[i + 1, j] != '.' && dungeonGenerator.map[i, j - 1] != '.' && dungeonGenerator.map[i, j + 1] != '.'
                    && dungeonGenerator.map[i - 1, j + 1] != '.' && dungeonGenerator.map[i + 1, j + 1] != '.' && dungeonGenerator.map[i - 1, j - 1] != '.' && dungeonGenerator.map[i + 1, j - 1] != '.')
                {
                    Quaternion objectRotation = Quaternion.Euler(0, randomRotation, 0);
                    if (r < 5)
                    {
                        objectRotation = Quaternion.Euler(0, fourRotationMultiplication * 90, 0);
                        Instantiate(Resources.Load<GameObject>("ChairAndTable"), new Vector3(i, 1f, j), objectRotation);
                        dungeonGenerator.map[i, j] = '.';
                        
                    }else if (r >= 5 && r < 10)
                    {
                        objectRotation = Quaternion.Euler(0, fourRotationMultiplication * 90, 0);
                        Instantiate(Resources.Load<GameObject>("Chest"), new Vector3(i, 0.9f, j), objectRotation);
                        dungeonGenerator.map[i, j] = '.';
                        
                    }else if (r >= 10 && r < 20)
                    {
                        Instantiate(Resources.Load<GameObject>("Bookshelf"), new Vector3(i, 1.5f, j), objectRotation);
                        dungeonGenerator.map[i, j] = '.';
                        
                    }else if (r >= 20 && r < 25)
                    {
                        Instantiate(Resources.Load<GameObject>("Pillar"), new Vector3(i, 0.75f, j), objectRotation);
                        dungeonGenerator.map[i, j] = '.';
                    }
                    else if (r >= 25 && r < 30)
                    {
                        objectRotation = Quaternion.Euler(0, Mathf.Abs(fourRotationMultiplication * 90), 0);
                        Instantiate(Resources.Load<GameObject>("Box"), new Vector3(i, 0.96f, j), objectRotation);
                        dungeonGenerator.map[i, j] = '.';
                        switch (fourRotationMultiplication)
                        {
                            case 0:
                                if(dungeonGenerator.map[i - 1, j] != '#')
                                dungeonGenerator.map[i - 1, j] = '.';
                                break;
                            case 1:
                                if(dungeonGenerator.map[i, j + 1] != '#')
                                dungeonGenerator.map[i, j + 1] = '.';
                                break;
                            case 2:
                                if(dungeonGenerator.map[i + 1, j] != '#')
                                dungeonGenerator.map[i + 1, j] = '.';
                                break;
                            case 3:
                                if(dungeonGenerator.map[i, j - 1] != '#')
                                dungeonGenerator.map[i, j - 1] = '.';
                                break;
                        }
                    }
                }
            }
        }
    }
    
    private void SpawnEnemies()
    {
        var RoomList = DungeonGenerator.Instance.roomList;
        var RoomNumber = RoomList.Count;
        var RoomCounter = 0;
        var numberOfEnemies = 5 + PlayerStats.CurrentLevel / 3 +  Random.Range(0, 10);
        var EnemyPerRoom = Mathf.Round((float) numberOfEnemies / (float) RoomNumber);
        var EnemyCounterPerRoom = 0;


        while (numberOfEnemies > 0)
        {
            var x = Random.Range((int)RoomList[RoomCounter].leftCorner.x, (int)RoomList[RoomCounter].leftCorner.x + RoomList[RoomCounter].length - 1);
            var y = Random.Range((int)RoomList[RoomCounter].leftCorner.y, (int)RoomList[RoomCounter].leftCorner.y + RoomList[RoomCounter].width - 1);

            var rarity = Random.Range(0, 100);

            if (dungeonGenerator.map[x, y] == ' ')
            {
                GameObject enemy;
                float angle = Random.Range(0, 360);
                Quaternion objectRotation = Quaternion.Euler(0, angle, 0);

                if (rarity < PlayerStats.CurrentLevel / 2)
                {
                    enemy = Instantiate(Resources.Load<GameObject>("Entity/EliteEnemy"), new Vector3(x, 0.75f, y),
                        objectRotation);
                    enemy.GetComponent<Enemy>().Stat =
                        EnemyFactory.CreateEnemyStat("Elite", PlayerStats.CurrentLevel);
                }
                else if (rarity < PlayerStats.CurrentLevel + 5 / 2)
                {
                    enemy = Instantiate(Resources.Load<GameObject>("Entity/MediumEnemy"), new Vector3(x, 0.75f, y),
                        objectRotation);
                    enemy.GetComponent<Enemy>().Stat =
                        EnemyFactory.CreateEnemyStat("Medium", PlayerStats.CurrentLevel);
                }
                else
                {
                    enemy = Instantiate(Resources.Load<GameObject>("Entity/CommonEnemy"), new Vector3(x, 0.75f, y),
                        objectRotation);
                    enemy.GetComponent<Enemy>().Stat =
                        EnemyFactory.CreateEnemyStat("Common", PlayerStats.CurrentLevel);
                }

                enemyList.Add(enemy.GetComponent<EnemyStateMachine>());
                enemy.GetComponentInChildren<Text>().text = EnemyNames[Random.Range(0, 23)];
                dungeonGenerator.map[x, y] = '#';
                enemy.transform.tag = "Enemy";
                numberOfEnemies--;
                EnemyCounterPerRoom++;
                
                if (EnemyCounterPerRoom == EnemyPerRoom)
                {
                    EnemyCounterPerRoom = 0;
                    RoomCounter++;
                    if (RoomCounter == RoomList.Count)
                    {
                        RoomCounter = 0;
                    }
                }
            }
                
        }
    }

    private void SpawnBoss()
    {
        var isSpawned = false;

        while (!isSpawned)
        {
            float angle = Random.Range(0, 360);
            Quaternion objectRotation = Quaternion.Euler(0, angle, 0);
            var x = Random.Range(0, lengthMap);
            var y = Random.Range(0, widthMap);
            
            if (dungeonGenerator.map[x, y] == ' ')
            {
                GameObject boss;
                boss = Instantiate(Resources.Load<GameObject>("Entity/Boss"), new Vector3(x, 0.75f, y), objectRotation);
                boss.GetComponent<Enemy>().Stat = EnemyFactory.CreateEnemyStat("Boss", PlayerStats.CurrentLevel);
                
                enemyList.Add(boss.GetComponent<EnemyStateMachine>());
                boss.GetComponentInChildren<Text>().text = EnemyNames[Random.Range(0, 23)];
                dungeonGenerator.map[x, y] = '#';
                boss.transform.tag = "Enemy";
                isSpawned = true;
            }
        }
    }
}
