using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitDialogue
{
    public FmodCarSoundManager Quit { get; private set; }
    public QuitDialogue()
    {
        string eventPath = "event:/IQuitdialogue";
        Quit = new FmodCarSoundManager(eventPath);
    }
}
