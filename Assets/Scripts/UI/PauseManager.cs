using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    private void OnEnable()
    {
        PlayerController.onPressStart += pause;
    }

    private void OnDisable()
    {
        PlayerController.onPressStart -= pause;
    }

    void pause()
    {
        if(GameManager.CurrentState == GameState.Gameplay)
        {
            //activar pause
            GameManager.SwitchState(GameState.Pause);
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if(GameManager.CurrentState == GameState.Pause)
        {
            //desactivar pause
            GameManager.SwitchState(GameState.Gameplay);
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
