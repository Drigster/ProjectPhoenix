using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class UIControler : Control
{
	private List<IUIElement> _nonTogglableUiElements = new List<IUIElement>();
	private Array<UIActionGroup> _togglableUiElements = new Array<UIActionGroup>();
	private UIPauseMenu _pauseMenu;
	[Export] private bool _isOpen;

	public override void _Ready()
	{
		Visible = true;

		foreach (Node child in GetChildren())
		{
			if (child is UIPauseMenu pauseMenu)
			{
				_pauseMenu = pauseMenu;
				_pauseMenu.Visible = false;
				continue;
			}

			if (child is IUIElement uiElement)
			{

				if (uiElement.InputEventAction != null && uiElement.InputEventAction.Action != "")
				{
					UIActionGroup uiActionGroup = _togglableUiElements.FirstOrDefault(x => x.Action.Action == uiElement.InputEventAction.Action);
					if (uiActionGroup != null)
					{
						uiActionGroup.Elements.Add(uiElement);
					}
					else
					{
						_togglableUiElements.Add(new UIActionGroup()
						{
							Action = uiElement.InputEventAction,
							Elements = new List<IUIElement>() { uiElement }
						});
					}
				}
				else
				{
					_nonTogglableUiElements.Add(uiElement);
				}

				if (uiElement.IsActiveOnStart)
				{
					uiElement.Open();
				}
				else
				{
					uiElement.Close();
				}
			}
			else
			{
				GD.PushError("Child of UIController is not UIElement. Child: " + child.Name + " Deleting...");
				child.QueueFree();
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("cancel") && _isOpen)
		{
			HideAll();
			_isOpen = false;
			AcceptEvent();
		}
		else if (@event.IsActionPressed("pause_toggle") && !_isOpen)
		{
			_pauseMenu.Visible = true;
			GetTree().Paused = true;
			AcceptEvent();
		}
		else
		{
			foreach (UIActionGroup uiActionGroup in _togglableUiElements)
			{
				if (@event.IsActionPressed(uiActionGroup.Action.Action))
				{
					if (uiActionGroup.IsOpen)
					{
						HideAll();
						_isOpen = false;
					}
					else
					{
						HideAll();
						uiActionGroup.IsOpen = true;
						foreach (IUIElement uiElement in uiActionGroup.Elements)
						{
							uiElement.Open();
						}
						_isOpen = true;
					}
				}
			}
			AcceptEvent();
		}
	}

	public void HideAll()
	{
		foreach (UIActionGroup uiActionGroup in _togglableUiElements)
		{
			uiActionGroup.IsOpen = false;
			foreach (IUIElement uiElement in uiActionGroup.Elements)
			{
				uiElement.Close();
			}
		}
	}
}
