using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; set; }
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Start()
    {
        Score = 0;
    }

    public void AdditionScore(int objectPrice)
    {
        Score += objectPrice;
        _scoreText.text = $"Score: {Score}";
    }

    public void ResetScore()
    {
        Score = 0;
        _scoreText.text = $"Score: {Score}";
    }
}