using Godot;
using ProgJam2023.World;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProgJam2023.UI;

public partial class ScreenTransitioner : CanvasLayer
{
   [Export]
   AnimationPlayer _animationPlayer;

   [Export]
   int test;

   public void ChangeRoom_StartAnimation()
   {
      WorldManager.PauseWorld();
      FadeOut();
   }

   public void ChangeRoom_MovePlayer()
   {
      WorldManager.CurrentRoom?.PauseAndHideRoom();
      WorldManager.SpawnPlayer(WorldManager.NextRoomKey, WorldManager.NextRoomDoor);
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
