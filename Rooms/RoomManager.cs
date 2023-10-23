using Godot;
using Godot.Collections;

using ProgJam2023.Actors.Player;
using ProgJam2023.World;

namespace ProgJam2023.Rooms;

public partial class RoomManager : Node
{
   [Export]
   public Array<Room> Rooms { get; set; }

   [Export]
   public Room CurrentRoom { get; set; }

   [Export]
   public Player Player { get; set; }

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      foreach (Room room in Rooms)
      {
         room.PauseAndHideRoom();
      }

      WorldManager.ChangeRoom(CurrentRoom);
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      if (Input.IsActionJustPressed("debug1"))
      {
         WorldManager.ChangeRoom(Rooms[1]);
      }

      if (Input.IsActionJustPressed("debug2"))
      {
         WorldManager.ChangeRoom(Rooms[2]);
      }

      if (Input.IsActionJustPressed("debug3"))
      {
         WorldManager.ChangeRoom(Rooms[3]);
      }

      if (Input.IsActionJustPressed("debug4"))
      {
         WorldManager.ChangeRoom(Rooms[4]);
      }
   }
}
