using Godot;
using System;

namespace ProgJam2023.UI.MainMenu;

public partial class MainMenu : Node
{
	[Export]
	public Button PlayButton;
	[Export]
	public Button QuitButton;
	
	[Signal]
	public delegate void PlayEventHandler();
	[Signal]
	public delegate void QuitEventHandler();
	
	public override void _Ready()
	{
		PlayButton.Pressed += OnPlay;
		QuitButton.Pressed += OnQuit;
	}
	
	private void OnPlay()
	{
		GD.Print("Play");
		EmitSignal(SignalName.Play);
	}
	
	private void OnQuit()
	{
		GD.Print("Quit");
		EmitSignal(SignalName.Quit);
	}
}
