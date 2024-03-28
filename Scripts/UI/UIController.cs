using Godot;
using Godot.Collections;
using System.Linq;

public partial class UIController : Control
{
	private Array<UIElement> _nonTogglableUiElements = new Array<UIElement>();
	private Array<UIActionGroup> _togglableUiElements = new Array<UIActionGroup>();

	public override void _Ready()
	{
		Visible = true;

		foreach (Node child in GetChildren()){
			if (child is UIElement){
				UIElement uiElement = (UIElement)child;
				if(uiElement.inputEventAction != null && uiElement.inputEventAction.Action != ""){
					UIActionGroup uiActionGroup = _togglableUiElements.FirstOrDefault(x => x.Action.Action == uiElement.inputEventAction.Action);
					if (uiActionGroup != null){
						uiActionGroup.Elements.Add(uiElement);
					}
					else{
						_togglableUiElements.Add(new UIActionGroup(){
							Action = uiElement.inputEventAction,
							Elements = new Array<UIElement>(){uiElement}
						});
					}
				}
				else{
					_nonTogglableUiElements.Add(uiElement);
				}
				uiElement.Visible = uiElement.isActiveOnStart;
			}
			else{
				GD.PushError("Child of UIController is not UIElement. Child: " + child.Name + " Deleting...");
				child.QueueFree();
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		foreach (UIActionGroup uiActionGroup in _togglableUiElements){
			if (@event.IsActionPressed(uiActionGroup.Action.Action)){
				if(uiActionGroup.Elements.Count > 0){
					HideAll();
				}
				uiActionGroup.IsOpen = !uiActionGroup.IsOpen;
				foreach (UIElement uiElement in uiActionGroup.Elements){
					uiElement.Visible = uiActionGroup.IsOpen;
				}
			}
		}
	}

	public void HideAll() {
		foreach (UIActionGroup uiActionGroup in _togglableUiElements){
			foreach (UIElement uiElement in uiActionGroup.Elements){
				uiElement.Visible = false;
			}
		}
	}
}
