interface IFactory
{
    Token CreateToken(TokenType type);
    Obstacle CreateObstacle(ObstacleType type);
    Bonus CreateBonus(BonusType type);
}