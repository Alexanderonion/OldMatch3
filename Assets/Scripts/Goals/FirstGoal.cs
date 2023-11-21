using TMPro;
using UnityEngine;
using System;

public class FirstGoal : Goal
{
    [SerializeField] private TextMeshProUGUI _firstGoalCountText;
    [SerializeField] private GameObject _firstPictureGoalObject;

    public override void LoadGoalJson(SaveData saveData)
    {
        GoalType = (GoalOfGame)saveData._firstGoal;
        GoalCount = saveData._firstGoalCount;
        _firstGoalCountText.text = $"{GoalCount}";
        AsignPictureGoals(saveData._firstGoal, _firstPictureGoalObject);
        ReachGoal = false;
    }

    public override void LoadGoal(int levelNumber)
    {
        if (PlayerPrefs.GetInt("first goal" + levelNumber) != 0)
        {
            gameObject.SetActive(true);
            GoalType = (GoalOfGame)PlayerPrefs.GetInt("first goal" + levelNumber);
            GoalCount = PlayerPrefs.GetInt("count of first goal" + levelNumber);
            _firstGoalCountText.text = $"{GoalCount}";
            AsignPictureGoals((int)GoalType, _firstPictureGoalObject);
            ReachGoal = false;
        }
    }

    public override void IncrementGoalsValue()
    {
        if (GoalCount > 0)
        {
            GoalCount--;
            _firstGoalCountText.text = $"{GoalCount}";

            if (GoalCount <= 0)
            {
                ReachGoal = true;
            }
        }
    }
}