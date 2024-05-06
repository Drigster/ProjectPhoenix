using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass]
public partial class Chunk : Node2D
{
	Grid<Building> buildingGrid;

	public Chunk(int width, int height, int cellSize, Vector2 worldPos){
		buildingGrid = new Grid<Building>(width, height, cellSize);
		Position = worldPos;
	}

	public void SetCell(int x, int y, Building blueprint){
		buildingGrid.SetCell(x, y, blueprint);
	}

	public Building GetCell(int x, int y){
		return buildingGrid.GetCell(x, y);
	}

	public bool CanBuild(int x, int y){
		return GetCell(x, y) == null;
	}

    public void _draw(){
		if(buildingGrid == null){
			return;
		}
		for (int x = 0; x < buildingGrid.GridArray.GetLength(0); x++)
		{
			for (int y = 0; y < buildingGrid.GridArray.GetLength(1); y++)
			{
				Vector2 cellPos = new Vector2(buildingGrid.CellSize * x, buildingGrid.CellSize * y);

				DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + new Vector2(0, buildingGrid.CellSize) * .5f, x + ", " + y, HorizontalAlignment.Center, buildingGrid.CellSize, buildingGrid.CellSize / 3);
				if(buildingGrid.GridArray[x, y] != null){
                    DrawString(GetTree().Root.GetThemeDefaultFont(), cellPos + new Vector2(0, buildingGrid.CellSize) * .5f + new Vector2(0, buildingGrid.CellSize / 3), buildingGrid.GridArray[x, y].ToString(), HorizontalAlignment.Center, buildingGrid.CellSize, buildingGrid.CellSize / 3);
				}
				DrawLine(cellPos, cellPos + Vector2.Down * buildingGrid.CellSize, Colors.White);
				DrawLine(cellPos, cellPos + Vector2.Right * buildingGrid.CellSize, Colors.White);
			}
		}
		DrawLine(new Vector2(buildingGrid.GridArray.GetLength(0), 0) * buildingGrid.CellSize, new Vector2(buildingGrid.GridArray.GetLength(0), buildingGrid.GridArray.GetLength(1)) * buildingGrid.CellSize, Colors.White);
		DrawLine(new Vector2(0, buildingGrid.GridArray.GetLength(1)) * buildingGrid.CellSize, new Vector2(buildingGrid.GridArray.GetLength(0), buildingGrid.GridArray.GetLength(1)) * buildingGrid.CellSize, Colors.White);
	}
}
