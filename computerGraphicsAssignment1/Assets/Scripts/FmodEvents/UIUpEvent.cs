using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpEvent
{
    public FmodCarSoundManager Up { get; private set; }
    public UIUpEvent()
    {
        string eventPath = "event:/UIUp";
        Up = new FmodCarSoundManager(eventPath);
    }
}
