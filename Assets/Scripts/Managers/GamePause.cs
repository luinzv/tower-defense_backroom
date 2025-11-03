using UnityEngine;
using UnityEngine.SceneManagement; 

public class GamePause : MonoBehaviour
{
    public GameObject menuPause;
    public bool gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    public void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
