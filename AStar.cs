using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private int lengthMap;
    private int widthMap;
    private List<Tile> tileListQueue;
    
    int[] moveX = { 0, 1,  0, -1 };
    int[] moveY = { 1, 0, -1,  0 };
    public AStar()
    {
        lengthMap = 50;
        widthMap = 50;
        tileListQueue = new List<Tile>();
    }

    private void insertTile(Tile t){
        if (tileListQueue.Contains(t)) return;
        for(int i = 0; i < tileListQueue.Count; i++){
            if(tileListQueue[i].F > t.F)
            {
                tileListQueue.Insert(i, t);
                return;
            }
        }
        tileListQueue.Add(t);
    }


    private char[,] duplicateMap(char[,] map){
        char[,] dMap = new char[lengthMap, widthMap];
        for(int i = 0; i < lengthMap; i++){
            for (int j = 0; j < widthMap; j++){
                dMap[i, j] = map[i, j];
            }
        }
        return dMap;
    }

    public List<Tile> Trace(Tile s, Tile e, char[,] map){
        char[,] dMap = duplicateMap(map);
        tileListQueue.Clear();
        
        insertTile(s);
        Tile curr = null;

        while(tileListQueue.Count > 0)
        {
            curr = tileListQueue[0];
            // Debug.Log($"Current Tile: {curr.x}, {curr.y}");
            tileListQueue.RemoveAt(0);
            dMap[curr.x, curr.y] = 'X';

            if (curr.x == e.x && curr.y == e.y) return traceback(curr);

            for(int i = 0; i < 4; i++){
                if (curr.x + moveX[i] < 0 || curr.y + moveY[i] < 0 || curr.x + moveX[i] >= lengthMap || curr.y + moveY[i] >= widthMap) continue;
                if (dMap[curr.x + moveX[i], curr.y + moveY[i]] == '#' || dMap[curr.x + moveX[i], curr.y + moveY[i]] == '.') continue;
                
                if (dMap[curr.x + moveX[i], curr.y + moveY[i]] != 'X')
                {
                    Tile newTile = new Tile(curr.x + moveX[i], curr.y + moveY[i]);
                    newTile.Prev = curr;
                    newTile.G = curr.G + 1;  
                    newTile.SetHeuristic(e);
                    
                    var skip = false;
                    foreach (var tile in tileListQueue)
                    {
                        if (tile.x == newTile.x && tile.y == newTile.y && tile.F <= newTile.F)
                        {
                            skip = true;
                            break;
                        }
                    }
                    if (!skip)
                    {
                        insertTile(newTile);
                    }
                }

            }
        }
        return null;
    }

    private List<Tile> traceback(Tile curr){
        List<Tile> result = new List<Tile>();
        while(curr != null)
        {
            result.Add(curr); 
            curr = curr.Prev;
        }

        if (result.Count < 16)
        {
            result.RemoveAt(result.Count - 1);
            result.Reverse();  
            return result;
        }
        else
        {
            return null;
        }
        
    }

}