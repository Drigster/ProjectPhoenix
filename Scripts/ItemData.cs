using Godot;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }
    [Export] public AtlasTexture Icon { get; set; }
    [Export] public int MaxStack { get; set; }
    [Export] public bool IsStackable { get; set; }
}
