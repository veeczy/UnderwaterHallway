using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject aboutMenu;
    public GameObject optionsMenu;
    public GameObject pauseMenu;
    public GameObject equipment;
    public GameObject loseScreen;
    public GameObject winScreen;

    private GameObject lastActiveMenu;
    private bool isPaused = false;

    public GameObject key;
    public bool winCondition;
    public GameObject player;
    public bool loseCondition;
    //public AudioSource winSource;
    //public AudioSource loseSource;

    // Start is called before the first frame update
    void Start()
    {
        OpenMenu(mainMenu);
        Time.timeScale = 0f;

        bool winCondition = key.GetComponent<keyScript>().winCondition;
        bool loseCondition = player.GetComponent<PlayerHealth>().loseCondition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeSelf || loseScreen.activeSelf)
                return;

            if (isPaused)
                OpenMenu(pauseMenu);
            else
                CloseMenus();
        }

        if(winCondition) //win
        {
            //winSource.Play(); // win vfx
            winScreen.SetActive(true);
        }
        if(loseCondition) //lose
        {
            //loseSource.Play(); // lose vfx
            loseScreen.SetActive(true);
        }
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