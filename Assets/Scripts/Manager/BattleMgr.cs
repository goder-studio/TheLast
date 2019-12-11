using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    private StateMgr stateMgr;
    private ResSvc resSvc;

    private FpsController playerController;
    public EntityPlayer entityPlayer;

    private Dictionary<string, EntityEnemy> enemyDicts = new Dictionary<string, EntityEnemy>();

    private int enemyCount = 0;
    private void Update()
    {
        foreach (EntityEnemy entityEnemy in enemyDicts.Values)
        {
            entityEnemy.TickAllLogic();

        }

        if(enemyDicts.Count == 0)
        {
            SpawnEnemy();
        }
    }

    public void Init()
    {
        resSvc = ResSvc.Instance;

        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();

        //加载主角与怪物
        LoadPlayer();
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
            BattleSys.Instance.SetPlayerPanel(player.GetComponentInChildren<PlayerPanel>());

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
                hp = 200,
                damage = 10,
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

    #region Effect Operaion
    public void PlayBloodEffect(Vector3 spawnPos, Quaternion rotation)
    {
        GameObject go = resSvc.LoadPrefab(PathDefine.BloodPrefab);
        go.transform.position = spawnPos;
        go.transform.rotation = rotation;
        go.AddComponent<AutoDestroy>();
    }
    #endregion
}
