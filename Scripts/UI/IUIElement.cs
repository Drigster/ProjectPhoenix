using Godot;
using System;

public interface IUIElement
{
    public InputEventAction InputEventAction { get; }
    public bool IsActiveOnStart { get; }

	public void Close();
	public void Open();
}
