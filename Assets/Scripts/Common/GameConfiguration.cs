using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameConfiguration
{
    public bool isActiveBgMusic;
    public float bgMusicVolumn;
    public bool isActiveEffectSound;
    public float effectSoundVolumn;
    public bool[] levelPass;
    public bool[] levelActive;
}
