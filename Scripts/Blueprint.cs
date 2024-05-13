using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Blueprint : ItemData, IProcessAction, IAction
{
	[Export] private PackedScene _placedObject;
	[Export] private PackedScene _ghostObject;
	[Export] private int _width = 1;
	[Export] private int _height = 1;
	[Export] private int _cellSize = 16;
	[Export] private int _buildingDurability = 10;
	private Direction _currentDirection;

	public PackedScene PlacedObject => _placedObject;
	public int Width => _width;
	public int Height => _height;
	public Direction CurrentDirection => _currentDirection;
	public int BuildingDurability => _buildingDurability;
	public enum Direction
	{
		Down,
		Left,
		Up,
		Right
	}

	public Direction GetNextDirection(Direction direction)
	{
		return direction switch
		{
			Direction.Down => Direction.Left,
			Direction.Left => Direction.Up,
			Direction.Up => Direction.Right,
			Direction.Right => Direction.Down,
			_ => Direction.Down,
		};
	}

	public Direction GetPrevDirection(Direction direction)
	{
		return direction switch
		{
			Direction.Down => Direction.Right,
			Direction.Left => Direction.Down,
			Direction.Up => Direction.Left,
			Direction.Right => Direction.Up,
			_ => Direction.Down,
		};
	}

	public int GetRotationAngle(Direction direction)
	{
		return direction switch
		{
			Direction.Left => 90,
			Direction.Up => 180,
			Direction.Right => 270,
			_ => 0,
		};
	}

	public Vector2 GetRotationOriginOffsetDirection(Direction direction)
	{
		return direction switch
		{
			Direction.Left => new Vector2(1, 0),
			Direction.Up => new Vector2(1, 1),
			Direction.Right => new Vector2(0, 1),
			_ => Vector2.Zero,
		};
	}

	public Vector2 GetRotationOriginOffset(Direction direction)
	{
		return GetRotationOriginOffsetDirection(direction) * 16;
	}

	public List<Vector2> GetGridPositionList(int originX, int originY, Direction direction)
	{
		List<Vector2> posList = new List<Vector2>();

		Vector2 originOffset = GetRotationOriginOffsetDirection(direction);
		switch (direction)
		{
			default:
			case Direction.Down:
				for (int x = 0; x < _width; x++)
				{
					for (int y = 0; y < _height; y++)
					{
						posList.Add(new Vector2(x + originX, y + originY) + originOffset);
					}
				}
				break;
			case Direction.Left:
				for (int x = -_height; x < 0; x++)
				{
					for (int y = 0; y < _width; y++)
					{
						posList.Add(new Vector2(x + originX, y + originY) + originOffset);
					}
				}
				break;
			case Direction.Up:
				for (int x = -_width; x < 0; x++)
				{
					for (int y = -_height; y < 0; y++)
					{
						posList.Add(new Vector2(x + originX, y + originY) + originOffset);
					}
				}
				break;
			case Direction.Right:
				for (int x = 0; x < _height; x++)
				{
					for (int y = -_width; y < 0; y++)
					{
						posList.Add(new Vector2(x + originX, y + originY) + originOffset);
					}
				}
				break;
		}

		return posList;
	}

	public void Action(Item self, Node2D caller, Vector2 target)
	{
		if (ReferenceCenter.ChunkSystem.TryBuild(target, this, CurrentDirection))
		{
			self.Remove(1);
		}
	}

	public void StartProcessAction()
	{
		_currentDirection = Direction.Down;
	}

	public void ProcessAction()
	{
		if (Input.IsActionJustReleased("RotateClockwise"))
		{
			_currentDirection = GetNextDirection(CurrentDirection);
		}
		else if (Input.IsActionJustReleased("RotateCounterClockwise"))
		{
			_currentDirection = GetPrevDirection(CurrentDirection);
		}
	}

	public void EndProcessAction() { }
}
