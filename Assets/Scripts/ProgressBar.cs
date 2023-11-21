using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _progressBarSlider;
    [SerializeField] private GameObject _firstPearl;
    [SerializeField] private GameObject _secondPearl;
    [SerializeField] private GameObject _thirdPearl;

    private void OnEnable()
    {
        LevelManager._pointsToGoalEnable += AnableProgressBar;
    }

    private void OnDisable()
    {
        LevelManager._pointsToGoalEnable -= AnableProgressBar;
    }

    private void Start()
    {
        _progressBarSlider.maxValue = LevelManager._pointsToGoal * 3;
    }

    public void UpdateRating()
    {
        _progressBarSlider.value = ScoreManager.Score;

        if (ScoreManager.Score >= LevelManager._pointsToGoal)
        {
            _firstPearl.SetActive(true);
        }
        
        if (ScoreManager.Score >= LevelManager._pointsToGoal * 2)
        {
            _secondPearl.SetActive(true);
        }
        
        if (ScoreManager.Score >= LevelManager._pointsToGoal * 3)
        {
            _thirdPearl.SetActive(true);
        }
    }

    private void AnableProgressBar()
    { 
        gameObject.SetActive(true);
    }
}