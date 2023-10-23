using Godot;
using System;

using ProgJam2023.Actors.Player;
using ProgJam2023.World;

namespace ProgJam2023.Rooms;

public partial class RoomManager : Node
{
   [Export]
   public Godot.Collections.Array<Room> Rooms { get; set; }

   [Export]
   public Room CurrentRoom { get; set; }

   [Export]
   public Player Player { get; set; }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      WorldManager.ChangeRoom(CurrentRoom);
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }
}
