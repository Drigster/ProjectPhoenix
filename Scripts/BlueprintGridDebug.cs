using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;

[GlobalClass]
[Tool]
public partial class BlueprintGridDebug : Node2D
{
	private Grid<Building> _grid;

    public override void _Process(double delta)
    {
		QueueRedraw();
    }

    public void SetGrid(Grid<Building> grid)
    {
		_grid = grid;
    }

    public void _draw(){
		if(_grid == null){
			return;
		}
		for (int x = 0; x < _grid.GridArray.GetLength(0); x++)
		{
			for (int y = 0; y < _grid.GridArray.GetLength(1); y++)
			{
				Vector2 cellPos = new Vector2(_grid.CellSize * x, _grid.CellSize * y) + Position;

				DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + new Vector2(0, _grid.CellSize) * .5f, x + ", " + y, HorizontalAlignment.Center, _grid.CellSize, _grid.CellSize / 3);
				if(_grid.GridArray[x, y] != null){
                    DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + new Vector2(0, _grid.CellSize) * .5f + new Vector2(0, _grid.CellSize / 3), _grid.GridArray[x, y].ToString(), HorizontalAlignment.Center, _grid.CellSize, _grid.CellSize / 3);
				}
				DrawLine(cellPos, cellPos + Vector2.Down * _grid.CellSize, Colors.White);
				DrawLine(cellPos, cellPos + Vector2.Right * _grid.CellSize, Colors.White);
			}
		}
		DrawLine(Position + new Vector2(_grid.GridArray.GetLength(0), 0) * _grid.CellSize, Position + new Vector2(_grid.GridArray.GetLength(0), _grid.GridArray.GetLength(1)) * _grid.CellSize, Colors.White);
		DrawLine(Position + new Vector2(0, _grid.GridArray.GetLength(1)) * _grid.CellSize, Position + new Vector2(_grid.GridArray.GetLength(0), _grid.GridArray.GetLength(1)) * _grid.CellSize, Colors.White);
	}
}
