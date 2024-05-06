using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

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
    public enum Direction {
        Down,
        Left,
        Up,
        Right
    }
    
    public Direction GetNextDirection(Direction direction){
        switch(direction){
            case Direction.Down: return Direction.Left;
            case Direction.Left: return Direction.Up;
            case Direction.Up: return Direction.Right;
            case Direction.Right: return Direction.Down;
            default: return Direction.Down;
        }
    }

    public Direction GetPrevDirection(Direction direction){
        switch(direction){
            case Direction.Down: return Direction.Right;
            case Direction.Left: return Direction.Down;
            case Direction.Up: return Direction.Left;
            case Direction.Right: return Direction.Up;
            default: return Direction.Down;
        }
    }

    public int GetRotationAngle(Direction direction){
        switch(direction){
            default:
            case Direction.Down: return 0;
            case Direction.Left: return 90;
            case Direction.Up: return 180;
            case Direction.Right: return 270;
        }
    }

    public Vector2 GetRotationOriginOffsetDirection(Direction direction){
        switch(direction){
            default:
            case Direction.Down: return Vector2.Zero;
            case Direction.Left: return new Vector2(1, 0);
            case Direction.Up: return new Vector2(1, 1);
            case Direction.Right: return new Vector2(0, 1);
        }
    }

    public Vector2 GetRotationOriginOffset(Direction direction){
        return GetRotationOriginOffsetDirection(direction) * 16;
    }

    public List<Vector2> GetGridPositionList(int originX, int originY, Direction direction){
        List<Vector2> posList = new List<Vector2>();

        Vector2 originOffset = GetRotationOriginOffsetDirection(direction);
        switch(direction){
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
        if(ReferenceCenter.ChunkSystem.TryBuild(target, this, CurrentDirection)){
            self.Remove(1);
        }
    }

    public void StartProcessAction()
    {
        _currentDirection = Direction.Down;
    }

    public void ProcessAction()
    {
        if(Input.IsActionJustReleased("RotateClockwise")){
            _currentDirection = GetNextDirection(CurrentDirection);
        }
        else if(Input.IsActionJustReleased("RotateCounterClockwise")){
            _currentDirection = GetPrevDirection(CurrentDirection);
        }
    }

    public void EndProcessAction() { }
}
