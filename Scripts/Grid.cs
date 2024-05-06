using Godot;
using System;
using System.Diagnostics;

public partial class Grid<TGridObject>
{
	[Export] protected int _width;
	[Export] protected int _height;
	[Export] protected int _cellSize;
	protected TGridObject[,] gridArray;

	[Signal] public delegate void GridCellUpdatedEventHandler(int x, int y);

	public int Width => _width;
	public int Height => _height;
	public int CellSize => _cellSize;
	public TGridObject[,] GridArray => gridArray;

	public Grid(int width, int height, int cellSize){
		_width = width;
		_height = height;
		_cellSize = cellSize;

		gridArray = new TGridObject[width, height];
	}

	public void SetCell(int x, int y, TGridObject gridObject){
		if(x > _width - 1 || x < 0 || y > _height - 1 || y < 0){
			return;
		}

		gridArray[x, y] = gridObject;
	}

	public TGridObject GetCell(int x, int y){
		if(x > _width - 1 || x < 0 || y > _height - 1 || y < 0){
			return default;
		}

		return gridArray[x, y];
	}
}
