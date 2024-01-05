using System;
using Godot;

namespace ProgJam2023.StateMachines
{

	public abstract partial class StateMachineNode : Node
	{

		public State CurrentState { get; protected set; }

		public virtual void MoveToState(State state)
		{
			if(CurrentState != null) CurrentState.Exit();
			CurrentState = state;
			CurrentState.Enter();
		}

		public override void _Process(double delta)
		{
			if(CurrentState != null)
			{
				CurrentState.Update();
			}
		}
	}

	public abstract class State
	{
		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
		public virtual string GetName()
		{
			return GetType().Name;
		}
	}

	public abstract class State<TStateMachine> : State
	{
		public TStateMachine StateMachine;
		public State(TStateMachine stateMachine)
		{
			StateMachine = stateMachine;
		}
	}

}
