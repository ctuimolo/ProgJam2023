using Godot;
using System;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class RoomLayoutParameters : Resource
{
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
	
	public RoomLayoutParameters() { }
	public RoomLayoutParameters(RoomLayoutParameters other)
	{
		Width = other.Width;
		Height = other.Height;
		Proportion = other.Proportion;
	}
	
	public RoomLayoutParametersCollapsed GetCollapsed(RandomNumberGenerator rng)
	{
		RoomLayoutParametersCollapsed collapsed = new RoomLayoutParametersCollapsed();
		
		RoomProportion proportion = RandomSelection.PickEnum<RoomProportion>(Proportion, rng);
		bool isSquare = proportion == RoomProportion.Square;
		
		RoomSize width = RandomSelection.PickEnum<RoomSize>(Width, rng);
		RoomSize height = RandomSelection.PickEnum<RoomSize>(Height, rng);
		
		int sizeX = GetSize(width).X;
		int sizeY = GetSize(height).Y;
		if(isSquare)
		{
			sizeX = Math.Min(sizeX, sizeY);
			sizeY = sizeX;
		}
		collapsed.Size = new Vector2I(sizeX, sizeY);
		
		collapsed.IsSquare = isSquare;
		
		return collapsed;
	}
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{ ");
		
		stringBuilder.Append($"Width: {Width}, ");
		stringBuilder.Append($"Height: {Height}, ");
		stringBuilder.Append($"Proportion: {Proportion}");
		
		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}
	
}
