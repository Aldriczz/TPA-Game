using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
   public int x {get; set;}
   public int y {get; set;}
   public Tile Prev {get; set;}
   public float G { get; set; } 
   public float F => G + Heuristic;  

   public float Heuristic {get; set;}

   public Tile(int x, int y)
   {
      this.x = x;
      this.y = y;
   }

   public void SetHeuristic(Tile dest)
   {
      this.Heuristic =  Mathf.Sqrt(Mathf.Pow(dest.x - this.x, 2) + Mathf.Pow(dest.y - this.y, 2));
   }
   
}
