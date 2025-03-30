using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectEvent
{
    public FmodCarSoundManager Select { get; private set; }
    public UISelectEvent()
    {
        string eventPath = "event:/UISelect";
        Select = new FmodCarSoundManager(eventPath);


    }

}
