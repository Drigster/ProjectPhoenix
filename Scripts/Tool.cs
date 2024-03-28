using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Tool : ItemData, IAction
{
    [Export] private ToolTypes _type;
    [Export] private int _level;
    public enum ToolTypes { Axe, Pickaxe }
    public ToolTypes Type => _type;
    public int Level => _level;

    public void Action(Node2D caller, Vector2 interationPosition)
    {
        PhysicsPointQueryParameters2D ray = new PhysicsPointQueryParameters2D
        {
            Position = interationPosition,
            CollideWithAreas = true,
            CollideWithBodies = false,
            //CollisionMask = 0b1000000000000000
        };

        Array<Dictionary> results = caller.GetWorld2D().DirectSpaceState.IntersectPoint(ray);
        foreach(Dictionary result in results)
        {
            if((Area2D)result["collider"] == null)
            {
                return;
            }

            if(((Area2D)result["collider"]).GetParent() is not MinableResource resource)
            {
                continue;
            }
            
            Array<Item> drops = resource.HitWith(this);
            if(drops != null)
            {
                foreach(Item drop in drops)
                {
                    if(caller is IStorrage storrage){
                        if(storrage.GetInventory().CanAddItem(drop)){
                            storrage.GetInventory().AddItem(drop);
                            continue;
                        }
                    }
                    
                    GD.PushWarning("TODO: Drop item on the ground");
                }
            }
        }
    }
}
