using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    #region 单例模式
    private static ResSvc _instance = null;

    public static ResSvc Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<ResSvc>();
            }
            return _instance;
        }
    }
    #endregion

    public void InitSvc()
    {
        Debug.Log("Init ResSvc Done");
    }

    //更新进度条的委托
    private Action progressUpdate = null;
    //存储预制体的字典
    private Dictionary<string, GameObject> prefabDicts = new Dictionary<string, GameObject>();
    //存储图片资源的字典
    private Dictionary<string, Sprite> spriteDicts = new Dictionary<string, Sprite>();
    //存储贴图的字典
    private Dictionary<string, Texture> textureDicts = new Dictionary<string, Texture>();
    //音效资源
    private Dictionary<string, AudioClip> audioClipDicts = new Dictionary<string, AudioClip>();


    public void AsyncLoadScene(int sceneIndex,Action finish)
    {
        //显示加载界面
        GameManager.Instance.loadingPanel.SetWindowState(true);
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneIndex);
        sceneAsync.allowSceneActivation = false;
        progressUpdate = () =>
        {
            float progress = sceneAsync.progress;
            //设置进度条
            GameManager.Instance.loadingPanel.SetProgress(progress);
            if(progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    sceneAsync.allowSceneActivation = true;
                }
            }

            if(progress == 1)
            {
                if(finish != null)
                {
                    finish();
                }
                progressUpdate = null;
                sceneAsync = null;
                //加载完成隐藏加载界面
                GameManager.Instance.loadingPanel.SetWindowState(false);
            }
        };
    }

    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if(!prefabDicts.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if(cache)
            {
                prefabDicts.Add(path, prefab);
            }
        }
        GameObject go = null;
        if(prefab != null)
        {
            go = Instantiate(prefab);
        }
        return go;
    }

    /// <summary>
    /// 加载Sprite资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string path,bool cache = false)
    {
        Sprite sprite = null;
        if(!spriteDicts.TryGetValue(path,out sprite))
        {
            sprite = Resources.Load<Sprite>(path);
            if(cache)
            {
                spriteDicts.Add(path, sprite);
            }
        }
        return sprite;
    }

    /// <summary>
    /// 加载纹理贴图资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public Texture LoadTexture(string path,bool cache = false)
    {
        Texture texture = null;
        if(!textureDicts.TryGetValue(path,out texture))
        {
            texture = Resources.Load<Texture>(path);
            if(cache)
            {
                textureDicts.Add(path, texture);
            }
        }
        return texture;
    }

    /// <summary>
    /// 加载音效资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public AudioClip LoadAudioClip(string path,bool cache = false)
    {
        AudioClip audioClip = null;
        if(!audioClipDicts.TryGetValue(path,out audioClip))
        {
            audioClip = Resources.Load<AudioClip>(path);
            if(cache)
            {
                audioClipDicts.Add(path, audioClip);
            }
        }
        return audioClip;
    }

    private void Update()
    {
        if(progressUpdate != null)
        {
            progressUpdate();
        }
    }
}
