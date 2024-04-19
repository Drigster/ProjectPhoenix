using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Tool : ItemData, IAction, ISecondaryAction
{
    [Export] private ToolTypes _type;
    [Export] private int _level;
    public enum ToolTypes { Axe, Pickaxe, ToolBox }
    public ToolTypes Type => _type;
    public int Level => _level;

    public void Action(Item self, Node2D caller, Vector2 interationPosition)
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

            if(_type == ToolTypes.Axe || _type == ToolTypes.Pickaxe){
                if(((Area2D)result["collider"]).GetParent() is MinableResource resource)
                {
                    Array<Item> drops = resource.HitWith(this);
                    if(drops != null)
                    {
                        foreach(Item drop in drops)
                        {
                            if(caller is IStorrage storrage){
                                if(storrage.GetInventory().CanAddItems(drop)){
                                    storrage.GetInventory().AddItems(drop);
                                    continue;
                                }
                            }
                            
                            GD.PushWarning("TODO: Drop item on the ground");
                        }
                    }
                }

                if(((Area2D)result["collider"]).GetParent() is Building building){
                    building.Hit(10);
                }
            }
        }
    }

    public void SecondaryAction(Item self, Node2D caller, Vector2 interationPosition)
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

            if(_type == ToolTypes.ToolBox){
                if(((Area2D)result["collider"]).GetParent() is Building building){
                    if(caller is IStorrage storrage){
                        storrage.GetInventory().AddItems(building.DismantleWith(this), 1);
                    }
                    else {
                        // TODO: Drop
                        GD.PushWarning("Drop");
                    }
                }
            }
        }
    }
}
