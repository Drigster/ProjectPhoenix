using Godot;
using Godot.Collections;

public partial class MainMenu : Control
{
	private string _mainSceneResourceString = "res://Scenes/Main.tscn";
	private Array _progress = new Array();
	private Label _progressLabel;
	private Control _loadingNode;
	private bool _isLoadingStarted = false;

	public override void _Ready()
	{
		_loadingNode = GetNode<Control>("Loading");
		_progressLabel = GetNode<Label>("Loading/MarginContainer/Label");
		_loadingNode.Visible = false;
	}

	public override void _Process(double delta)
	{
		if (_isLoadingStarted)
		{
			ResourceLoader.ThreadLoadStatus sceneLoadStatus = ResourceLoader.LoadThreadedGetStatus(_mainSceneResourceString, _progress);
			_progressLabel.Text = Mathf.FloorToInt((float)_progress[0] * 100) + "%";
			if (sceneLoadStatus == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				GetTree().ChangeSceneToPacked(ResourceLoader.LoadThreadedGet(_mainSceneResourceString) as PackedScene);
			}
		}
	}

	public void OnPlayPressed()
	{
		ResourceLoader.LoadThreadedRequest(_mainSceneResourceString);
		_isLoadingStarted = true;
		_loadingNode.Visible = true;
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
