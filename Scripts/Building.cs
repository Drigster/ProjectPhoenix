using Godot;
using Godot.Collections;

public partial class Building : StaticBody2D
{
	// Circular dependency, bugged in GD
	private Blueprint _blueprint;
	private int _durability;
	private Blueprint.Direction _direction;

    public int Durability => _durability;
	public Blueprint Blueprint => _blueprint;
	public Blueprint.Direction Direction => _direction;

	public static Building Build(Blueprint blueprint, Node2D parrent, Vector2 localPos){
		Building building = blueprint.PlacedObject.Instantiate<Building>();
		parrent.AddChild(building);
		building.RotationDegrees = blueprint.GetRotationAngle(blueprint.CurrentDirection);
		building.Position = localPos + blueprint.GetRotationOriginOffset(blueprint.CurrentDirection);
		building._direction = blueprint.CurrentDirection;
		building._blueprint = blueprint;
		building._durability = blueprint.BuildingDurability;

		return building;
	}

    public void Hit(int damage)
    {
		if(_durability - damage > 0){
			_durability -= damage;
		}
		else{
			_durability = 0;
		}

		if(_durability == 0){
            // TODO: Drop
			GD.PushWarning("Drop");
			QueueFree();
		}
    }

    public Blueprint DismantleWith(Tool tool)
    {
		QueueFree();
		return _blueprint;
    }
}
