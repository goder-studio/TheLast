using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave
{
    public float delayTime;
    public int waveIndex;
    //敌人生成间隔
    public float enemySpawnInterval;
    //敌人数目
    public int enemyCount;
    //敌人属性
    public BattleProps enemyBattleProps;
}

public class BattleMgr : MonoBehaviour
{
    private StateMgr stateMgr;
    private ResSvc resSvc;

    private FpsController playerController;
    public EntityPlayer entityPlayer;

    private Dictionary<string, EntityEnemy> enemyDicts = new Dictionary<string, EntityEnemy>();
    private List<EnemyWave> enemyWaveList = new List<EnemyWave>();

    private bool canSpawnEnemy;

    private EnemyWave curEnemyWave;
    private int curWaveIndex = 0;
    private int enemyCount = 0;
    private int totalKillCount = 0;

    private float countDownTimer = 0;
    private float intervalTimer = 0;

    private void Update()
    {
        //暂停状态下，跳过Update()的逻辑处理
        if (GameManager.Instance.isPauseGame)
            return;

        foreach (EntityEnemy entityEnemy in enemyDicts.Values)
        {
            entityEnemy.TickAllLogic();
        }

        Debug.Log("CurWaveIndex：" + curWaveIndex + " EnemyCount：" + enemyDicts.Count + " TotalKillCount：" + totalKillCount); ;
        if(curWaveIndex < enemyWaveList.Count  && canSpawnEnemy)
        {
            intervalTimer += Time.deltaTime;
            if (intervalTimer >= curEnemyWave.enemySpawnInterval && enemyCount < curEnemyWave.enemyCount)
            {
                intervalTimer = 0;
                SpawnEnemy();
            }

            //一批怪物被消灭，更新数据
            if (enemyDicts.Count == 0)
            {
                if(curWaveIndex < enemyWaveList.Count - 1)
                {
                    enemyCount = 0;
                    //当前波次加一
                    curWaveIndex++;
                    //更新当前波次信息
                    curEnemyWave = enemyWaveList[curWaveIndex];
                    //SpawnEnemy();
                    canSpawnEnemy = false;
                    intervalTimer = 0;
                    //StartCoroutine(DelaySpawnEnemy(curEnemyWave.delayTime));
                    DelayTimeSpawnEnemy(curEnemyWave.delayTime);
                }
            }
        }

        if(!canSpawnEnemy)
        {
            countDownTimer -= Time.deltaTime;
            BattleSys.Instance.SetCountDown((int)countDownTimer + 1);
            if(countDownTimer <= 0)
            {
                BattleSys.Instance.HideCountDown();
                canSpawnEnemy = true;
                SpawnEnemy();
            }
        }
    }

    public void Init()
    {
        resSvc = ResSvc.Instance;

        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();

        enemyWaveList = resSvc.GetAllEnemyWaveCfgs();
        curEnemyWave = enemyWaveList[curWaveIndex];
        canSpawnEnemy = false;

        //加载主角
        LoadPlayer();
        //StartCoroutine(DelaySpawnEnemy(curEnemyWave.delayTime));
        DelayTimeSpawnEnemy(curEnemyWave.delayTime);
    }

    private void DelayTimeSpawnEnemy(float delayTime)
    {
        countDownTimer = delayTime;
        BattleSys.Instance.ShowCountDown();
    }

    private IEnumerator DelaySpawnEnemy(float delayTime)
    {
        //设置倒计时计时器
        countDownTimer = delayTime;
        //显示倒计时
        BattleSys.Instance.ShowCountDown();
        yield return new WaitForSeconds(delayTime);
        //关闭倒计时
        BattleSys.Instance.HideCountDown();
        canSpawnEnemy = true;
        SpawnEnemy();
    }

    private void LoadPlayer()
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.PlayerPrefab);
        if(player != null)
        {
            player.transform.position = new Vector3(0, 0.3f, 0);
            player.transform.rotation = Quaternion.identity;
            player.transform.localScale = Vector3.one;

            playerController = player.GetComponent<FpsController>();

            entityPlayer = new EntityPlayer
            {
                Name = player.name,
            };

            entityPlayer.SetController(playerController);

            BattleProps battleProps = new BattleProps
            {
                hp = 100,
                damage = 10,
            };

            entityPlayer.SetBattleProps(battleProps);
            
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = resSvc.LoadPrefab(PathDefine.EnemyPrefab);
        if(enemy != null)
        {
            enemy.transform.position = new Vector3(0,-1.0f,0);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.localScale = Vector3.one;
            enemy.name = "enemy" + enemyCount;
            enemyCount++;
            //设置enemy的外观
            int index = Random.Range(0, PathDefine.EnemySkins.Length);
            enemy.GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = resSvc.LoadTexture(PathDefine.EnemySkins[index]);
            EntityEnemy entityEnemy = new EntityEnemy
            {
                battleMgr = this,
                stateMgr = this.stateMgr,
                Name = enemy.name,
            };

            entityEnemy.SetAtkProps(Constant.EnemyAttackDistance, Constant.EnemyAttackAngle);
            entityEnemy.SetController(enemy.GetComponent<EnemyController>());

            BattleProps battleProps = new BattleProps
            {
                hp = curEnemyWave.enemyBattleProps.hp,
                damage = curEnemyWave.enemyBattleProps.damage,
            };
            entityEnemy.SetBattleProps(battleProps);
            entityEnemy.Born();
            //加入字典
            enemyDicts.Add(enemy.name, entityEnemy);

        }
        

    }

    /// <summary>
    /// 获取所有敌人
    /// </summary>
    /// <returns></returns>
    public List<EntityEnemy> GetAllEnemy()
    {
        List<EntityEnemy> enemyList = new List<EntityEnemy>();
        foreach(EntityEnemy enemy in enemyDicts.Values)
        {
            enemyList.Add(enemy);
        }
        return enemyList;
    }

    public EntityEnemy GetEnemyByName(string name)
    {
        foreach(string n in enemyDicts.Keys)
        {
            if(name == n)
            {
                EntityEnemy enemy = enemyDicts[n];
                return enemy;
            }
        }
        return null;
    }

    public void RemoveEnemy(string name)
    {
        EntityEnemy entityEnemy = null;
        if (enemyDicts.TryGetValue(name, out entityEnemy))
        {
            enemyDicts.Remove(name);
        }
    }

    public Dictionary<string,EntityEnemy> GetEnemyDict()
    {
        return enemyDicts;
    }

    public FpsController GetFpsController()
    {
        return playerController;
    }

    #region Effect Operaion
    public void PlayBloodEffect(Vector3 spawnPos, Quaternion rotation)
    {
        GameObject go = resSvc.LoadPrefab(PathDefine.BloodPrefab);
        go.transform.position = spawnPos;
        go.transform.rotation = rotation;
        go.AddComponent<AutoDestroy>();
    }
    #endregion

    public void AddKillCount()
    {
        totalKillCount += 1;
    }

    public int GetTotalKillCount()
    {
        return totalKillCount;
    }
}
