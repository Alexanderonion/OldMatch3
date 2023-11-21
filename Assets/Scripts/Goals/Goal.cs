using TMPro;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    public int GoalCount { get; set; }
    private TextMeshProUGUI _goalCountText;
    private GameObject _pictureGoalObject;
    public GoalOfGame GoalType { get; set; }
    public bool ReachGoal { get; set; }

    public virtual void LoadGoalJson(SaveData saveData)
    {
        GoalType = (GoalOfGame)saveData._firstGoal;
        GoalCount = saveData._firstGoalCount;
        _goalCountText.text = $"{GoalCount}";
        AsignPictureGoals(saveData._firstGoal, _pictureGoalObject);
        ReachGoal = false;
    }

    public virtual void LoadGoal(int levelsNumber)
    {
        if (PlayerPrefs.GetInt("lock goal" + levelsNumber) != 0)
        {
            GoalType = (GoalOfGame)PlayerPrefs.GetInt("lock goal" + levelsNumber);
            GoalCount = PlayerPrefs.GetInt("lock count of goal" + levelsNumber);
            _goalCountText.text = $"{GoalCount}";
            AsignPictureGoals((int)GoalType, _pictureGoalObject);
            ReachGoal = false;
        }
    }

    public virtual void IncrementGoalsValue()
    {
        if (GoalCount > 0)
        {
            GoalCount--;
            _goalCountText.text = $"{GoalCount}";

            if (GoalCount <= 0)
            {
                ReachGoal = true;
            }
        }
    }

    public void AsignPictureGoals(int playerPrefsGoal, GameObject goalPicture)
    {
        switch (playerPrefsGoal)
        {
            case 1: goalPicture.transform.GetChild(1).gameObject.SetActive(true); break;
            case 2: goalPicture.transform.GetChild(2).gameObject.SetActive(true); break;
            case 3: goalPicture.transform.GetChild(3).gameObject.SetActive(true); break;
            case 4: goalPicture.transform.GetChild(4).gameObject.SetActive(true); break;
            case 5: goalPicture.transform.GetChild(5).gameObject.SetActive(true); break;
            case 6: goalPicture.transform.GetChild(6).gameObject.SetActive(true); break;
            case 7: goalPicture.transform.GetChild(7).gameObject.SetActive(true); break;
        }
    }
}
