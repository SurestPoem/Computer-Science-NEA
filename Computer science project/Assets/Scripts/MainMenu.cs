using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider difficultySlider;  // Reference to the slider in the main menu

    private void Start()
    {
        difficultySlider.value = (int)GameManager.Instance.currentDifficulty;
        difficultySlider.onValueChanged.AddListener(OnDifficultyChanged);
    }

    // Called when the slider value changes
    public void OnDifficultyChanged(float value)
    {
        // Set difficulty in GameManager (cast the float slider value to an int)
        GameManager.Instance.SetDifficulty((int)value);

        // Optionally: debug log to confirm it's being set
        Debug.Log("Difficulty Set to: " + GameManager.Instance.currentDifficulty);
    }
    
    public void NormalGameButton()
    {
        GameManager.Instance.StartGame(0);
    }

    public void EndlessGameButton()
    {
        GameManager.Instance.StartGame(1);
    }

    public void TutorialGameButton()
    {
        GameManager.Instance.currentDifficulty = GameManager.Difficulty.Baby;
        GameManager.Instance.StartGame(2);
    }

    public void QuitGameButton()
    {
        GameManager.Instance.QuitGame();
    }
}
