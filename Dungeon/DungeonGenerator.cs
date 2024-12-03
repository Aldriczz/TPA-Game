using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance;

    [SerializeField] private PlayerStatsSO PlayerStats;
    [HideInInspector] public char[,] map; 
    [HideInInspector] public GameObject[,] tileGameObjectsMap;
    [HideInInspector] public List<Room> roomList;
    private Tile[,] tileMap;

    [HideInInspector] public int widthMap;
    [HideInInspector] public int lengthMap;
    private void Start()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
        
        roomList = new List<Room>();

        StartCreateMap();
    }

    private void StartCreateMap()
    {
        if (PlayerStats.CurrentLevel == 0) CreateBossMap();
        else CreateNormalMap();
    }

    private void CreateBossMap()
    {
        widthMap = 20;
        lengthMap = 20;
        
        map = new char[widthMap, lengthMap];
        tileMap = new Tile[widthMap, lengthMap];
        tileGameObjectsMap = new GameObject[widthMap, lengthMap];
        
        for (var i = 0; i < widthMap; i++)
        {
            for (var j = 0; j < lengthMap; j++)
            {
                map[i, j] = ' '; 
                Instantiate(Resources.Load<GameObject>("Tile"), new Vector3(i, 0, j), Quaternion.identity);
            }
        }
        GenerateMap();
    }   

    private void CreateNormalMap()
    {
        widthMap = 70;
        lengthMap = 70;
        
        map = new char[widthMap, lengthMap];
        tileMap = new Tile[widthMap, lengthMap];
        tileGameObjectsMap = new GameObject[widthMap, lengthMap];
        
        InitMap();
        RandomizeMap();
        
        ConnectRooms();
        GenerateMap();
    }

    private void InitMap()
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

    private void RandomizeMap()
    {
        int attempts = 15;
        while (attempts > 0)
        {
            
            int x = Random.Range(1, lengthMap - 1);
            int y = Random.Range(1, widthMap - 1);
            int roomLength = Random.Range(4, 11);
            int roomWidth = Random.Range(4, 11);

            Room newRoom = new Room(new Vector2(x, y), roomWidth, roomLength);
            
            if (IsRoomValid(newRoom))
            {
                AddRoom(newRoom);
            }
            attempts--; 
        }
    }

    private bool IsRoomValid(Room room)
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

    private void AddRoom(Room newRoom)
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

    private void GenerateMap()
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
                else
                {
                    GameObject tile = Instantiate(Resources.Load<GameObject>("Empty Tile"), new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }
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

    private List<(Vector2, Vector2)> GenerateMST(List<Vector2> centers)
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

    private void AddRandomConnections(List<Vector2> centers, List<(Vector2, Vector2)> connections)
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

    private void DrawCorridor(Vector2 start, Vector2 end)
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(widthMap / 2, 0, lengthMap / 2), new Vector3(widthMap, 0, lengthMap));
    }
}



