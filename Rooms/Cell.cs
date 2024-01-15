using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors;

namespace ProgJam2023.Rooms
{
   public partial class Cell : Node
   {
      public int X;
      public int Y;

      public Room Room;

      public List<GridActor> Actors { get; private set;}

      public Cell(Vector2I location, Room room)
      {
         X = location.X; 
         Y = location.Y;

         Room = room;

         Actors = new List<GridActor>();
      }

      public bool HasCollisions(bool allowDoors = true)
      {
         foreach (GridActor actor in Actors) 
         {
            if (actor.CollisionType == GridActor.ActorCollisionType.Solid)
               return true;

            if (!allowDoors && actor.Type == GridActorType.Door)
               return true;
         }
         return false;
      }

      public void PutActor(GridActor actor)
      {
         Actors.Add(actor);
      }

      public void RemoveActor(GridActor actor)
      {
         Actors.Remove(actor);
      }

      public Vector2I Position()
      {
         return new Vector2I(X, Y);
      }
   }
}
