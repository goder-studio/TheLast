using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected TimerSvc timerSvc = null;
    protected AudioSvc audioSvc = null;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }

    public virtual void ClearSvc()
    {
        resSvc = null;
        timerSvc = null;
        audioSvc = null;
    }
}
