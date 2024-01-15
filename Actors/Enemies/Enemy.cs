using Godot;

namespace ProgJam2023.Actors.Enemies;

public partial class Enemy : GridActor
{
   private protected bool _needsToTakeTurn = true;

   public virtual void TakeTurn()
   {
   }

   public virtual void Idle()
   {
      _needsToTakeTurn = true;
   }
   public virtual bool IsMobile() => false;
}
