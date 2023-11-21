using UnityEngine;

public class BoardObject : MonoBehaviour
{
    public bool Gravity { get; set; }
    public Coordinate Position { get; set; }

    public BoardObject(bool gravity, int row, int column)
    {
        Position = new Coordinate();
        Gravity = true;
        Position.Row = row;
        Position.Column = column;
    }

    public BoardObject()
    {
        Position = new Coordinate();
        Gravity = true;
        Position.Row = 0;
        Position.Column = 0;
    }
}