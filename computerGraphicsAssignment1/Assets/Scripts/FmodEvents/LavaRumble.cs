using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava
{
   public FmodCarSoundManager lavaRumble;
   public FmodCarSoundManager lavaCrackle;
   public Lava()
    {
        string fmodEvent = "event:/LavaRumble";
        string fmodEvent1 = "event:/Crackling";
        lavaRumble = new FmodCarSoundManager(fmodEvent);
        lavaCrackle = new FmodCarSoundManager(fmodEvent1);

    }
}
