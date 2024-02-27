using Godot;
using System;

public partial class Main : Node2D
{
	private Player player;
	private UIController uiController;
	
	public override void _Ready()
	{
		player = GetNode<Player>("Player");
		uiController = GetNode<UIController>("UI/UIController");
	}
}
