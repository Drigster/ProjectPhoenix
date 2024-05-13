using Godot;

[GlobalClass]
public partial class Chunk : Node2D
{
	private Grid<Building> _buildingGrid;

	public Chunk(int width, int height, int cellSize, Vector2 worldPos)
	{
		_buildingGrid = new Grid<Building>(width, height, cellSize);
		Position = worldPos;
	}

	public void SetCell(int x, int y, Building blueprint)
	{
		_buildingGrid.SetCell(x, y, blueprint);
	}

	public Building GetCell(int x, int y)
	{
		return _buildingGrid.GetCell(x, y);
	}

	public bool CanBuild(int x, int y)
	{
		return GetCell(x, y) == null;
	}

	public void _draw()
	{
		if (_buildingGrid == null)
		{
			return;
		}
		for (int x = 0; x < _buildingGrid.GridArray.GetLength(0); x++)
		{
			for (int y = 0; y < _buildingGrid.GridArray.GetLength(1); y++)
			{
				Vector2 cellPos = new Vector2(_buildingGrid.CellSize * x, _buildingGrid.CellSize * y);

				DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + (new Vector2(0, _buildingGrid.CellSize) * .5f), x + ", " + y, HorizontalAlignment.Center, _buildingGrid.CellSize, _buildingGrid.CellSize / 3);
				if (_buildingGrid.GridArray[x, y] != null)
				{
					DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + (new Vector2(0, _buildingGrid.CellSize) * .5f) + new Vector2(0, _buildingGrid.CellSize / 3), _buildingGrid.GridArray[x, y].ToString(), HorizontalAlignment.Center, _buildingGrid.CellSize, _buildingGrid.CellSize / 3);
				}
				DrawLine(cellPos, cellPos + (Vector2.Down * _buildingGrid.CellSize), Colors.White);
				DrawLine(cellPos, cellPos + (Vector2.Right * _buildingGrid.CellSize), Colors.White);
			}
		}
		DrawLine(new Vector2(_buildingGrid.GridArray.GetLength(0), 0) * _buildingGrid.CellSize, new Vector2(_buildingGrid.GridArray.GetLength(0), _buildingGrid.GridArray.GetLength(1)) * _buildingGrid.CellSize, Colors.White);
		DrawLine(new Vector2(0, _buildingGrid.GridArray.GetLength(1)) * _buildingGrid.CellSize, new Vector2(_buildingGrid.GridArray.GetLength(0), _buildingGrid.GridArray.GetLength(1)) * _buildingGrid.CellSize, Colors.White);
	}
}
