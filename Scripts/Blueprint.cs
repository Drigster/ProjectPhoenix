using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Blueprint : ItemData, IProcessAction, IAction
{
    [Export] private PackedScene _placedObject;
    [Export] private PackedScene _ghostObject;
    [Export] private int _width = 1;
    [Export] private int _height = 1;
    
    public void Action(Item self, Node2D caller, Vector2 target)
    {
        Building building = _placedObject.Instantiate<Building>();
        building.SetBlueprint(this);
        self.Remove(1);

        building.Position = target;
        caller.GetTree().Root.GetNode("Main").AddChild(building);
    }

    public void ProcessAction()
    {

    }
}
