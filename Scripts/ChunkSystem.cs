using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class ChunkSystem : Node2D
{
    int _chunkWidth = 16;
    int _chunkHeight = 16;
    int _chunkCellSize = 16;
	List<Vector2> loadedChunks;
    Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    public override void _Ready()
    {
        if(ReferenceCenter.ChunkSystem == null){
            ReferenceCenter.ChunkSystem = this;
        }
        else if(ReferenceCenter.ChunkSystem != this){
            GD.PrintErr("Cant be more than one ChunkSystem");
            QueueFree();
        }

        Vector2 newChunkPos;
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                newChunkPos = new Vector2(x * (_chunkWidth * _chunkCellSize), y * (_chunkHeight * _chunkCellSize));
                Chunk newChunk = new Chunk(_chunkWidth, _chunkHeight, _chunkCellSize, newChunkPos);
                chunks.Add(new Vector2(x, y), newChunk);
                newChunk.Name = "Chunk " + x + ", " + y;
                newChunk.YSortEnabled = true;
                AddChild(newChunk);
            }
        }
    }

    public bool TryBuild(Vector2 worldPos, Blueprint blueprint, Blueprint.Direction direction)
	{
        Chunk chunk = GetChunkAtWorldPosition(worldPos, out int x, out int y);

        List<Vector2> gridPosList = blueprint.GetGridPositionList(x, y, direction);
        foreach (Vector2 pos in gridPosList)
        {
            if(!chunk.CanBuild(Mathf.FloorToInt(pos.X), Mathf.FloorToInt(pos.Y))){
                return false;
            }
        }

        Building building = Building.Build(blueprint, chunk, new Vector2(x, y) * _chunkCellSize);
        foreach (Vector2 pos in gridPosList){
            chunk.SetCell(Mathf.FloorToInt(pos.X), Mathf.FloorToInt(pos.Y), building);
        }
        chunk.QueueRedraw();

        return true;
	}

    public bool CanDestroy(Vector2 worldPos){
        Chunk chunk = GetChunkAtWorldPosition(worldPos, out int x, out int y);
        Building building = chunk.GetCell(x, y);

        if(building == null){
            return false;
        }

        return true;
    }

    public Blueprint Destroy(Vector2 worldPos)
	{
        if(!CanDestroy(worldPos)){
            return null;
        }

        Chunk chunk = GetChunkAtWorldPosition(worldPos, out int x, out int y);
        Building building = chunk.GetCell(x, y);

        GetChunkAtWorldPosition(building.Position, out int buildingX, out int buildingY);
        Vector2 buildingOriginOffset = building.Blueprint.GetRotationOriginOffsetDirection(building.Direction);
        List<Vector2> gridPosList = building.Blueprint.GetGridPositionList(buildingX - Mathf.FloorToInt(buildingOriginOffset.X), buildingY - Mathf.FloorToInt(buildingOriginOffset.Y), building.Direction);
        chunk.GetCell(Mathf.FloorToInt(gridPosList[0].X), Mathf.FloorToInt(gridPosList[0].Y)).QueueFree();
        foreach (Vector2 pos in gridPosList)
        {
            chunk.SetCell(Mathf.FloorToInt(pos.X), Mathf.FloorToInt(pos.Y), null);
        }
        chunk.QueueRedraw();

        return building.Blueprint;
	}

    public Chunk GetChunkAtWorldPosition(Vector2 worldPos, out int innerX, out int innerY){
        int x = Mathf.FloorToInt((worldPos / _chunkCellSize).X / _chunkCellSize);
        int y = Mathf.FloorToInt((worldPos / _chunkCellSize).Y / _chunkCellSize);
        Chunk chunk = chunks.GetValueOrDefault(new Vector2(x, y));
        innerX = Mathf.FloorToInt((worldPos - chunk.Position).X / _chunkCellSize);
        innerY = Mathf.FloorToInt((worldPos - chunk.Position).Y / _chunkCellSize);

        // GD.Print("Chunk at " + x + ", " + y);
        // GD.Print("Cell at " + innerX + ", " + innerY);
        // GD.Print();
        return chunk;
    }
}
