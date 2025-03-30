using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDownEvent
{
    public FmodCarSoundManager Down { get; private set; }
    public UIDownEvent()
    {
        string eventPath = "event:/UIDown";
        Down = new FmodCarSoundManager(eventPath);
    }
}
