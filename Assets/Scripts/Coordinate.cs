public class Coordinate
{
    public int Row { get; set; }    
    public int Column { get; set; }

    public Coordinate(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public Coordinate() { }

    //public static Position operator - (Position pos1, Position pos2)
    //{
    //    return new Position(pos1.Row - pos2.Row, pos1.Column - pos2.Column);
    //}
}