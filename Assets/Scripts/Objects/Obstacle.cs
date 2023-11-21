using UnityEngine;
using UnityEngine.UIElements;

public class Obstacle : BoardObject
{
    public ObstacleType ObstacleType { get; set; }
    public Obstacle()
    {
        Position = new Coordinate();
        ObstacleType = ObstacleType.stone;
        Gravity = true;
        Position.Row = 0;
        Position.Column = 0;
    }

    public Obstacle(ObstacleType obstacleType, int row, int column)
    {
        Position = new Coordinate();
        ObstacleType = obstacleType;
        if (obstacleType == ObstacleType.stone || obstacleType == ObstacleType.reinforcedStone)
        {
            Gravity = true;
        }
        else
        {
            Gravity = false;
        }

        Position.Row = row;
        Position.Column = column;
    }
}