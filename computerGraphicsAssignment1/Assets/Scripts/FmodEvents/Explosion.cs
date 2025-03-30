using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion
{
    public FmodCarSoundManager explode { get; private set; }
    public Explosion()
    {
        string eventPath = "event:/Explosion";
        explode = new FmodCarSoundManager(eventPath);
    }
}
