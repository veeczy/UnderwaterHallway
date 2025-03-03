using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //misc UI variables
    public GameObject mainMenu;
    public GameObject aboutMenu;
    public GameObject optionsMenu;
    public GameObject pauseMenu;

    private GameObject lastActiveMenu;
    private bool isPaused = false;

    //win lose variables
    public GameObject loseScreen;
    public GameObject winScreen;
    public GameObject key;
    public bool winCondition;
    public GameObject player;
    public bool loseCondition;
    public AudioSource bgmSource;

    //health bar variables
    public int currentHealth;
    public GameObject healthIndicator;
    public GameObject health1; //20 health
    public GameObject health2; //40 health
    public GameObject health3; //60 health
    public GameObject health4; //80 health
    public GameObject health5; //100 health (full)

    //stunHUD variables
    public GameObject equipment;
    public Image IMG;
    public float stunRatio;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("SkipMainMenu", 0) == 1)
        {
            PlayerPrefs.SetInt("SkipMainMenu", 0);
            PlayerPrefs.Save();
            StartGame();
        }
        else
        {
            OpenMenu(mainMenu);
            Time.timeScale = 0f;
        }

        winCondition = key.GetComponent<keyScript>().winCondition;
        loseCondition = player.GetComponent<PlayerHealth>().loseCondition;
        currentHealth = player.GetComponent<PlayerHealth>().currentHealth;
        stunRatio = enemy.GetComponent<EnemyScript>().stunRatio;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;

            if (mainMenu.activeSelf || loseScreen.activeSelf || winScreen.activeSelf)
                return;

            if (isPaused)
            {
                OpenMenu(pauseMenu);
            }
            else
            {
                CloseMenus();
            }
        }

        if (key.GetComponent<keyScript>().winCondition)
        {
            WinScreen();
            bgmSource.Stop();
        }

        if (player.GetComponent<PlayerHealth>().loseCondition)
        {
            LoseScreen();
            bgmSource.Stop();
        }

        //health bar
        if (player.GetComponent<PlayerHealth>().currentHealth <= 80) { health5.SetActive(false); } 
        if (player.GetComponent<PlayerHealth>().currentHealth <= 60) { health4.SetActive(false); }
        if (player.GetComponent<PlayerHealth>().currentHealth <= 40) { health3.SetActive(false); }
        if (player.GetComponent<PlayerHealth>().currentHealth <= 20) { health2.SetActive(false); }
        if (player.GetComponent<PlayerHealth>().currentHealth <= 0) { health1.SetActive(false); }

        IMG.fillAmount = (enemy.GetComponent<EnemyScript>().stunRatio); // fill for flashlight HUD

    }

    public void ResumeGame()
    {
        CloseMenus();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        equipment.SetActive(false);
        healthIndicator.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenMenu(GameObject menu)
    {
        if (mainMenu.activeSelf) lastActiveMenu = mainMenu;
        else if (pauseMenu.activeSelf) lastActiveMenu = pauseMenu;

        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);

        menu.SetActive(true);


        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        FindObjectOfType<flashlight>().enabled = false;
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<CharacterController>().enabled = false;
    }

    public void CloseMenus()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FindObjectOfType<flashlight>().enabled = true;
        FindObjectOfType<PlayerMovement>().enabled = true;
        FindObjectOfType<CharacterController>().enabled = true;
    }

    public void OpenOptions()
    {
        OpenMenu(optionsMenu);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
    }

    public void ReturnToPreviousMenu()
    {
        {
            if (lastActiveMenu != null)
            {
                OpenMenu(lastActiveMenu);
            }
            else
            {
                OpenMenu(mainMenu);
            }
        }
    }

    public void WinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;

        FindObjectOfType<flashlight>().enabled = false;
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<CharacterController>().enabled = false;
    }

    public void LoseScreen()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;

        FindObjectOfType<flashlight>().enabled = false;
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<CharacterController>().enabled = false;
    }

    public void OpenAbout()
    {
        aboutMenu.SetActive(true);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void StartGame()
    {
        CloseMenus();
        equipment.SetActive(true);
        healthIndicator.SetActive(true);
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("SkipMainMenu", 1);
        PlayerPrefs.Save();

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.currentHealth = playerHealth.startHealth;
            PlayerHealth.isDead = false;
            playerHealth.loseCondition = false;
        }


        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}