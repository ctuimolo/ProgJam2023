using Godot;
using ProgJam2023.World;
using System;
using static Godot.TextServer;

namespace ProgJam2023.Actors.Enemies.Slimes;

public partial class Slime : Enemy
{
   public override void TakeTurn()
   {
      if (_needsToTakeTurn)
      {
         GridDirection direction = (GridDirection)Utils.RNG.RandiRange(1, 4);

         if (WorldManager.TestTraversable(this, direction) && (CurrentCell.Position() + Utils.DirectionToVector(direction) != WorldManager.CurrentPlayer.CurrentCell.Position()))
         {
            WorldManager.TryMoveActor(this, direction);
         }

         _needsToTakeTurn = false;
      }
   }
   public override bool IsMobile() => true;
}
