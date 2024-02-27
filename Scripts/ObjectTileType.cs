using Godot;

[GlobalClass]
public partial class ObjectTileType : Resource
{
    [Export] private int _id;
    [Export] private PackedScene _scene;

    public int Id => _id;
    public PackedScene Scene => _scene;
}
