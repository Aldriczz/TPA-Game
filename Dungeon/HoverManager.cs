using System;
using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public static HoverManager Instance;

    private AStar astar;
    [HideInInspector] public List<TileObject> highlightedPath;
    private GameObject[,] tileGameObjects;
    private int layerMask;
    private TileObject currentTile;
    
    
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
        layerMask = LayerMask.GetMask("Ground", "Enemy");
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                OnTileHovered(hit.collider.gameObject.GetComponent<TileObject>());
            }
        }
    }

    public void OnTileHovered(TileObject hoveredTile)
    {
        if (currentTile == hoveredTile) return;
        ResetHighlights();

        if (PlayerStateMachine.Instance.currentState == PlayerStateMachine.Instance.movingState)
        {
            hoveredTile.Highlight();
            highlightedPath.Add(hoveredTile);
            currentTile = null;
            return;
        }
        else
        {
            ResetHighlights();
        }
        

        Vector3 playerPos = PlayerStateMachine.Instance.transform.position;
        Tile start = new Tile((int)playerPos.x, (int)playerPos.z);
        Tile end = new Tile((int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.z);

        path = astar.Trace(start, end, DungeonGenerator.Instance.map);
        
        if (path != null && path.Count < 15)
        {
            foreach (Tile tile in path)
            {
                var tileObj = tileGameObjects[tile.x, tile.y];
                var tileComponent = tileObj.GetComponent<TileObject>();
                tileComponent.Highlight();
                highlightedPath.Add(tileComponent);
            }
        }
        
        currentTile = hoveredTile;
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