using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using log4net.Core;
using System;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;

public class EditorScript : EditorWindow
{
    public int _countTabsEditor = 2;

    readonly string[] _tabsNames = new string[] { "Load Level", "Level Designer" };
    int _levelNumber = 0;
    
    bool _firstGoalEnabled;
    bool _secondGoalEnabled;
    bool _thirdGoalEnabled;

    int _firstGoalCount = 0;
    int _secondGoalCount = 0;
    int _thirdGoalCount = 0;
    int _actionsGoalCount = 0;

    int _indexFirstGoalPopup = 0;
    int _indexSecondGoalPopup = 0;
    int _indexThirdGoalPopup = 0;

    [SerializeField] private Texture _redToken;
    [SerializeField] private Texture _greenToken;
    [SerializeField] private Texture _blueToken;
    [SerializeField] private Texture _yellowToken;
    [SerializeField] private Texture _pinkToken;

    [SerializeField] private Texture _stoneObstacle;
    [SerializeField] private Texture _reinforcedStoneObstacle;
    [SerializeField] private Texture _iceObstacle;
    [SerializeField] private Texture _reinforcedIceObstacle;

    [SerializeField] private Texture _rocketBonus;
    [SerializeField] private Texture _bombBonus;

    [SerializeField] static private int _row = 10;
    [SerializeField] static private int _column = 5;

    [SerializeField] private int[,] _boardObjectForLoadLevel = new int[_row, _column];
    
    private int _countTabsOfLevelDisignerTab = 4;
    private string[] _tabsNamesOfLevelsDesigner = { "Tokens", "Obstacles", "Bonuses", "Goals" };
    private Texture _selectedObject;
    private int _idSelectedObject;
    private Texture[,] _objectsOfEditorField = new Texture[_row, _column];

    private SaveData _levelCharacters = new SaveData(_row, _column);
    string json;

    string[] _gameGoals = new string[] { $"{(GoalOfGame)0}", $"{(GoalOfGame)1}", $"{(GoalOfGame)2}", $"{(GoalOfGame)3}",
                                        $"{(GoalOfGame)4}", $"{(GoalOfGame)5}", $"{(GoalOfGame)6}", $"{(GoalOfGame)7}"};

    [MenuItem("Window/Match3 Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EditorScript));
    }

    private void OnGUI()
    {   
        _countTabsEditor = GUILayout.Toolbar(_countTabsEditor, _tabsNames);

        switch (_countTabsEditor)
        {
            case 0:
                LevelTab();
                break;
            case 1:
                GameDesigner();
                break;
        }
    }

    private void LevelTab()
    {
        _levelNumber = EditorGUILayout.IntField("Level", _levelNumber);

        if (GUILayout.Button("Load level parameters"))
        {
            LoadField();
            LoadGoals();
        }
    }

    private void GameDesigner()
    {   
        EditorGUILayout.LabelField("Level Designer:", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        _countTabsOfLevelDisignerTab = GUILayout.Toolbar(_countTabsOfLevelDisignerTab, _tabsNamesOfLevelsDesigner);

        switch (_countTabsOfLevelDisignerTab)
        {
            case 0:
                Tokens();
                break;
            case 1:
                Obstacles();
                break;
            case 2:
                Bonuses();
                break;
            case 3:
                Goals();
                break;
        }

        EditorGUILayout.LabelField("Game field:", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        for (int i = 0; i < _objectsOfEditorField.GetLength(0); i++)
        {
            for (int j = 0; j < _objectsOfEditorField.GetLength(1); j++)
            {
                if (j == 0)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                if (GUILayout.Button(_objectsOfEditorField[i, j], GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
                {
                    _objectsOfEditorField[i, j] = _selectedObject;
                    _boardObjectForLoadLevel[i, j] = _idSelectedObject;
                }

                if (j == 4)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        if (GUILayout.Button("Save Field"))
        {
            SaveField();
        }
    }
    
    private void Tokens()
    {
        GUILayout.Label("Tokens", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_redToken, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _redToken;
            _idSelectedObject = (int)TokenType.red;
        }

        if (GUILayout.Button(_greenToken, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _greenToken;
            _idSelectedObject = (int)TokenType.green;
        }
        
        if (GUILayout.Button(_blueToken, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50))) 
        {
            _selectedObject = _blueToken;
            _idSelectedObject = (int)TokenType.blue;
        }

        if (GUILayout.Button(_yellowToken, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50))) 
        {
            _selectedObject = _yellowToken;
            _idSelectedObject = (int)TokenType.yellow;
        }

        if (GUILayout.Button(_pinkToken, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _pinkToken;
            _idSelectedObject = (int)TokenType.pink;
        }
        GUILayout.EndHorizontal();
    }

    private void Obstacles()
    {
        GUILayout.Label("Obstacles", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_stoneObstacle, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _stoneObstacle;
            _idSelectedObject = (int)ObstacleType.stone;
        }

        if (GUILayout.Button(_reinforcedStoneObstacle, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _reinforcedStoneObstacle;
            _idSelectedObject = (int)ObstacleType.reinforcedStone;
        }

        if (GUILayout.Button(_iceObstacle, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _iceObstacle;
            _idSelectedObject = (int)ObstacleType.ice;
        }

        if (GUILayout.Button(_reinforcedIceObstacle, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _reinforcedIceObstacle;
            _idSelectedObject = (int)ObstacleType.reinforcedIce;
        }        
        GUILayout.EndHorizontal();
    }

    private void Bonuses()
    {
        GUILayout.Label("Bonuses", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_rocketBonus, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _rocketBonus;
            _idSelectedObject = (int)BonusType.rocket;
        }

        if (GUILayout.Button(_bombBonus, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
        {
            _selectedObject = _bombBonus;
            _idSelectedObject = (int)BonusType.bomb;
        }
        GUILayout.EndHorizontal();
    }

    private void Goals()
    {
        GUILayout.Label("Goals", EditorStyles.boldLabel);
        _actionsGoalCount = EditorGUILayout.IntField("Actions count", _actionsGoalCount);

        _firstGoalEnabled = EditorGUILayout.BeginToggleGroup("First goal", _firstGoalEnabled);
        _indexFirstGoalPopup = EditorGUILayout.Popup("First goal", _indexFirstGoalPopup, _gameGoals);
        _firstGoalCount = EditorGUILayout.IntField("Count", _firstGoalCount);
        EditorGUILayout.EndToggleGroup();

        if (_firstGoalEnabled)
        {
            _secondGoalEnabled = EditorGUILayout.BeginToggleGroup("Second goal", _secondGoalEnabled);
            _indexSecondGoalPopup = EditorGUILayout.Popup("Second goal", _indexSecondGoalPopup, _gameGoals);
            _secondGoalCount = EditorGUILayout.IntField("Count", _secondGoalCount);
            EditorGUILayout.EndToggleGroup();
        }

        if (_firstGoalEnabled && _secondGoalEnabled)
        {
            _thirdGoalEnabled = EditorGUILayout.BeginToggleGroup("Third goal", _thirdGoalEnabled);
            _indexThirdGoalPopup = EditorGUILayout.Popup("Third goal", _indexThirdGoalPopup, _gameGoals);
            _thirdGoalCount = EditorGUILayout.IntField("Count", _thirdGoalCount);
            EditorGUILayout.EndToggleGroup();
        }

        if (GUILayout.Button("Save Level"))
        {   
            SaveField();
            SaveGoals();

            json = JsonUtility.ToJson(_levelCharacters);
            string path = $"D:/UnityProjects/Match3/TestMatch3/Assets/Resources/{_levelNumber}.txt";
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(json);
            writer.Close();
            Debug.Log(path);
        }

        if (!_firstGoalEnabled)
        {
            _secondGoalEnabled = false;
            _thirdGoalEnabled = false;
        }
        else if (!_secondGoalEnabled)
        {
            _thirdGoalEnabled = false;
        }
    }

    private void SaveField()
    {
        int index = 0;
        for (int i = 0; i < _boardObjectForLoadLevel.GetLength(0); i++)
        {
            for (int j = 0; j < _boardObjectForLoadLevel.GetLength(1); j++)
            {
                PlayerPrefs.SetInt("Object of field[" + i + j + "]" + _levelNumber, _boardObjectForLoadLevel[i, j]);
                _levelCharacters._field[index++] = _boardObjectForLoadLevel[i, j];
            }
        }
    }

    private void SaveGoals()
    {
        PlayerPrefs.SetInt("Action count" + _levelNumber, _actionsGoalCount);
        _levelCharacters._countActionsToEndGame = _actionsGoalCount;

        if (_firstGoalEnabled)
        {
            PlayerPrefs.SetInt("first goal" + _levelNumber, _indexFirstGoalPopup);
            _levelCharacters._firstGoal = _indexFirstGoalPopup;
            PlayerPrefs.SetInt("count of first goal" + _levelNumber, _firstGoalCount);
            _levelCharacters._firstGoalCount = _firstGoalCount;
        }
        else
        {
            _firstGoalEnabled = false;
            _secondGoalEnabled = false;
            _thirdGoalEnabled = false;
            _firstGoalCount = 0;
            PlayerPrefs.SetInt("count of first goal" + _levelNumber, _firstGoalCount);
            _levelCharacters._firstGoalCount = _firstGoalCount;
            _secondGoalCount = 0;
            PlayerPrefs.SetInt("count of second goal" + _levelNumber, _secondGoalCount);
            _levelCharacters._secondGoalCount = _secondGoalCount;
            _thirdGoalCount = 0;
            PlayerPrefs.SetInt("count of third goal" + _levelNumber, _thirdGoalCount);
            _levelCharacters._thirdGoalCount = _thirdGoalCount;
        }

        if (_secondGoalEnabled)
        {
            PlayerPrefs.SetInt("second goal" + _levelNumber, _indexSecondGoalPopup);
            _levelCharacters._secondGoal = _indexSecondGoalPopup;
            PlayerPrefs.SetInt("count of second goal" + _levelNumber, _secondGoalCount);
            _levelCharacters._secondGoalCount = _secondGoalCount;
        }
        else
        {
            _secondGoalEnabled = false;
            _thirdGoalEnabled = false;
            _secondGoalCount = 0;
            PlayerPrefs.SetInt("count of second goal" + _levelNumber, _secondGoalCount);
            _levelCharacters._secondGoal = _secondGoalCount;
            _thirdGoalCount = 0;
            PlayerPrefs.SetInt("count of third goal" + _levelNumber, _thirdGoalCount);
            _levelCharacters._thirdGoal = _thirdGoalCount;
        }

        if (_thirdGoalEnabled)
        {
            PlayerPrefs.SetInt("third goal" + _levelNumber, _indexThirdGoalPopup);
            _levelCharacters._thirdGoal = _indexThirdGoalPopup;
            PlayerPrefs.SetInt("count of third goal" + _levelNumber, _thirdGoalCount);
            _levelCharacters._thirdGoalCount = _thirdGoalCount;
        }
        else
        {
            _thirdGoalEnabled = false;
            _thirdGoalCount = 0;
            PlayerPrefs.SetInt("count of third goal" + _levelNumber, _thirdGoalCount);
            _levelCharacters._thirdGoal = _indexThirdGoalPopup;
        }
    }

    private void LoadGoals()
    {
        _actionsGoalCount = PlayerPrefs.GetInt("Action count" + _levelNumber);

        if (PlayerPrefs.GetInt("count of first goal" + _levelNumber) != 0)
        {
            _firstGoalEnabled = true;
            _indexFirstGoalPopup = PlayerPrefs.GetInt("first goal" + _levelNumber);
            _firstGoalCount = PlayerPrefs.GetInt("count of first goal" + _levelNumber);
        }
        else
        {
            _firstGoalEnabled = false;
        }

        if (PlayerPrefs.GetInt("count of second goal" + _levelNumber) != 0)
        {
            _secondGoalEnabled = true;
            _indexSecondGoalPopup = PlayerPrefs.GetInt("second goal" + _levelNumber);
            _secondGoalCount = PlayerPrefs.GetInt("count of second goal" + _levelNumber);
        }
        else
        {
            _secondGoalEnabled = false;
            _secondGoalCount = 0;
        }

        if (PlayerPrefs.GetInt("count of third goal" + _levelNumber) != 0)
        {
            _thirdGoalEnabled = true;
            _indexThirdGoalPopup = PlayerPrefs.GetInt("third goal" + _levelNumber);
            _thirdGoalCount = PlayerPrefs.GetInt("count of third goal" + _levelNumber);
        }
        else
        {
            _thirdGoalEnabled = false;
            _thirdGoalCount = 0;
        }
    }

    private void LoadField()
    {
        for (int row = 0; row < _objectsOfEditorField.GetLength(0); row++)
        {
            for (int column = 0; column < _objectsOfEditorField.GetLength(1); column++)
            {
                int id = PlayerPrefs.GetInt("Object of field[" + row + column + "]" + _levelNumber);

                switch (id)
                {
                    case (int)TokenType.red: _objectsOfEditorField[row, column] = _redToken;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)TokenType.green: _objectsOfEditorField[row, column] = _greenToken;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)TokenType.blue: _objectsOfEditorField[row, column] = _blueToken;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)TokenType.yellow: _objectsOfEditorField[row, column] = _yellowToken;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)TokenType.pink: _objectsOfEditorField[row, column] = _pinkToken;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)ObstacleType.stone: _objectsOfEditorField[row, column] = _stoneObstacle;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)ObstacleType.reinforcedStone: _objectsOfEditorField[row, column] = _reinforcedStoneObstacle;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)ObstacleType.ice: _objectsOfEditorField[row, column] = _iceObstacle;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)ObstacleType.reinforcedIce: _objectsOfEditorField[row, column] = _reinforcedIceObstacle;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)BonusType.rocket: _objectsOfEditorField[row, column] = _rocketBonus;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                    case (int)BonusType.bomb: _objectsOfEditorField[row, column] = _bombBonus;
                        _boardObjectForLoadLevel[row, column] = id;
                        break;
                }
            }
        }
    }
}