using UnityEngine;

public class Bonus : BoardObject
{
    public BonusType BonusType { get; set; }
    public Bonus()
    {
        Position = new Coordinate();
        BonusType = BonusType.rocket;
        Gravity = true;
        Position.Row = 0;
        Position.Column = 0;
    }

    public Bonus(BonusType bonusType, int row, int column)
    {
        Position = new Coordinate();
        BonusType = bonusType;
        Gravity = true;
        Position.Row = row;
        Position.Column = column;
    }
}