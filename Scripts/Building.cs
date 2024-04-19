using Godot;
using Godot.Collections;

public partial class Building : StaticBody2D
{
	// Circular dependency, bugged in GD
	private Blueprint _blueprint;
	private int _health;
    public int Health => _health;

	public void SetBlueprint(Blueprint blueprint){
		_blueprint = blueprint;
	}

    public void Hit(int damage)
    {
		if(_health - damage > 0){
			_health -= damage;
		}
		else{
			_health = 0;
		}

		if(_health == 0){
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
