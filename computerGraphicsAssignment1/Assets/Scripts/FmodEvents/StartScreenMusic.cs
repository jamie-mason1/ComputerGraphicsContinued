using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMusic : MonoBehaviour
{
    FmodCarSoundManager start;
    void Start()
    {
        start = new FmodCarSoundManager("event:/StartMusic");
        start.StartEventSound();
    }

    private void OnDisable()
    {
        start.EndSoundInstance();
    }
    private void OnDestroy()
    {
        start.EndSoundInstance();
    }
}
