using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public static HoverManager Instance;

    private AStar astar;
    [HideInInspector] public List<TileObject> highlightedPath;
    private GameObject[,] tileGameObjects;
    
    
    [HideInInspector] public List<Tile> path;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        astar = new AStar();
        highlightedPath = new List<TileObject>();
        tileGameObjects = DungeonGenerator.Instance.tileGameObjectsMap;
        path = new List<Tile>();
    }

    public void OnTileHovered(TileObject hoveredTile)
    {
        ResetHighlights();
        if (PlayerStateMachine.Instance.currentState == PlayerStateMachine.Instance.idleState)
        {
            Vector3 playerPos = PlayerStateMachine.Instance.transform.position;
            Tile start = new Tile((int)playerPos.x, (int)playerPos.z);
            Tile end = new Tile((int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.z);

            path = astar.Trace(start, end, DungeonGenerator.Instance.map);

            if (path != null && path.Count < 15)
            {
                foreach (Tile tile in path)
                {
                    GameObject tileObj = tileGameObjects[tile.x, tile.y];
                    TileObject tileComponent = tileObj.GetComponent<TileObject>();
                    tileComponent.Highlight();
                    highlightedPath.Add(tileComponent);
                }
            }
        }
        else
        {
            hoveredTile.Highlight();
            highlightedPath.Add(hoveredTile);
        }
    }

    public void OnTileUnhovered(TileObject unhoveredTile)
    {
        ResetHighlights();
    }

    private void ResetHighlights()
    {
        foreach (TileObject tile in highlightedPath)
        {
            tile.ResetHighlight();
        }
        highlightedPath.Clear();
    }
}