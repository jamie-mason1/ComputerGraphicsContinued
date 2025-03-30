using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    Lava lava;
    [SerializeField] private PauseMenu pause;
    bool lastPausedState;
    void Start()
    {
        lava = new Lava();
        lastPausedState = pause.paused;
    }

    void Update()
    {
        if(lava == null)
        {
            lava = new Lava();
        }
        if (pause.paused)
        {
            if (lava.lavaRumble.IsEventPlaying())
            {
                lava.lavaRumble.PauseEventSound();
            }
            if (lava.lavaCrackle.IsEventPlaying())
            {
                lava.lavaCrackle.PauseEventSound();
            }
        }
        else
        {
            if (!lava.lavaRumble.IsEventPlaying())
            {
                lava.lavaRumble.StartEventSound();
            }
            if (!lava.lavaCrackle.IsEventPlaying())
            {
                lava.lavaCrackle.StartEventSound();
            }
            if (pause != lastPausedState)
            {
                lava.lavaCrackle.ResumeEventSound();
                lava.lavaRumble.ResumeEventSound();
            }

        }
        lastPausedState = pause.paused;

    }
    private void OnDisable()
    {
        lava.lavaCrackle.EndSoundInstance();
        lava.lavaRumble.EndSoundInstance();


    }
    private void OnDestroy()
    {
        lava.lavaCrackle.EndSoundInstance();
        lava.lavaRumble.EndSoundInstance();


    }
}
