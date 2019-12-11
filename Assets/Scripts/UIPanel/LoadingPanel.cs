using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public Image img_progressBar;
    public Text txt_progress;
    public Text tips;

    private float currentProgress;
    private float targetProgress;

    protected override void InitWindow()
    {
        base.InitWindow();

        currentProgress = targetProgress = 0;
        img_progressBar.fillAmount = 0;
        SetText(txt_progress, 0 + "%");
        SetActive(tips, false);
    }

    private void Update()
    {
        if(currentProgress != targetProgress)
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, 0.5f);
            img_progressBar.fillAmount = currentProgress;
            SetText(txt_progress, (int)(currentProgress* 100) + "%");
        }
        if(currentProgress == 1.0f)
        {
            SetActive(tips, true);
        }
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        targetProgress = progress;
        if(progress >= 0.9f)
        {
            targetProgress = 1.0f;
        }
    }
}
