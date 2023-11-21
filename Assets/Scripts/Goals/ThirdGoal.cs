using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThirdGoal : Goal
{
    [SerializeField] private TextMeshProUGUI _thirdGoalCountText;
    [SerializeField] private GameObject _thirdPictureGoalObject;

    public override void LoadGoalJson(SaveData saveData)
    {
        GoalType = (GoalOfGame)saveData._thirdGoal;
        GoalCount = saveData._thirdGoalCount;
        _thirdGoalCountText.text = $"{GoalCount}";
        AsignPictureGoals(saveData._thirdGoal, _thirdPictureGoalObject);
        ReachGoal = false;
    }

    public override void LoadGoal(int levelNumber)
    {
        if (PlayerPrefs.GetInt("third goal" + levelNumber) != 0)
        {
            gameObject.SetActive(true);
            GoalType = (GoalOfGame)PlayerPrefs.GetInt("third goal" + levelNumber);
            GoalCount = PlayerPrefs.GetInt("count of third goal" + levelNumber);
            _thirdGoalCountText.text = $"{GoalCount}";
            AsignPictureGoals((int)GoalType, _thirdPictureGoalObject);
            ReachGoal = false;
        }
    }

    public override void IncrementGoalsValue()
    {
        if (GoalCount > 0)
        {
            GoalCount--;
            _thirdGoalCountText.text = $"{GoalCount}";

            if (GoalCount <= 0)
            {
                ReachGoal = true;
            }
        }
    }
}