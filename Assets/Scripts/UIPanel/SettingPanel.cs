using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btn_ControlBgMusic;
    public Button btn_ControlEffectSound;

    public Slider bgMusicVolumnSlider;
    public Slider effectSoundVolumnSlider;

    protected override void InitWindow()
    {
        base.InitWindow();
        btn_ControlBgMusic.GetComponent<Image>().sprite = 
            audioSvc.isActiveBgMusic ? resSvc.LoadSprite(PathDefine.checkSprite) : resSvc.LoadSprite(PathDefine.checknotSprite);
        btn_ControlEffectSound.GetComponent<Image>().sprite =
            audioSvc.isActiveBgMusic ? resSvc.LoadSprite(PathDefine.checkSprite) : resSvc.LoadSprite(PathDefine.checknotSprite);

        btn_ControlBgMusic.onClick.AddListener(OnBtnControlBgMusicClick);
        btn_ControlEffectSound.onClick.AddListener(OnBtnControlEffectSoundClick);

        bgMusicVolumnSlider.onValueChanged.AddListener((float value) => 
        {
            audioSvc.SetBgMusicVolumn(value);
        });
        effectSoundVolumnSlider.onValueChanged.AddListener((float value) =>
        {
            audioSvc.SetEffectSoundVolumn(value);
        });
    }

    private void OnBtnControlBgMusicClick()
    {
        audioSvc.PlayEffectSound(PathDefine.btnClickSound);
        if (audioSvc.isActiveBgMusic)
        {
            audioSvc.InActiveBgMusic();
            btn_ControlBgMusic.GetComponent<Image>().sprite = resSvc.LoadSprite(PathDefine.checknotSprite);
        }
        else
        {
            audioSvc.ActiveBgMusic();
            btn_ControlBgMusic.GetComponent<Image>().sprite = resSvc.LoadSprite(PathDefine.checkSprite);
        }
    }

    private void OnBtnControlEffectSoundClick()
    {
        audioSvc.PlayEffectSound(PathDefine.btnClickSound);
        if (audioSvc.isActiveEffectSound)
        {
            audioSvc.InActiveEffectSound();
            btn_ControlEffectSound.GetComponent<Image>().sprite = resSvc.LoadSprite(PathDefine.checknotSprite);
        }
        else
        {
            audioSvc.ActiveEffectSound();
            btn_ControlEffectSound.GetComponent<Image>().sprite = resSvc.LoadSprite(PathDefine.checkSprite);
        }
    }


}
