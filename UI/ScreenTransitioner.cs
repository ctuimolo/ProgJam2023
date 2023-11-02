using Godot;
using ProgJam2023.World;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using static Godot.TextServer;

namespace ProgJam2023.UI;

public partial class ScreenTransitioner : CanvasLayer
{
   [Export]
   AnimationPlayer _animationPlayer;

   [Export]
   ColorRect _fillColorRect;
   
   [Export]
   int test;

   private void ConfigureFillShader_StartAnimation()
   {
      if (_fillColorRect == null) return;

      ShaderMaterial shader = (ShaderMaterial)_fillColorRect.Material;

      switch (WorldManager.CurrentPlayer.LastStep)
      {
         case GridDirection.Left:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", true);
            shader.SetShaderParameter("directionY", false);
            break;
         case GridDirection.Right:
            shader.SetShaderParameter("inverseDraw", true);
            shader.SetShaderParameter("directionX", true);
            shader.SetShaderParameter("directionY", false);
            break;
         case GridDirection.Up:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", true);
            break;
         case GridDirection.Down:
            shader.SetShaderParameter("inverseDraw", true);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", true);
            break;
         default:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", false);
            break;
      }
   }

   private void ConfigureFillShader_EndAnimation()
   {
      if (_fillColorRect == null) return;

      ShaderMaterial shader = (ShaderMaterial)_fillColorRect.Material;

      GridDirection direction = GridDirection.None;

      if (WorldManager.NextRoomDoor != null)
      {
         direction = WorldManager.CurrentRoom.Doors[WorldManager.NextRoomDoor].ExitDirection;
      }

      switch (direction)
      {
         case GridDirection.Left:
            shader.SetShaderParameter("inverseDraw", true);
            shader.SetShaderParameter("directionX", true);
            shader.SetShaderParameter("directionY", false);
            break;
         case GridDirection.Right:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", true);
            shader.SetShaderParameter("directionY", false);
            break;
         case GridDirection.Up:
            shader.SetShaderParameter("inverseDraw", true);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", true);
            break;
         case GridDirection.Down:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", true);
            break;
         default:
            shader.SetShaderParameter("inverseDraw", false);
            shader.SetShaderParameter("directionX", false);
            shader.SetShaderParameter("directionY", false);
            break;
      }
   }

   public void ChangeRoom_StartAnimation()
   {
      WorldManager.PauseWorld();
      ConfigureFillShader_StartAnimation();
      FadeOut();
   }

   public void ChangeRoom_MovePlayer()
   {
      WorldManager.CurrentRoom?.PauseAndHideRoom();
      WorldManager.SpawnPlayer(WorldManager.NextRoomKey, WorldManager.NextRoomDoor);
      ConfigureFillShader_EndAnimation();
      FadeIn();
      WorldManager.CurrentRoom.Visible = true;
   }

   public void ChangeRoom_EndAnimation()
   {
      if (WorldManager.NextRoomDoor != null)
      {
         WorldManager.SpawnPlayer_DoorAnimation(WorldManager.NextRoomDoor);
      }
      WorldManager.NextRoomKey = null;
      WorldManager.NextRoomDoor = null;
      WorldManager.ResumeWorld();
      WorldManager.CurrentRoom.ActivateRoom();
   }

   public void EndAnimation()
   {

      WorldManager.ResumeWorld();
   }

   public void PauseWorld()
   {
      WorldManager.PauseWorld();
   }

   public void ResumeWorld()
   {
      WorldManager.ResumeWorld();
   }

   public void FadeIn()
   {
      _animationPlayer.Play("fade_in");
   }

   public void FadeOut()
   {
      _animationPlayer.Play("fade_out");
   }

   public void Clear()
   {
      _animationPlayer.Play("clear");
   }

   public void Fill()
   {
      _animationPlayer.Play("fill");
   }

   public override void _Ready()
   {
      base._Ready();
   }
}
