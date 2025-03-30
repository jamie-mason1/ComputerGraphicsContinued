using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic
{
    // Start is called before the first frame update
    public FmodCarSoundManagerSetGlobalParameters MusicChange;
    public FmodCarSoundManager BattleMusic;
    public FmodCarSoundManager StrollMusic;
    public bool paused;
    public BackgroundMusic()
    {
        BattleMusic = new FmodCarSoundManager("event:/BackgroundMusicEngaged");
        StrollMusic = new FmodCarSoundManager("event:/BackgroundMusicNotEngaged");
        MusicChange = new FmodCarSoundManagerSetGlobalParameters();
    }

    public void ManageMusic(float m)
    {
        m = Mathf.Clamp01(m);

        

        MusicChange.SetGlobalParameter("GoToLoop", m);
    }
}
