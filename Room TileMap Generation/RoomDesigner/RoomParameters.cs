using Godot;
using System;

namespace ProgJam2023.RoomTileMapGeneration;

[GlobalClass]
public partial class RoomParameters : Resource
{
	[Export]
	public string RoomName;
	
	[Export]
	public bool HasNorthDoor, HasSouthDoor, HasEastDoor, HasWestDoor;
	[Export]
	public string NorthRoom, SouthRoom, EastRoom, WestRoom;
	
	[Flags]
	public enum RoomSize {
		ExtraSmall = 1,
		Small = 2,
		Medium = 4,
		Large = 8,
		ExtraLarge = 16,
		AnySmall = ExtraSmall | Small,
		AnyMedium = Small | Medium | Large,
		AnyLarge = Large | ExtraLarge
	}
	[Export]
	public RoomSize Width, Height;
	// Move to resource later
	public Vector2I GetSize(RoomSize size)
	{
		switch(size)
		{
			case RoomSize.ExtraSmall:
				return new Vector2I(10, 8);
			case RoomSize.Small:
				return new Vector2I(12, 10);
			case RoomSize.Medium:
				return new Vector2I(18, 12);
			case RoomSize.Large:
				return new Vector2I(22, 14);
			case RoomSize.ExtraLarge:
				return new Vector2I(26, 16);
			default:
				throw new ArgumentException();
		}
	}
	
	[Flags]
	public enum RoomProportion {
		Normal = 1,
		Square = 2
	}
	[Export]
	public RoomProportion Proportion;
	public bool IsSquareProportions() => Proportion == RoomProportion.Square;
	
	public RoomParameters()
	{
		
	}
	public RoomParameters(RoomParameters other) : this()
	{
		HasNorthDoor = other.HasNorthDoor;
		HasSouthDoor = other.HasSouthDoor;
		HasEastDoor = other.HasEastDoor;
		HasWestDoor = other.HasWestDoor;
		Width = other.Width;
		Height = other.Height;
		Proportion = other.Proportion;
	}
	
	public RoomParameters GetCollapsed(RandomNumberGenerator rng)
	{
		RoomParameters collapsed = new RoomParameters(this);
		collapsed.Collapse(rng);
		return collapsed;
	}
	public void Collapse(RandomNumberGenerator rng)
	{
		Width = PickEnum<RoomSize>(Width, rng);
		Height = PickEnum<RoomSize>(Height, rng);
		
		Proportion = PickEnum<RoomProportion>(Proportion, rng);
	}
	
	private static int FlagCount<T>(T flags) where T : struct, IConvertible
	{
		int f = flags.ToInt32(null);
		int count = 0;
		for(int i = 0; i < 32; i++)
		{
			if((f & (1 << i)) != 0)
			{
				count++;
			}
		}
		return count;
	}
	private static T PickEnum<T>(T flags, RandomNumberGenerator rng) where T : struct, IConvertible
	{
		int f = flags.ToInt32(null);
		if(f == 0)
		{
			return flags;
		}
		
		int count = FlagCount<T>(flags);
		if(count == 1)
		{
			return flags;
		}
		
		int pickIndex = rng.RandiRange(0, count - 1);
		for(int i = 0; i < 32; i++)
		{
			if((f & (1 << i)) != 0)
			{
				pickIndex--;
				if(pickIndex < 0)
				{
					return (T)(object)(1 << i);
				}
			}
		}
		
		// Should not reach this point
		throw new Exception();
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{");
		
		stringBuilder.Append($"Doors: ");
		stringBuilder.Append(HasNorthDoor ? "N" : "");
		stringBuilder.Append(HasSouthDoor ? "S" : "");
		stringBuilder.Append(HasEastDoor ? "E" : "");
		stringBuilder.Append(HasWestDoor ? "W" : "");
		if(!HasNorthDoor && !HasSouthDoor && !HasEastDoor && !HasWestDoor)
		{
			stringBuilder.Append("None");
		}
		stringBuilder.Append(", ");
		
		stringBuilder.Append($"(Width: {Width}, Height: {Height}), ");
		
		stringBuilder.Append($"Proportion: {Proportion}");
		
		stringBuilder.Append("}");
		
		return stringBuilder.ToString();
	}
	
}
