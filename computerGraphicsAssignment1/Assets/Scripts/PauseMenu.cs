using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseContents;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] Button controlsButton;
    [SerializeField] Button resumeButton;
    public bool paused;
    //resume sound fmodevent
    //pause sound fmod event
    //ui naivation sounds event
    //button click event sound

    private void Awake()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Time.timeScale = 1f;

    }
    void Start()
    {
        paused = false;
        Time.timeScale = 1f;
        if (controlsButton != null) controlsButton.onClick.AddListener(() => TransitionToNewMenu(pauseContents, controlsMenu));
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                PauseGame();
                
            }
            else if (controlsMenu.activeSelf)
            {
                TransitionToNewMenu(controlsMenu, pauseContents);
            }
            else
            {
                ResumeGame();
            }

        }
    }
    void PauseSounds()
    {

    }
    void ResumeSounds()
    {

    }
    void ResumeGame()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        ResumeSounds();


    }
    void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        pauseMenu.SetActive(true);
        pauseContents.SetActive(true);
        PauseSounds();

    }
    private void TransitionToNewMenu(GameObject current, GameObject nextMenu)
    {
        if (current != null && nextMenu != null)
        {
            current.SetActive(false);
            nextMenu.SetActive(true);
        }
    }
}
