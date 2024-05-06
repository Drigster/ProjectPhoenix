using Godot;

public partial class ItemsController : Node
{
	[Export] public PackedScene _groundItemScene;

	public override void _Ready()
	{
		ReferenceCenter.ItemsController = this;
	}

	//TODO: Make items fly in random direction after spawning
	public void Spawn(ItemData itemData, int amount, Vector2 position)
	{
		int alreadySpawned = 0;
		do
		{
			int amountToSpawn;
			if (amount > itemData.MaxStack)
			{
				amountToSpawn = itemData.MaxStack;
			}
			else
			{
				amountToSpawn = amount;
			}
			alreadySpawned += amountToSpawn;

			GroundItem groundItem = _groundItemScene.Instantiate<GroundItem>();
			groundItem.Position = position;
			groundItem.SetItem(new Item(itemData, amountToSpawn));

			AddChild(groundItem);
		} while (alreadySpawned < amount);
	}
}
