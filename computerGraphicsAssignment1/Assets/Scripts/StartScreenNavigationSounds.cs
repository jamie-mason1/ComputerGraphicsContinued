using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScreenNavigationSounds : MonoBehaviour
{
    UIUpEvent up;
    UIDownEvent down;
    UISelectEvent select;
    Restart restart;
    QuitDialogue quitDialogue;
    GameObject selectedObject;
    GameObject lastSelectedObject;
    void Start()
    {
        selectedObject = EventSystem.current.firstSelectedGameObject;
        lastSelectedObject = selectedObject;
        up = new UIUpEvent();
        down = new UIDownEvent();
        restart = new Restart();
        quitDialogue = new QuitDialogue();
        select = new UISelectEvent();
    }

    void Update()
    {
        selectedObject = EventSystem.current.currentSelectedGameObject;
        if (down == null)
        {
            down = new UIDownEvent();
        }
        if(up == null)
        {
            up = new UIUpEvent();
        }
        if (select == null)
        {
            select = new UISelectEvent();
        }
        if(restart == new Restart())
        {
            restart = new Restart();
        }
        if(quitDialogue == null)
        {
            quitDialogue = new QuitDialogue();
        }

        if (selectedObject.GetComponent<RectTransform>().position.y > lastSelectedObject.GetComponent<RectTransform>().position.y) {
            if (!up.Up.IsEventPlaying())
            {
                up.Up.StartEventSound();
            }
        }
        else if (selectedObject.GetComponent<RectTransform>().anchoredPosition.y < lastSelectedObject.GetComponent<RectTransform>().anchoredPosition.y)
        {
            if (!down.Down.IsEventPlaying())
            {
                down.Down.StartEventSound();
            }
        }
        /*
*/

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
