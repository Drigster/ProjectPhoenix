using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InteractiveTileMap : TileMap
{
	[Export] public Dictionary<int, PackedScene> TileData;

	public async override void _Ready()
	{
		await ToSignal(GetTree(), "process_frame");
		ReplaceTilesWithScenes(TileData);
	}

	public void ReplaceTilesWithScenes(Dictionary<int, PackedScene> scenes)
	{
		for (int layerId = 0; layerId < GetLayersCount(); layerId++)
		{
			foreach (Vector2I tilePos in GetUsedCellsById(layerId))
			{
				TileData tileData = GetCellTileData(layerId, tilePos);
				int interactableType = (int)tileData.GetCustomData("InteractableType");
				if(interactableType != 0){
					if(scenes.ContainsKey(interactableType)){
						ReplaceTileWithObject(layerId, tilePos, scenes[interactableType], GetTree().CurrentScene);
					}
				}
			}
		}
	}

    private void ReplaceTileWithObject(int layerId, Vector2I tilePos, PackedScene packedScene, Node parentScene)
    {
		SetCell(layerId, tilePos, -1);
		Node2D tileObject = (Node2D)packedScene.Instantiate();
		parentScene.AddChild(tileObject);
		tileObject.Position = MapToLocal(tilePos);
    }
}
