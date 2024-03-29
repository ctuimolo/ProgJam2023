using Godot;

using System.Collections.Generic;

namespace ProgJam2023.Rooms;

public static class EnemySpawner
{
   public struct Instruction
   {
      public EnemyType Type;
      public Vector2I Cell;
      public StringName Name;
   }
   public enum EnemyType
   {
      GreenSlime,
      Spider,
      Bat,
      Rat,
      Mushroom,
      Moldman,
      Snail,
      PlaceHolder,
   }

   private static Dictionary<EnemyType, string> _library = new Dictionary<EnemyType, string>()
   {
      { EnemyType.GreenSlime, "res://Actors/Enemies/Slimes/Slime.tscn" },
      { EnemyType.Spider, "res://Actors/Enemies/Spiders/Spider.tscn" },
      { EnemyType.Bat, "res://Actors/Enemies/Bats/Bat.tscn" },
      { EnemyType.Rat, "res://Actors/Enemies/Rats/Rat.tscn" },
      { EnemyType.Mushroom, "res://Actors/Enemies/Mushrooms/Mushroom.tscn" },
      { EnemyType.Moldman, "res://Actors/Enemies/Moldmen/Moldman.tscn" },
      { EnemyType.Snail, "res://Actors/Enemies/Snails/Snail.tscn" },
   };

   public static PackedScene GetEnemyScene(EnemyType type)
   {
      return ResourceLoader.Load<PackedScene>(_library[type]);
   }
}
