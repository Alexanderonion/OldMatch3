using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using static UnityEditor.Progress;

public class Board : MonoBehaviour
{
    private LevelManager _levelManager;
    public const int rows = 10;
    public const int rowsGameArea = 5;
    public const int columns = 5;
    public float moveTimeObject = 1f;
    private Vector3[,] _objectPositionsOnField;
    private BoardObject[,] _objectOnField;
    public static List<Coordinate> _destroyObjectInPositionAfterMatch;
    public static List<Coordinate> _listOfNullObjectsPosition;
    public static TokenType _chainTokenType;
    [SerializeField] private CreateBoardObject _factory;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UnityEngine.UI.Slider _scoreSlider;
    private System.Random rnd;
    [SerializeField] private GameObject levelsButtonsGroup;
    private int _countTokenInChainForCreateRocket;
    private int _countTokenInChainForCreateBomb;
    bool _objectPositionIsOccuped;
    bool _obstacleDestroyed;
    private int _listOfNullObjectsPositionIndex;

    public static LineRenderer _lineBetweenToken;

    private void Awake()
    {
        _objectPositionsOnField = new Vector3[rows, columns];
        _objectOnField = new BoardObject[rows, columns];
        rnd = new System.Random();
        _objectPositionsOnField = TilesLoadInArray(ref _objectPositionsOnField);
    }

    private void Start()
    {
        _obstacleDestroyed = false;
        _listOfNullObjectsPositionIndex = 0;
        _countTokenInChainForCreateRocket = 5;
        _countTokenInChainForCreateBomb = 8;
        _destroyObjectInPositionAfterMatch = new List<Coordinate>();
        _listOfNullObjectsPosition= new List<Coordinate>();
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        _lineBetweenToken = GameObject.Find("ConnectionLineOfToken").GetComponent<LineRenderer>();
    }

    public void MoveField()
    {
        _listOfNullObjectsPosition.Sort((x, y) => x.Column.CompareTo(y.Column));

        for (; _listOfNullObjectsPositionIndex < _listOfNullObjectsPosition.Count; _listOfNullObjectsPositionIndex++)
        {
            _objectPositionIsOccuped = false;
            DropObject(_listOfNullObjectsPosition[_listOfNullObjectsPositionIndex]);

            if (_objectPositionIsOccuped)
            {
                _listOfNullObjectsPosition[_listOfNullObjectsPositionIndex] = null;
            }
        }

        while (_obstacleDestroyed && _listOfNullObjectsPosition.Count > 0)
        {
            
            _listOfNullObjectsPosition.RemoveAll(item => item == null);
            _listOfNullObjectsPositionIndex = 0;

            for (; _listOfNullObjectsPositionIndex < _listOfNullObjectsPosition.Count; _listOfNullObjectsPositionIndex++)
            {
                _obstacleDestroyed = false;
                _objectPositionIsOccuped = false;
                DropObject(_listOfNullObjectsPosition[_listOfNullObjectsPositionIndex]);

                if (_objectPositionIsOccuped)
                {
                    _listOfNullObjectsPosition[_listOfNullObjectsPositionIndex] = null;
                }
            }
            
        }

        _destroyObjectInPositionAfterMatch.Clear();
        _levelManager.IncrementActionsCount();
        _listOfNullObjectsPosition.RemoveAll(item => item == null);
        LevelManager._stopInteraction = false;
        _listOfNullObjectsPositionIndex = 0;
    }
    
    private void DropObject(Coordinate coordinateOfNullObject)
    {
        if (_objectOnField[coordinateOfNullObject.Row, coordinateOfNullObject.Column] != null)
        {
            _objectPositionIsOccuped = true;
            return;
        }

        if (coordinateOfNullObject.Row == 0)
        {
            _objectPositionIsOccuped = true;
            CreateRandomTokenOnField(coordinateOfNullObject);
            return;
        }

        int movableObjectRow = coordinateOfNullObject.Row - 1;

        while (_objectOnField[movableObjectRow, coordinateOfNullObject.Column] == null)
        {
            movableObjectRow--;
        }

        BoardObject movableObject = _objectOnField[movableObjectRow, coordinateOfNullObject.Column];

        for (int nextRow = movableObject.Position.Row + 1; nextRow <= coordinateOfNullObject.Row; nextRow++)
        {
            if (movableObject.Gravity)
            {
                if (nextRow > rowsGameArea)
                {
                    _objectPositionIsOccuped = true;
                    ChangeObjectPosition(movableObject, nextRow, coordinateOfNullObject.Column);
                    ChangeObjectAddress(new Coordinate(movableObject.Position.Row + 1, movableObject.Position.Column), new Coordinate(movableObject.Position.Row, movableObject.Position.Column));
                    AddNullCoordinateInList(new Coordinate(movableObjectRow, movableObject.Position.Column));
                }
                else
                {
                    _objectPositionIsOccuped = true;
                    ChangeObjectPosition(movableObject, nextRow, coordinateOfNullObject.Column);
                    ChangeObjectAddress(new Coordinate(movableObject.Position.Row + 1, movableObject.Position.Column), new Coordinate(movableObject.Position.Row, movableObject.Position.Column));
                    AddNullCoordinateInList(new Coordinate(movableObjectRow, coordinateOfNullObject.Column));
                }
            }
            else
            {
                if (coordinateOfNullObject.Row - movableObjectRow > 1)
                {
                    TakeDiagonileObject(coordinateOfNullObject);
                }
                else
                {
                    TakeDiagonileObject(new Coordinate(movableObjectRow + 1, coordinateOfNullObject.Column));
                }
            }
        }

        if (_objectOnField[0, movableObject.Position.Column] == null)
        {
            _objectPositionIsOccuped = true;
            CreateRandomTokenOnField(new Coordinate(0, movableObject.Position.Column));
            return;
        }
    }

    private IEnumerator MoveObject(BoardObject movableObject, int finishRow, int finish—olumn)
    {
        Vector3 startPosition = movableObject.transform.position;
        Vector3 finalPosition = _objectPositionsOnField[finishRow, finish—olumn];
        finalPosition.z -= 1;

        float i = 0;
        float rate = (1 / moveTimeObject);

        while (movableObject.transform.position != finalPosition)
        {
            i += Time.deltaTime * rate;
            movableObject.transform.position = Vector3.Lerp(startPosition, finalPosition, i);
            yield return null;
        }
    }

    private void ChangeObjectPosition(BoardObject movableObject, int nextRow, int nextColumn)
    {
        Vector3 nextPosition = _objectPositionsOnField[nextRow, nextColumn];
        nextPosition.z -= 1;
        movableObject.transform.position = nextPosition;
    }

    private void ChangeObjectAddress(Coordinate newObjectCoordinate, Coordinate oldObjectCoordinate)
    {
        _objectOnField[newObjectCoordinate.Row, newObjectCoordinate.Column] = _objectOnField[oldObjectCoordinate.Row, oldObjectCoordinate.Column].GetComponent<BoardObject>();
        _objectOnField[newObjectCoordinate.Row, newObjectCoordinate.Column].Position.Column = newObjectCoordinate.Column;
        _objectOnField[newObjectCoordinate.Row, newObjectCoordinate.Column].Position.Row = newObjectCoordinate.Row;
        _objectOnField[oldObjectCoordinate.Row, oldObjectCoordinate.Column] = null;
    }

    private void AddNullCoordinateInList(Coordinate nullObjectCoordinate)
    {
        if (_listOfNullObjectsPositionIndex + 1 > _listOfNullObjectsPosition.Count)
        {
            _listOfNullObjectsPosition.Add(nullObjectCoordinate);
        }
        else
        {
            _listOfNullObjectsPosition.Insert(_listOfNullObjectsPositionIndex + 1, nullObjectCoordinate);
        }
    }

    private void TakeDiagonileObject(Coordinate coordinateOfNullObject)
    {
        if (_objectOnField[coordinateOfNullObject.Row, coordinateOfNullObject.Column] != null) { return; }

        BoardObject rightUpperObject = null;
        BoardObject leftUpperObject = null;

        if (ObjectInGameBoard(new Coordinate(coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column + 1)))
        {
            rightUpperObject = _objectOnField[coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column + 1];
        }

        if (ObjectInGameBoard(new Coordinate(coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column - 1)))
        {
            leftUpperObject = _objectOnField[coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column - 1];
        }
        
        if (leftUpperObject != null && leftUpperObject.Gravity)
        {
            _objectPositionIsOccuped = true;
            ChangeObjectPosition(leftUpperObject, coordinateOfNullObject.Row, coordinateOfNullObject.Column);
            ChangeObjectAddress(new Coordinate(leftUpperObject.Position.Row + 1, leftUpperObject.Position.Column + 1), new Coordinate(leftUpperObject.Position.Row, leftUpperObject.Position.Column));
            AddNullCoordinateInList(new Coordinate(coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column - 1));
        }
        else if (rightUpperObject != null && rightUpperObject.Gravity)
        {
            _objectPositionIsOccuped = true;
            ChangeObjectPosition(rightUpperObject, coordinateOfNullObject.Row, coordinateOfNullObject.Column);
            ChangeObjectAddress(new Coordinate(rightUpperObject.Position.Row + 1, rightUpperObject.Position.Column - 1), new Coordinate(rightUpperObject.Position.Row, rightUpperObject.Position.Column));
            AddNullCoordinateInList(new Coordinate(coordinateOfNullObject.Row - 1, coordinateOfNullObject.Column + 1));
        }
    }

    private bool ObjectInGameBoard(Coordinate position)
    {
        if (position.Row < rowsGameArea || position.Row >= rows)
        {
            return false;
        }

        if (position.Column < 0 || position.Column >= columns)
        {
            return false;
        }

        return true;
    }

    private Vector3[,] TilesLoadInArray(ref Vector3[,] arrayTokensPosition)
    {
        for (int row = 0, tilesCounter = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++, tilesCounter++)
            {
                arrayTokensPosition[row, column] = gameObject.transform.GetChild(tilesCounter).position;
            }
        }

        return arrayTokensPosition;
    }

    public bool IfTokenInChain(List<Coordinate> objectPositions)
    {
        for (int i = 0; i < objectPositions.Count; i++)
        {
            if (_objectOnField[objectPositions[i].Row, objectPositions[i].Column] is Token)
            {
                return true;
            }
        }

        return false;
    }

    public void ResetLevel()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Destroy(_objectOnField[i, j].gameObject);
                _objectOnField[i, j] = null;
            }
        }

        _levelManager.LoadLevel();
        _levelManager.LoadGoals();
        _scoreManager.ResetScore();
    }

    #region Destroy Object region
    public void RemoveAnObjectFromTheListOfDestroyObjects()
    {
        ReplaceTokenWithBonusInChain(ref _destroyObjectInPositionAfterMatch);

        for (int i = 0; i < _destroyObjectInPositionAfterMatch.Count; i++)
        {
            DestroyBoardObject(_destroyObjectInPositionAfterMatch[i]);
        }
    }

    private void ReplaceTokenWithBonusInChain(ref List<Coordinate> destroyList)
    {
        List<int> withdrawIndexFromDestroyList = new List<int>();

        for (int i = 0, countTokensOfChain = 0; i < destroyList.Count; i++)
        {
            if (!(_objectOnField[destroyList[i].Row, destroyList[i].Column] is Token))
            {
                continue;
            }
            else
            {
                countTokensOfChain++;

                if (countTokensOfChain % _countTokenInChainForCreateRocket == 0 && countTokensOfChain % _countTokenInChainForCreateBomb != 0)
                {
                    ReplaceTokenWithBonus(destroyList[i], BonusType.rocket);
                    withdrawIndexFromDestroyList.Add(i);
                }
                else if (countTokensOfChain % _countTokenInChainForCreateBomb == 0)
                {
                    ReplaceTokenWithBonus(destroyList[i], BonusType.bomb);
                    withdrawIndexFromDestroyList.Add(i);
                }
            }
        }

        for (int i = withdrawIndexFromDestroyList.Count; i > 0; i--)
        {
            destroyList.RemoveAt(withdrawIndexFromDestroyList[i - 1]);
        }
    }

    private void ReplaceTokenWithBonus(Coordinate position, BonusType bonustype)
    {
        DestroyBoardObject(position);
        _objectOnField[position.Row, position.Column] = null;
        CreateBonusOnField(bonustype, position);
        _listOfNullObjectsPosition.Remove(position);
    }

    private void DestroyBoardObject(Coordinate boardObjectPosition)
    {
        if (_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column] is Token)
        {
            DestroyToken((Token)_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column]);
            SoundManager.audioSources[1].PlayOneShot(SoundManager.deleteObjectSound[0]);
        }
        else if (_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column] is Obstacle)
        {
            DestroyObstacle((Obstacle)_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column]);
            SoundManager.audioSources[1].PlayOneShot(SoundManager.deleteObjectSound[0]);
        }
        else if (_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column] is Bonus)
        {
            DestroyBonus((Bonus)_objectOnField[boardObjectPosition.Row, boardObjectPosition.Column]);
        }
    }

    private void DestroyToken(Token tokenObject)
    {
        int distanceOfTokkensDammage = 1;

        if (ObjectInGameBoard(new Coordinate(tokenObject.Position.Row - distanceOfTokkensDammage, tokenObject.Position.Column)) && _objectOnField[tokenObject.Position.Row - distanceOfTokkensDammage, tokenObject.Position.Column] is Obstacle)
        {
            DestroyObstacle((Obstacle)_objectOnField[tokenObject.Position.Row - distanceOfTokkensDammage, tokenObject.Position.Column]);
        }

        if (ObjectInGameBoard(new Coordinate(tokenObject.Position.Row, tokenObject.Position.Column + distanceOfTokkensDammage)) && _objectOnField[tokenObject.Position.Row, tokenObject.Position.Column + distanceOfTokkensDammage] is Obstacle)
        {
            DestroyObstacle((Obstacle)_objectOnField[tokenObject.Position.Row, tokenObject.Position.Column + distanceOfTokkensDammage]);
        }

        if (ObjectInGameBoard(new Coordinate(tokenObject.Position.Row + distanceOfTokkensDammage, tokenObject.Position.Column)) && _objectOnField[tokenObject.Position.Row + distanceOfTokkensDammage, tokenObject.Position.Column] is Obstacle)
        {
            DestroyObstacle((Obstacle)_objectOnField[tokenObject.Position.Row + distanceOfTokkensDammage, tokenObject.Position.Column]);
        }

        if (ObjectInGameBoard(new Coordinate(tokenObject.Position.Row, tokenObject.Position.Column - distanceOfTokkensDammage)) && _objectOnField[tokenObject.Position.Row, tokenObject.Position.Column - distanceOfTokkensDammage] is Obstacle)
        {
            DestroyObstacle((Obstacle)_objectOnField[tokenObject.Position.Row, tokenObject.Position.Column - distanceOfTokkensDammage]);
        }

        IncrementTokenGoal(tokenObject.TokenType, _levelManager._firstGoal);
        IncrementTokenGoal(tokenObject.TokenType, _levelManager._secondGoal);
        IncrementTokenGoal(tokenObject.TokenType, _levelManager._thirdGoal);
        _listOfNullObjectsPosition.Add(tokenObject.Position);
        _objectOnField[tokenObject.Position.Row, tokenObject.Position.Column] = null;
        Destroy(tokenObject.gameObject);
        _scoreManager.AdditionScore(50);
        _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
    }

    private void DestroyObstacle(Obstacle obstacleObject)
    {
        switch (obstacleObject.ObstacleType)
        {
            case ObstacleType.reinforcedStone:
                obstacleObject.ObstacleType = ObstacleType.stone;
                obstacleObject.transform.GetChild(1).gameObject.SetActive(false);
                _scoreManager.AdditionScore(50);
                _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
                break;
            case ObstacleType.reinforcedIce:
                obstacleObject.ObstacleType = ObstacleType.ice;
                obstacleObject.transform.GetChild(1).gameObject.SetActive(false);
                _scoreManager.AdditionScore(50);
                _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
                break;
            default:
                if (!_listOfNullObjectsPosition.Contains(obstacleObject.Position))
                {
                    _listOfNullObjectsPosition.Add(obstacleObject.Position);
                }

                IncrementObstacleGoal(obstacleObject.ObstacleType, _levelManager._firstGoal);
                IncrementObstacleGoal(obstacleObject.ObstacleType, _levelManager._secondGoal);
                IncrementObstacleGoal(obstacleObject.ObstacleType, _levelManager._thirdGoal);

                _objectOnField[obstacleObject.Position.Row, obstacleObject.Position.Column] = null;
                Destroy(obstacleObject.gameObject);
                _scoreManager.AdditionScore(50);
                _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
                break;
        }

        _obstacleDestroyed = true;
    }

    void IncrementTokenGoal(TokenType tokenType, Goal goal)
    {
        if ((int)tokenType == (int)goal.GoalType)
        {
            goal.IncrementGoalsValue();
        }
    }

    void IncrementObstacleGoal(ObstacleType obstacleType, Goal goal)
    {
        if (obstacleType == ObstacleType.ice && goal.GoalType == GoalOfGame.destroyIce)
        {
            goal.IncrementGoalsValue();
        }
        else if (obstacleType == ObstacleType.stone && goal.GoalType == GoalOfGame.destroyStone)
        {
            goal.IncrementGoalsValue();
        }
    }

    public void DestroyBonus(Bonus bonusObject)
    {
        switch (bonusObject.BonusType)
        {
            case BonusType.rocket:
                SoundManager.audioSources[1].PlayOneShot(SoundManager.deleteObjectSound[1]);
                _listOfNullObjectsPosition.Add(bonusObject.Position);
                _objectOnField[bonusObject.Position.Row, bonusObject.Position.Column] = null;
                Destroy(bonusObject.gameObject);
                _scoreManager.AdditionScore(75);
                _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
                DestroyLine(bonusObject.Position.Row);
                break;
            case BonusType.bomb:
                SoundManager.audioSources[1].PlayOneShot(SoundManager.deleteObjectSound[2]);
                _listOfNullObjectsPosition.Add(bonusObject.Position);
                _objectOnField[bonusObject.Position.Row, bonusObject.Position.Column] = null;
                Destroy(bonusObject.gameObject);
                _scoreManager.AdditionScore(100);
                _scoreSlider.GetComponent<ProgressBar>().UpdateRating();
                DestroySquare(bonusObject.Position);
                break;
            default: break;
        }
    }

    private void DestroyLine(int row)
    {
        for (int i = 0; i < columns; i++)
        {
            if (_objectOnField[row, i] != null)
            {
                DestroyBoardObject(_objectOnField[row, i].Position);
            }
        }
    }

    private void DestroySquare(Coordinate position)
    {
        int distanceFromBomb = 1;

        for (int i = -distanceFromBomb; i <= distanceFromBomb; i++)
        {
            for (int j = -distanceFromBomb; j <= distanceFromBomb; j++)
            {
                if (ObjectInGameBoard(new Coordinate(position.Row + i, position.Column + j)) && _objectOnField[position.Row + i, position.Column + j] != null)
                {
                    DestroyBoardObject(_objectOnField[position.Row + i, position.Column + j].Position);
                }
            }
        }
    }
    #endregion

    #region Create Object region
    public void CreateRandomTokenOnField(Coordinate position)
    {
        TokenType type = (TokenType)rnd.Next(1, 6);
        CreateTokenOnField(type, position);
    }

    public void CreateTokenOnField(TokenType tokenType, Coordinate position)
    {
        Token downcastToken = new Token();
        _objectOnField[position.Row, position.Column] = _factory.CreateToken(tokenType);
        downcastToken = (Token)_objectOnField[position.Row, position.Column];
        downcastToken.TokenType = tokenType;
        _objectOnField[position.Row, position.Column].transform.position = _objectPositionsOnField[position.Row, position.Column];
        _objectOnField[position.Row, position.Column].Position.Row = position.Row;
        _objectOnField[position.Row, position.Column].Position.Column = position.Column;
    }

    public void CreateBonusOnField(BonusType bonusType, Coordinate position)
    {
        Bonus downcastBonus = new Bonus();
        _objectOnField[position.Row, position.Column] = _factory.CreateBonus(bonusType);
        downcastBonus = (Bonus)_objectOnField[position.Row, position.Column];
        downcastBonus.BonusType = bonusType;
        _objectOnField[position.Row, position.Column].transform.position = _objectPositionsOnField[position.Row, position.Column];
        _objectOnField[position.Row, position.Column].Position.Row = position.Row;
        _objectOnField[position.Row, position.Column].Position.Column = position.Column;

    }

    public void CreateObstacleOnField(ObstacleType obstacleType, Coordinate position)
    {
        Obstacle downcastObstacle = new Obstacle();
        _objectOnField[position.Row, position.Column] = _factory.CreateObstacle(obstacleType);
        downcastObstacle = (Obstacle)_objectOnField[position.Row, position.Column];
        downcastObstacle.ObstacleType = obstacleType;

        if (obstacleType == ObstacleType.stone || obstacleType == ObstacleType.reinforcedStone)
        {
            _objectOnField[position.Row, position.Column].Gravity = true;
        }
        else
        {
            _objectOnField[position.Row, position.Column].Gravity = false;
        }

        _objectOnField[position.Row, position.Column].transform.position = _objectPositionsOnField[position.Row, position.Column];
        _objectOnField[position.Row, position.Column].Position.Row = position.Row;
        _objectOnField[position.Row, position.Column].Position.Column = position.Column;
    }
    #endregion
}