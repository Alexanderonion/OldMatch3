using System;

[Serializable]
public class SaveData
{
    public int _row;
    public int _column;
    public int _firstGoal;
    public int _firstGoalCount;
    public int _secondGoal;
    public int _secondGoalCount;
    public int _thirdGoal;
    public int _thirdGoalCount;
    public int _countActionsToEndGame;
    public int[] _field;

    public SaveData(int row, int column)
    {
        _row = row;
        _column = column;
        _field = new int[row * column];
        _firstGoal = 0;
        _firstGoalCount = 0;
        _secondGoal = 0;
        _secondGoalCount = 0;
        _thirdGoal = 0;
        _thirdGoalCount = 0;
        _countActionsToEndGame = 0;
    }
}