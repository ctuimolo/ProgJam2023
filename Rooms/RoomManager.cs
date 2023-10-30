using Godot;
using System.Collections.Generic;

using ProgJam2023.Actors.Players;
using ProgJam2023.World;

namespace ProgJam2023.Rooms;

public partial class RoomManager : Node
{
   public Dictionary<StringName, Room> Rooms { get; set; }

   [Export]
   public Room CurrentRoom { get; set; }

   [Export]
   public StringName DebugStartRoom;

   [Export]
   public Player Player { get; set; }

   [Export]
   private PackedScene RoomScene;

   [Export]
   private Godot.Collections.Array<PackedScene> DebugRoomTileMaps;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      Rooms = new Dictionary<StringName, Room>();

      ////////////////////////////////////////////////////////////
      // Init Debug Rooms
      foreach (PackedScene tileMapScene in DebugRoomTileMaps)
      {
         Room newRoom         = RoomScene.Instantiate<Room>();
         RoomTileMap tileMap  = tileMapScene.Instantiate<RoomTileMap>();

         newRoom.AddChild(tileMap);
         newRoom.Map = tileMap;

         AddChild(newRoom);
         Rooms[tileMap.DebugName] = newRoom;
      }
      ////////////////////////////////////////////////////////////

      // Actual rooms
      foreach (Room room in Rooms.Values)
      {
         room.InitCells();
         room.FindAndAddActors(true);
         room.PauseAndHideRoom();
      }

      WorldManager.SetRoomManager(this);
      WorldManager.ChangeRoom(DebugStartRoom, GridDirection.None);
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      if (Input.IsActionJustPressed("debug1"))
      {
         WorldManager.ChangeRoom("North", GridDirection.None);
      }

      if (Input.IsActionJustPressed("debug2"))
      {
         WorldManager.ChangeRoom("West", GridDirection.None);
      }

      if (Input.IsActionJustPressed("debug3"))
      {
         WorldManager.ChangeRoom("South", GridDirection.None);
      }

      if (Input.IsActionJustPressed("debug4"))
      {
         WorldManager.ChangeRoom("East", GridDirection.None);
      }

      if (Input.IsActionJustPressed("debug5"))
      {
         WorldManager.ChangeRoom("Hub", GridDirection.None);
      }
   }
}
