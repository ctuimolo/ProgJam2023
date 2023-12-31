﻿using Godot;

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
      PlaceHolder,
   }

   private static Dictionary<EnemyType, string> _library = new Dictionary<EnemyType, string>()
   {
      { EnemyType.GreenSlime, "res://Actors/Enemies/Slimes/Slime.tscn" },
   };

   public static PackedScene GetEnemyScene(EnemyType type)
   {
      return ResourceLoader.Load<PackedScene>(_library[type]);
   }
}
