using Godot;

using ProgJam2023.Actors;

namespace ProgJam2023.Rooms;

public partial class Door : GridActor
{
   [Export]
   public StringName ToRoom;

   [Export]
   public GridDirection ToDoor;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
      base._Ready();
   }

   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
      base._Process(delta);
   }

}
