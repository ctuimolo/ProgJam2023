using Godot;
using System;

namespace ProgJam2023.Rooms.Demo;

public partial class Demo : Room
{
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    InitRoom();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }
}
