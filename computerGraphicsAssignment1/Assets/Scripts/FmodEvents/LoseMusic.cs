using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMusic : MonoBehaviour
{
    FmodCarSoundManager lose;
    void Start()
    {
        lose = new FmodCarSoundManager("event:/LoseSong");
        lose.StartEventSound();
    }

    private void OnDisable()
    {
        lose.EndSoundInstance();
    }
    private void OnDestroy()
    {
        lose.EndSoundInstance();
    }

}
