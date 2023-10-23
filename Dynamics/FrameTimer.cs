using Godot;

namespace ProgJam2023.Dynamics;

public partial class FrameTimer : Node
{
   private enum TimerState
   {
      Finished,
      Counting,
   }

   private TimerState _state;
   private int _count;

   public bool Finished()
   {
      return _state != TimerState.Finished;
   }

   public void ForceFinish()
   {
      _count = 0;
      _state = TimerState.Finished;
   }

   public override void _Ready()
   {
      _state = TimerState.Finished;
      _count = 0;
   }

   public void SetFrameCount(int frames)
   {
      _count = frames;
      _state = TimerState.Counting;
   }

   public override void _Process(double delta)
   {
      if (_count <= 0)
      {
         _state = TimerState.Finished;
      }

      if (_count > 0)
      {
         _count--;
      }
   }
}
