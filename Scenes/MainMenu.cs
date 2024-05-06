using Godot;
using System;

public partial class MainMenu : Control
{
	public void OnPlayPressed(){
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	public void OnQuitPressed(){
		GetTree().Quit();
	}
}
