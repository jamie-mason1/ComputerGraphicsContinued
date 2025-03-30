using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart
{
    public FmodCarSoundManager RestartSound { get; private set; }
    public Restart()
    {
        string eventPath = "event:/Restart";
        RestartSound = new FmodCarSoundManager(eventPath);
    }
}
