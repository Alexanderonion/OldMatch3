using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void AssigningALevelNumber()
    {
        LevelManager._levelNumber = int.Parse(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
    }
}