using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMusic : MonoBehaviour
{
    // Start is called before the first frame update
    FmodCarSoundManager win;
    void Start()
    {
        win = new FmodCarSoundManager("event:/WinSong");
        win.StartEventSound();
    }

    private void OnDisable()
    {
        win.EndSoundInstance();
    }
    private void OnDestroy()
    {
        win.EndSoundInstance();
    }
  
}
