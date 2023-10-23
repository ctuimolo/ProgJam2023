using Godot;

using ProgJam2023.Actors;

namespace ProgJam2023.Rooms;

public partial class Door : GridActor
{
   [Export]
   Room ToRoom;

   [Export]
   GridDirection ToDoor;

   public override void _Ready()
   {
   }
}
