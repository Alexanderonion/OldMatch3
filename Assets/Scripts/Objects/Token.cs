using UnityEngine;

public class Token : BoardObject
{    
    public TokenType TokenType { get; set; }
    
    public Token()
    {
        Position = new Coordinate();
        TokenType = TokenType.red;
        Gravity = true;
        Position.Row = 0;
        Position.Column = 0;
    }

    public Token(TokenType tokenType, int row, int column)
    {
        Position = new Coordinate();
        TokenType = tokenType;
        Gravity = true;
        Position.Row = row;
        Position.Column = column;
    }
}