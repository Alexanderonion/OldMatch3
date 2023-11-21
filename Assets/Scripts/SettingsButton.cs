using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject menu;
    private bool isShowing = false;

    public void OnToggleMenu()
    {
        isShowing = !isShowing;
        menu.SetActive(isShowing);
    }
}
