using UnityEngine;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.IO;
using UnityEngine.UIElements;
using DG.Tweening.Plugins.Core.PathCore;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Board _board;
    public static int _levelNumber;
    public static int _endActionsCount;
    [SerializeField] private TextMeshProUGUI _endActionsCountText;
    public static int _pointsToGoal;
    public static Action _pointsToGoalEnable;
    public Goal _firstGoal;
    public Goal _secondGoal;
    public Goal _thirdGoal;
    private SaveData _saveData;
    public TextAsset jsonFile;
    public static bool _stopInteraction;

    private void Awake()
    {
        _levelNumber = 0;
        _stopInteraction = false;
    }

    public void LoadLevelJson()
    {
        jsonFile = Resources.Load<TextAsset>($"{_levelNumber}");
        _saveData = JsonUtility.FromJson<SaveData>(jsonFile.text);
        _endActionsCount = _saveData._countActionsToEndGame;
        _endActionsCountText.text = $"{_endActionsCount}";

        for (int row = 0; row < Board.rows; row++)
        {
            for (int column = 0; column < Board.columns; column++)
            {
                Coordinate position = new Coordinate(row, column);

                switch (_saveData._field[column + (row * _saveData._column)])
                {
                    case (int)TokenType.red:
                        _board.CreateTokenOnField(TokenType.red, position);
                        break;
                    case (int)TokenType.green:
                        _board.CreateTokenOnField(TokenType.green, position);
                        break;
                    case (int)TokenType.blue:
                        _board.CreateTokenOnField(TokenType.blue, position);
                        break;
                    case (int)TokenType.yellow:
                        _board.CreateTokenOnField(TokenType.yellow, position);
                        break;
                    case (int)TokenType.pink:
                        _board.CreateTokenOnField(TokenType.pink, position);
                        break;
                    case (int)ObstacleType.stone:
                        _board.CreateObstacleOnField(ObstacleType.stone, position);
                        break;
                    case (int)ObstacleType.reinforcedStone:
                        _board.CreateObstacleOnField(ObstacleType.reinforcedStone, position);
                        break;
                    case (int)ObstacleType.ice:
                        _board.CreateObstacleOnField(ObstacleType.ice, position);
                        break;
                    case (int)ObstacleType.reinforcedIce:
                        _board.CreateObstacleOnField(ObstacleType.reinforcedIce, position);
                        break;
                    case (int)BonusType.rocket: _board.CreateBonusOnField(BonusType.rocket, position); break;
                    case (int)BonusType.bomb: _board.CreateBonusOnField(BonusType.bomb, position); break;
                }
            }
        }
    }

    public void LoadGoalsJson()
    {
        _firstGoal.LoadGoalJson(_saveData);
        _secondGoal.LoadGoalJson(_saveData);
        _thirdGoal.LoadGoalJson(_saveData);
    }

    public void LoadGoals()
    {
        _firstGoal.LoadGoal(_levelNumber);
        _secondGoal.LoadGoal(_levelNumber);
        _thirdGoal.LoadGoal(_levelNumber);
    }

    public void LoadLevel()
    {
        _endActionsCount = PlayerPrefs.GetInt("Action count" + _levelNumber);
        _endActionsCountText.text = $"{_endActionsCount}";

        for (int row = 0; row < Board.rows; row++)
        {
            for (int column = 0; column < Board.columns; column++)
            {
                Coordinate position = new Coordinate(row, column);

                switch (PlayerPrefs.GetInt("Object of field[" + row + column + "]" + _levelNumber))
                {
                    case (int)TokenType.red:
                        _board.CreateTokenOnField(TokenType.red, position);
                        break;
                    case (int)TokenType.green:
                        _board.CreateTokenOnField(TokenType.green, position);
                        break;
                    case (int)TokenType.blue:
                        _board.CreateTokenOnField(TokenType.blue, position);
                        break;
                    case (int)TokenType.yellow:
                        _board.CreateTokenOnField(TokenType.yellow, position);
                        break;
                    case (int)TokenType.pink:
                        _board.CreateTokenOnField(TokenType.pink, position);
                        break;
                    case (int)ObstacleType.stone:
                        _board.CreateObstacleOnField(ObstacleType.stone, position);
                        break;
                    case (int)ObstacleType.reinforcedStone:
                        _board.CreateObstacleOnField(ObstacleType.reinforcedStone, position);
                        break;
                    case (int)ObstacleType.ice:
                        _board.CreateObstacleOnField(ObstacleType.ice, position);
                        break;
                    case (int)ObstacleType.reinforcedIce:
                        _board.CreateObstacleOnField(ObstacleType.reinforcedIce, position);
                        break;
                    case (int)BonusType.rocket: _board.CreateBonusOnField(BonusType.rocket, position); break;
                    case (int)BonusType.bomb: _board.CreateBonusOnField(BonusType.bomb, position); break;
                }
            }
        }
    }

    public void CreateRandomLevel()
    {
        for (int column = 0; column < Board.columns; column++)
        {
            _board.CreateRandomTokenOnField(new Coordinate(0, column));
        }
    }

    public void IncrementActionsCount()
    {
        _endActionsCount--;
        _endActionsCountText.text = $"{_endActionsCount}";

        if (_endActionsCount >= 0 && _firstGoal.ReachGoal && _secondGoal.ReachGoal && _thirdGoal.ReachGoal )
        {
            Debug.Log("WIN");
        }
        else if (_endActionsCount <= 0 && !(_firstGoal.ReachGoal && _secondGoal.ReachGoal && _thirdGoal.ReachGoal))
        {
            Debug.Log("LOOSE");
        }
    }
}