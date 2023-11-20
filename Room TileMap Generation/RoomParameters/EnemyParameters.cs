using Godot;
using System;

using static RandomSelection;

namespace ProgJam2023.RoomDesignParameters;

[GlobalClass]
public partial class EnemyParameters : Resource
{
	
	[Export]
	public EnemySet[] EnemySets;
	
	[Export]
	public int EnemySetCount = 1;
	
	[Export]
	public EnemyAmount Amount;
	
	[Flags]
	public enum EnemyAmount {
		None = 1,
		Few = 2,
		Many = 4
	}
	public Vector2I GetCountRange(EnemyAmount amount)
	{
		switch(amount)
		{
			case EnemyAmount.None:
				return new Vector2I(0, 0);
			case EnemyAmount.Few:
				return new Vector2I(1, 3);
			case EnemyAmount.Many:
				return new Vector2I(3, 5);
			default:
				throw new ArgumentException();
		}
	}
	
	
	public EnemyParameters()
	{
		EnemySets = new EnemySet[0];
	}
	public EnemyParameters(EnemyParameters other) : this()
	{
		EnemySets = new EnemySet[other.EnemySets.Length];
		Array.Copy(other.EnemySets, EnemySets, EnemySets.Length);
		EnemySetCount = other.EnemySetCount;
		Amount = other.Amount;
	}
	
	public EnemyParametersCollapsed GetCollapsed(RandomNumberGenerator rng)
	{
		EnemyParametersCollapsed collapsed = new EnemyParametersCollapsed();
		
		EnemyAmount amount = PickEnum<EnemyAmount>(Amount, rng);
		Vector2I range = GetCountRange(amount);
		int count = rng.RandiRange(range.X, range.Y);
		
		PackedScene[] enemies = new PackedScene[count];
		
		EnemySet[] sets = PickN<EnemySet>(EnemySets, EnemySetCount, rng);
		for(int i = 0; i < count; i++)
		{
			EnemySet set = Pick<EnemySet>(sets, rng);
			enemies[i] = Pick<PackedScene>(set.Enemies, rng);
		}
		
		collapsed.Enemies = enemies;
		
		return collapsed;
	}
	
	
	
	public override string ToString()
	{
		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
		stringBuilder.Append("{");
		
		stringBuilder.Append($"EnemySetCount: {EnemySetCount}, ");
		
		stringBuilder.Append($"EnemySets: {EnemySets}");
		
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}
	
}
