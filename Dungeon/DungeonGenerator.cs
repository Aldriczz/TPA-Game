using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance;
    
    [HideInInspector] public char[,] map;
    [HideInInspector] public Tile[,] tileMap;
    [HideInInspector] public GameObject[,] tileGameObjectsMap;
    [HideInInspector] public List<Room> roomList;
    [HideInInspector] public List<EnemyStateMachine> enemyList;

    [HideInInspector]
    public int widthMap;
    [HideInInspector]
    public int lengthMap;

    [SerializeField]
    private GameObject Player;
    
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
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        widthMap = 50;
        lengthMap = 50;
        map = new char[widthMap, lengthMap];
        tileMap = new Tile[widthMap, lengthMap];
        tileGameObjectsMap = new GameObject[widthMap, lengthMap];
        roomList = new List<Room>();
        
        InitMap();
        RandomizeMap();
        
        ConnectRooms();
        GenerateMap();
        GenerateDecorations();
        SpawnEnemies();
    }

    private void Start()
    {
        PutPlayer(Player);
    }

    void InitMap()
    {
        for (int i = 0; i < widthMap; i++)
        {
            for (int j = 0; j < lengthMap; j++)
            {
                map[i, j] = '#'; 
                tileMap[i, j] = null; 
            }
        }
    }

    void RandomizeMap()
    {
        int attempts = 10;
        while (attempts > 0)
        {
            
            int x = Random.Range(1, lengthMap - 1);
            int y = Random.Range(1, widthMap - 1);
            int roomLength = Random.Range(4, 8);
            int roomWidth = Random.Range(4, 8);

            Room newRoom = new Room(new Vector2(x, y), roomWidth, roomLength);
            
            if (IsRoomValid(newRoom))
            {
                AddRoom(newRoom);
                attempts--; 
            }
        }
    }

    bool IsRoomValid(Room room)
    {
        if (room.leftCorner.x + room.length + 1 >= lengthMap ||
            room.leftCorner.y + room.width + 1 >= widthMap)
            return false;
        
        foreach (Room existingRoom in roomList)
        {
            if (room.leftCorner.x < existingRoom.leftCorner.x + existingRoom.length + 1 &&
                room.leftCorner.x + room.length + 1 > existingRoom.leftCorner.x &&
                room.leftCorner.y < existingRoom.leftCorner.y + existingRoom.width + 1 &&
                room.leftCorner.y + room.width + 1 > existingRoom.leftCorner.y)
            {
                return false;
            }
        }

        return true;
    }

    void AddRoom(Room newRoom)
    {
        roomList.Add(newRoom);
        int x = (int)newRoom.leftCorner.x;
        int y = (int)newRoom.leftCorner.y;

        for (int i = x; i < x + newRoom.length; i++)
        {
            for (int j = y; j < y + newRoom.width; j++)
            {
                map[i, j] = ' '; 
                tileMap[i, j] = new Tile(i, j);
            }
        }
    }

    void GenerateMap()
    {
        for (int i = 0; i < lengthMap; i++)
        {
            for (int j = 0; j < widthMap; j++)
            {
                int r = Random.Range(0, 20);
                int randomRotation = Random.Range(0, 360);
                if (map[i, j] == ' ' || map[i, j] == '-')
                {
                    GameObject tile = Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(i, 0, j), Quaternion.identity);
                    tileGameObjectsMap[i,j] = tile;
                    tile.transform.gameObject.name = $"Tile{i}_{j}";
                    if (r < 3)
                    {
                        Quaternion objectRotation = Quaternion.Euler(0, randomRotation, 0);
                        Instantiate(Resources.Load<GameObject>($"FloorDecoration{r + 1}"), new Vector3(i, 0.76f, j), objectRotation);
                    }
                }
            }
        }
    }

    private void PutPlayer(GameObject player)
    {
        int x = Random.Range(1, lengthMap - 1);
        int y = Random.Range(1, widthMap - 1);

        while (map[x, y] == '.' || map[x, y] == '#')
        {
            x = Random.Range(1, lengthMap - 1);
            y = Random.Range(1, widthMap - 1);
        }
        
        player.transform.position = new Vector3(x, 0.75f, y);
    }
    
    private void ConnectRooms()
    {
        List<Vector2> roomCenters = new List<Vector2>();
        foreach (Room room in roomList)
        {
            int centerX = (int)(room.leftCorner.x + room.width / 2);
            int centerY = (int)(room.leftCorner.y + room.length / 2);
            roomCenters.Add(new Vector2(centerX, centerY));
        }
        
        List<(Vector2, Vector2)> connections = GenerateMST(roomCenters);

        AddRandomConnections(roomCenters, connections);
    
        foreach (var connection in connections)
        {
            DrawCorridor(connection.Item1, connection.Item2);
        }
    }

    List<(Vector2, Vector2)> GenerateMST(List<Vector2> centers)
    {
        List<(Vector2, Vector2)> connections = new List<(Vector2, Vector2)>();
        HashSet<Vector2> connected = new HashSet<Vector2>();
        List<(Vector2, Vector2, float)> allEdges = new List<(Vector2, Vector2, float)>();
        
        for (int i = 0; i < centers.Count; i++)
        {
            for (int j = i + 1; j < centers.Count; j++)
            {
                float distance = Vector2.Distance(centers[i], centers[j]);
                allEdges.Add((centers[i], centers[j], distance));
            }
        }
        
        allEdges.Sort((a, b) => a.Item3.CompareTo(b.Item3));
        
        connected.Add(centers[0]);

        while (connected.Count < centers.Count)
        {
            for (int i = 0; i < allEdges.Count; i++)
            {
                var edge = allEdges[i];
                if (connected.Contains(edge.Item1) && !connected.Contains(edge.Item2))
                {
                    connections.Add((edge.Item1, edge.Item2));
                    connected.Add(edge.Item2);
                    allEdges.RemoveAt(i);
                    break;
                }
                else if (connected.Contains(edge.Item2) && !connected.Contains(edge.Item1))
                {
                    connections.Add((edge.Item2, edge.Item1));
                    connected.Add(edge.Item1);
                    allEdges.RemoveAt(i);
                    break;
                }
            }
        }

        return connections;
    }

    void AddRandomConnections(List<Vector2> centers, List<(Vector2, Vector2)> connections)
    {
        int extraConnections = Random.Range(1, centers.Count / 2);
        for (int i = 0; i < extraConnections; i++)
        {
            Vector2 roomA = centers[Random.Range(0, centers.Count)];
            Vector2 roomB = centers[Random.Range(0, centers.Count)];
            if (roomA != roomB && !connections.Contains((roomA, roomB)) && !connections.Contains((roomB, roomA)))
            {
                connections.Add((roomA, roomB));
            }
        }
    }

    void DrawCorridor(Vector2 start, Vector2 end)
    {
        int x = (int)start.x;
        int y = (int)start.y;
        int targetX = (int)end.x;
        int targetY = (int)end.y;
        
        while (x != targetX)
        {
            map[x, y] = '-';
            tileMap[x, y] = new Tile(x, y);
            if (x < targetX) x++;
            else x--;
        }
        
        while (y != targetY)
        {
            map[x, y] = '-';
            tileMap[x, y] = new Tile(x, y);
            if (y < targetY) y++;
            else y--;
        }
    }

    private void GenerateDecorations()
    {
        for (int i = 0; i < lengthMap; i++)
        {
            for (int j = 0; j < widthMap; j++)
            {
                int r = Random.Range(0, 100);
                int randomRotation = Random.Range(0, 360);
                int fourRotationMultiplication = Random.Range(0, 5);
                if (map[i, j] == ' ' && map[i - 1, j] != '-' && map[i + 1, j] != '-' && map[i, j - 1] != '-' && map[i, j + 1] != '-' 
                    && map[i - 1, j] != '.' && map[i + 1, j] != '.' && map[i, j - 1] != '.' && map[i, j + 1] != '.'
                    && map[i - 1, j + 1] != '.' && map[i + 1, j + 1] != '.' && map[i - 1, j - 1] != '.' && map[i + 1, j - 1] != '.')
                {
                    Quaternion objectRotation = Quaternion.Euler(0, randomRotation, 0);
                    if (r < 5)
                    {
                        objectRotation = Quaternion.Euler(0, fourRotationMultiplication * 90, 0);
                        Instantiate(Resources.Load<GameObject>("ChairAndTable"), new Vector3(i, 1f, j), objectRotation);
                        map[i, j] = '.';
                        
                    }else if (r >= 5 && r < 10)
                    {
                        objectRotation = Quaternion.Euler(0, fourRotationMultiplication * 90, 0);
                        Instantiate(Resources.Load<GameObject>("Chest"), new Vector3(i, 0.9f, j), objectRotation);
                        map[i, j] = '.';
                        
                    }else if (r >= 10 && r < 20)
                    {
                        Instantiate(Resources.Load<GameObject>("Bookshelf"), new Vector3(i, 1.5f, j), objectRotation);
                        map[i, j] = '.';
                        
                    }else if (r >= 20 && r < 25)
                    {
                        Instantiate(Resources.Load<GameObject>("Pillar"), new Vector3(i, 0.75f, j), objectRotation);
                        map[i, j] = '.';
                    }
                    else if (r >= 25 && r < 30)
                    {
                        objectRotation = Quaternion.Euler(0, fourRotationMultiplication * 90, 0);
                        Instantiate(Resources.Load<GameObject>("Box"), new Vector3(i, 0.96f, j), objectRotation);
                        map[i, j] = '.';
                        switch (fourRotationMultiplication)
                        {
                            case 0:
                                map[i - 1, j] = '.';
                                break;
                            case 1:
                                map[i - 1, j] = '.';
                                break;
                            case 2:
                                map[i + 1, j] = '.';
                                break;
                            case 3:
                                map[i, j - 1] = '.';
                                break;
                            case 4:
                                map[i, j + 1] = '.';
                                break;
                            
                        }
                    }
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        int numberOfEnemies = 3;
        Debug.Log(numberOfEnemies);
        int x = Random.Range(1, lengthMap - 1);
        int y = Random.Range(1, widthMap - 1);

        while (numberOfEnemies > 0)
        {
            x = Random.Range(1, lengthMap - 1);
            y = Random.Range(1, widthMap - 1);
            if (map[x, y] == ' ')
            {
                float angle = Random.Range(0, 360);
                Quaternion objectRotation = Quaternion.Euler(0, angle, 0);
                GameObject enemy = Instantiate(Resources.Load<GameObject>("Entity/Enemy"), new Vector3(x, 0.75f, y), objectRotation);
                enemyList.Add(enemy.GetComponent<EnemyStateMachine>());
                enemy.GetComponentInChildren<Text>().text = EnemyNames[Random.Range(0, 23)];
                map[x, y] = '#';
                enemy.transform.tag = "Enemy";
                numberOfEnemies--;
            }
        }
    }


    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(widthMap / 2, 0, lengthMap / 2), new Vector3(widthMap, 0, lengthMap));
    }
}



