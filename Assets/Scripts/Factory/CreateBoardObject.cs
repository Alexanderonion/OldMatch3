using UnityEngine;
public class CreateBoardObject : MonoBehaviour, IFactory
{
    #region pole
    [SerializeField] private Token _redTokenPrefab;
    [SerializeField] private Token _greenTokenPrefab;
    [SerializeField] private Token _BlueTokenPrefab;
    [SerializeField] private Token _yellowTokenPrefab;
    [SerializeField] private Token _pinkTokenPrefab;
    [SerializeField] private Obstacle _stone;
    [SerializeField] private Obstacle _reinforcedStone;
    [SerializeField] private Obstacle _ice;
    [SerializeField] private Obstacle _reinforcedIce;
    [SerializeField] private Bonus _rocket;
    [SerializeField] private Bonus _bomb;
    #endregion

    public Token CreateToken(TokenType type)
    {
        switch (type)
        {
            case TokenType.red:
                return Instantiate(_redTokenPrefab, gameObject.transform);
            case TokenType.green:
                return Instantiate(_greenTokenPrefab, gameObject.transform);
            case TokenType.blue:
                return Instantiate(_BlueTokenPrefab, gameObject.transform);
            case TokenType.yellow:
                return Instantiate(_yellowTokenPrefab, gameObject.transform);
            case TokenType.pink:
                return Instantiate(_pinkTokenPrefab, gameObject.transform);
            default: return _redTokenPrefab;
        }
    }

    public Obstacle CreateObstacle(ObstacleType type)
    {
        switch(type)
        { 
            case ObstacleType.stone: 
                return Instantiate(_stone, gameObject.transform);
            case ObstacleType.reinforcedStone: 
                return Instantiate(_reinforcedStone, gameObject.transform);
            case ObstacleType.ice:
                return Instantiate(_ice, gameObject.transform);
            case ObstacleType.reinforcedIce:
                return Instantiate(_reinforcedIce, gameObject.transform);
            default: return _ice;
        }
    }

    public Bonus CreateBonus(BonusType type)
    {
        switch (type)
        {
            case BonusType.rocket:
                return Instantiate(_rocket, gameObject.transform);
            case BonusType.bomb:
                return Instantiate(_bomb, gameObject.transform);
            default: return _rocket;
        }
    }
}