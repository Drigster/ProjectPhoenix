using Godot;

public partial class Grid<TGridObject>
{
	[Export] protected int _width;
	[Export] protected int _height;
	[Export] protected int _cellSize;
	protected TGridObject[,] _gridArray;

	[Signal] public delegate void GridCellUpdatedEventHandler(int x, int y);

	public int Width => _width;
	public int Height => _height;
	public int CellSize => _cellSize;
	public TGridObject[,] GridArray => _gridArray;

	public Grid(int width, int height, int cellSize)
	{
		_width = width;
		_height = height;
		_cellSize = cellSize;

		_gridArray = new TGridObject[width, height];
	}

	public void SetCell(int x, int y, TGridObject gridObject)
	{
		if (x > _width - 1 || x < 0 || y > _height - 1 || y < 0)
		{
			return;
		}

		_gridArray[x, y] = gridObject;
	}

	public TGridObject GetCell(int x, int y)
	{
		if (x > _width - 1 || x < 0 || y > _height - 1 || y < 0)
		{
			return default;
		}

		return _gridArray[x, y];
	}
}
