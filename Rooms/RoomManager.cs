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
	
	private bool Initialized = false;
	[Export]
	private bool InitializeOnReady = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Rooms = new Dictionary<StringName, Room>();
		if(InitializeOnReady)
		{
			Initialize();
		}
	}
	
	public void Initialize()
	{
		if(Initialized)
		{
			throw new System.InvalidOperationException();
		}
		Initialized = true;
		
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
			room.FindAndAddActors();
			room.PauseAndHideRoom();
		}

		WorldManager.SetCurrentPlayer(Player);
		WorldManager.SetRoomManager(this);
		WorldManager.ChangeRoom(DebugStartRoom);
	}
	
	public void AddRoom(string roomName, Room room)
	{
		if(Initialized)
		{
			throw new System.InvalidOperationException();
		}
		if(Rooms.ContainsKey(roomName))
		{
			throw new System.ArgumentException($"{this} already has room named {roomName}");
		}
		Rooms[roomName] = room;
		AddChild(room);
	}

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
	  if(!Initialized)
	  {
		  return;
	  }
	
	  if (Input.IsActionJustPressed("debug1"))
	  {
		 WorldManager.ChangeRoom("North");
	  }

	  if (Input.IsActionJustPressed("debug2"))
	  {
		 WorldManager.ChangeRoom("West");
	  }

	  if (Input.IsActionJustPressed("debug3"))
	  {
		 WorldManager.ChangeRoom("South");
	  }

	  if (Input.IsActionJustPressed("debug4"))
	  {
		 WorldManager.ChangeRoom("East");
	  }

	  if (Input.IsActionJustPressed("debug5"))
	  {
		 WorldManager.ChangeRoom("Hub");
	  }
   }
}
