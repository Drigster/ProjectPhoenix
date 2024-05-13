using Godot;

[GlobalClass]
public partial class ItemData : Resource
{
	[Export] public string Name { get; private set; }
	[Export(PropertyHint.MultilineText)] public string Description { get; private set; }
	[Export] public AtlasTexture Icon { get; private set; }
	[Export] public int MaxStack { get; private set; } = 1;
	public bool IsStackable => MaxStack > 1;
}
