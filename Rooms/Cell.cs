using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors;

namespace ProgJam2023.Rooms
{
   public partial class Cell : Node
   {
      public int X;
      public int Y;

      public List<GridActor> Actors { get; private set;}

      public Cell(Vector2I location)
      {
         X = location.X; 
         Y = location.Y;

         Actors = new List<GridActor>();
      }

      public bool HasCollisions()
      {
         foreach (GridActor actor in Actors) 
         {
            if (actor.CollisionType == GridActor.ActorCollisionType.Solid)
               return true;
         }
         return false;
      }

      public void PutActor (GridActor actor)
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
