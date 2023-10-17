using Godot;
using ProgJam2023.World;
using System;

namespace ProgJam2023.Actors;

public partial class GridActor : Node2D
{
    [Export]
    protected AnimationPlayer _animationPlayer;

    public Direction Direction = Direction.Down;

    public Vector2I CurrentCell = new Vector2I(0, 0);

    public void SingleStep(Direction direction)
    {
        Vector2I toTile = CurrentCell + Utils.DirectionToVector(direction);

        if (!WorldManager.CurrentRoom.TileMap.GetUsedCells(0).Contains(toTile)) return;

        WorldManager.CurrentRoom.PutOnTile(toTile, this);
    }
}
