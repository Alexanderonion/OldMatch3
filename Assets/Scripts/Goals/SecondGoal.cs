using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondGoal : Goal
{
    [SerializeField] private TextMeshProUGUI _secondGoalCountText;
    [SerializeField] private GameObject _secondPictureGoalObject;

    public override void LoadGoalJson(SaveData saveData)
    {
        GoalType = (GoalOfGame)saveData._secondGoal;
        GoalCount = saveData._secondGoalCount;
        _secondGoalCountText.text = $"{GoalCount}";
        AsignPictureGoals(saveData._secondGoal, _secondPictureGoalObject);
        ReachGoal = false;
    }

    public override void LoadGoal(int levelNumber)
    {
        if (PlayerPrefs.GetInt("second goal" + levelNumber) != 0)
        {
            gameObject.SetActive(true);
            GoalType = (GoalOfGame)PlayerPrefs.GetInt("second goal" + levelNumber);
            GoalCount = PlayerPrefs.GetInt("count of second goal" + levelNumber);
            _secondGoalCountText.text = $"{GoalCount}";
            AsignPictureGoals((int)GoalType, _secondPictureGoalObject);
            ReachGoal = false;
        }
    }

    public override void IncrementGoalsValue()
    {
        if (GoalCount > 0)
        {
            GoalCount--;
            _secondGoalCountText.text = $"{GoalCount}";

            if (GoalCount <= 0)
            {
                ReachGoal = true;
            }
        }
    }
}
