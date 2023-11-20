using Godot;
using System;

[GlobalClass]
public partial class RoomData : Resource
{
	[Export]
	public string RoomName;
	
	[Export]
	public bool HasNorthDoor, HasSouthDoor, HasEastDoor, HasWestDoor;
	[Export]
	public string NorthRoom, SouthRoom, EastRoom, WestRoom;
	
	public RoomData() { }
	public RoomData(RoomData other)
	{
		RoomName = other.RoomName;
		
		HasNorthDoor = other.HasNorthDoor;
		HasSouthDoor = other.HasSouthDoor;
		HasEastDoor = other.HasEastDoor;
		HasWestDoor = other.HasWestDoor;
		
		NorthRoom = other.NorthRoom;
		SouthRoom = other.SouthRoom;
		EastRoom = other.EastRoom;
		WestRoom = other.WestRoom;
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{ ");
		
		stringBuilder.Append($"RoomName: {RoomName}");
		stringBuilder.Append(", ");
		
		stringBuilder.Append($"Doors: ");
		stringBuilder.Append(HasNorthDoor ? "N" : "");
		stringBuilder.Append(HasSouthDoor ? "S" : "");
		stringBuilder.Append(HasEastDoor ? "E" : "");
		stringBuilder.Append(HasWestDoor ? "W" : "");
		if(!HasNorthDoor && !HasSouthDoor && !HasEastDoor && !HasWestDoor)
		{
			stringBuilder.Append("None");
		}
		
		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}
}
