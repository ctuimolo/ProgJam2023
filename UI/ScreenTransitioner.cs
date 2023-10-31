using Godot;
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

   public async Task WaitOnAnimation()
   {
      await Task.Run(() => 
      {
         while (_animationPlayer.IsPlaying())
         {
            // ...
         }
      });
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
