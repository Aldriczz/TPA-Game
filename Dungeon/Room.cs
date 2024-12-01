using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
   public Vector2 leftCorner;
   public int width;
   public int length;
   public List<Vector2> EntrancePoints;
   // public 

   public Room(Vector2 corner, int width, int length)
   {
      this.leftCorner = corner;
      this.width = width;
      this.length = length;
      EntrancePoints = new List<Vector2>();
   }
}
