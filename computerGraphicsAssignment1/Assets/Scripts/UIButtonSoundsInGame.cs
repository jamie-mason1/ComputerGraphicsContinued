using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSoundsInGame : MonoBehaviour
{
    UISelectEvent select;
    Restart restart;
    QuitDialogue quitDialogue;
    public UIUpEvent up;
    public UIDownEvent down;
    GameObject selectedObject;
    GameObject lastSelectedObject;
    PauseMenu pause;

    void Start()
    {
        selectedObject = EventSystem.current.firstSelectedGameObject;
        lastSelectedObject = selectedObject;
        restart = new Restart();
        quitDialogue = new QuitDialogue();
        select = new UISelectEvent();
        up = new UIUpEvent();
        down = new UIDownEvent();
        pause = GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (down == null)
        {
            down = new UIDownEvent();
        }
        if (up == null)
        {
            up = new UIUpEvent();
        }
        if (select == null)
        {
            select = new UISelectEvent();
        }
        if (restart == new Restart())
        {
            restart = new Restart();
        }
        if (quitDialogue == null)
        {
            quitDialogue = new QuitDialogue();
        }
        if (pause.paused)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject != lastSelectedObject)
                {
                    if (!up.Up.IsEventPlaying())
                    {
                        up.Up.StartEventSound();
                    }
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                selectedObject = EventSystem.current.currentSelectedGameObject;
                if (selectedObject != lastSelectedObject)
                {
                    if (!down.Down.IsEventPlaying())
                    {
                        down.Down.StartEventSound();
                    }
                }
            }
        }
        lastSelectedObject = selectedObject;
    }
    public void SelectSound()
    {
        if (!select.Select.IsEventPlaying())
        {
            select.Select.StartEventSound();
        }
    }
    public void RestartSound()
    {
        if (!restart.RestartSound.IsEventPlaying())
        {
            restart.RestartSound.StartEventSound();
        }
    }
    public void QuitSound()
    {
        if (!quitDialogue.Quit.IsEventPlaying())
        {
            quitDialogue.Quit.StartEventSound();
        }
    }
}
