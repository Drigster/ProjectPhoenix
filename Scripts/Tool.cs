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
		foreach (Dictionary result in results)
		{
			if ((Area2D)result["collider"] == null)
			{
				return;
			}

			if (_type is ToolTypes.Axe or ToolTypes.Pickaxe)
			{
				if (((Area2D)result["collider"]).GetParent() is MinableResource resource)
				{
					Array<Item> drops = resource.HitWith(this);
					if (drops != null)
					{
						Array<Node> nodes = caller.FindChildren("*", "IInventorySystem");
						InventorySystem inventorySystem = null;
						if (nodes.Count > 0)
						{
							inventorySystem = ((IInventorySystem)nodes[0]).GetInventory();
						}

						foreach (Item drop in drops)
						{
							if (inventorySystem?.CountAvailableItemSpace(drop.ItemData) >= drop.Amount)
							{
								inventorySystem.AddItems(drop);
								continue;
							}

							GD.PushWarning("TODO: Drop item on the ground");
						}
					}
				}

				if (((Area2D)result["collider"]).GetParent() is Building building)
				{
					building.Hit(10);
				}
			}
			else if (_type == ToolTypes.ToolBox)
			{
				Blueprint blueprint = ReferenceCenter.ChunkSystem.Destroy(interationPosition);
				if (blueprint != null)
				{
					Array<Node> nodes = caller.FindChildren("*", "IInventorySystem");
					InventorySystem inventorySystem = null;
					if (nodes.Count > 0)
					{
						inventorySystem = ((IInventorySystem)nodes[0]).GetInventory();
					}

					inventorySystem?.AddItems(blueprint, 1);
				}
			}
		}
	}

	public void SecondaryAction(Item self, Node2D caller, Vector2 interationPosition)
	{

	}
}
