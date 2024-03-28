using Godot;
using Godot.Collections;

public partial class MinableResource : StaticBody2D
{
	private AnimationPlayer _animationPlayer;
	private Area2D _hitArea;
	[Export] private ResourceTypes _type;
    [Export] private int _level;
    [Export] private int _health;
    private int _maxHealth;
    [Export] private Array<Item> _drops;
    private CpuParticles2D _particles;
    
    public enum ResourceTypes { Wood, Stone }
    public ResourceTypes Type => _type;
    public IInteractable.InteractableTypes InteractableType => IInteractable.InteractableTypes.Resource;

    public override void _Ready()
    {
		_hitArea = GetNode<Area2D>("HitArea");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _particles = GetNode<CpuParticles2D>("Particles");
        _maxHealth = _health;
        Array<Item> newArr = new Array<Item>();
        foreach (Item drop in _drops)
        {
            if (drop.ItemData == null)
            {
                continue;
            }
            newArr.Add(new Item(drop.ItemData, drop.Amount));
        }
        _drops = newArr;
    }

    public Array<Item> HitWith(Tool tool)
    {
        if (tool == null){
            return null;
        }

        switch (tool.Type, _type){
            case (Tool.ToolTypes.Axe, ResourceTypes.Wood):
                int damage = (tool.Level + 1) - _level;
                if(damage > 0)
                {
                    Array<Item> _currentDrops = new Array<Item>();
                    _health -= damage;
                    GD.Print("Health: " + _health);
                    if(_health > 0)
                    {
                        for (int i = 0; i < _drops.Count; i++)
                        {
                            int amount = (int)Mathf.Floor((_drops[i].Amount / _maxHealth) * damage);
                            amount = Mathf.Max(amount, 1);
                            if(amount < _drops[i].Amount)
                            {
                                _currentDrops.Add(new Item(_drops[i].ItemData, amount));
                                _drops[i].Remove(amount);
                            }
                        }
                        _particles.Emitting = true;
                        _animationPlayer.Play("Hit");
                    }
                    else {
                        
                        foreach (Item drop in _drops)
                        {
                            _currentDrops.Add(new Item(drop.ItemData, drop.Amount));
                            _drops.Remove(drop);
                        }
                        _particles.Emitting = true;
                        _animationPlayer.Play("Destroy");
                        QueueFree();
                    }
                    return _currentDrops;
                }
                return null;
            case (Tool.ToolTypes.Pickaxe, ResourceTypes.Stone):
                return null;
        }
        return null;
    }

    public void EndInteraction() {}
}
