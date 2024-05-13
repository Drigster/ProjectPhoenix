using Godot;

public partial class UIPauseMenu : PanelContainer
{
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_toggle"))
		{
			Resume();
			AcceptEvent();
		}
	}

	public void Resume()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	public void Quit()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
