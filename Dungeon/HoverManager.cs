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
        if (highlightedPath.Contains(hoveredTile)) return;
        ResetHighlights();

        if (PlayerStateMachine.Instance.currentState == PlayerStateMachine.Instance.movingState)
        {
            hoveredTile.Highlight();
            highlightedPath.Add(hoveredTile);
            return;
        }
        Debug.Log("ASTAARRRRRRRRR");

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