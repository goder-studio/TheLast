using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ----- Low Poly FPS Pack Free Version -----
public class BulletScript : MonoBehaviour {

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform [] metalImpactPrefabs;

	private void Start () 
	{
		//Start destroy timer
		StartCoroutine (DestroyAfter ());
	}

	//If the bullet collides with anything
	private void OnCollisionEnter (Collision collision) 
	{
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		else 
		{
			Destroy (gameObject);
		}

		//If bullet collides with "Metal" tag
		if (collision.transform.tag == "Metal" || collision.transform.tag == "Env") 
		{
			//生成弹痕
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, metalImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			Destroy(gameObject);
		}

		//If bullet collides with "Target" tag
		if (collision.transform.tag == "Target") 
		{
			//Toggle "isHit" on target object
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
			Destroy(gameObject);
		}
			
		//If bullet collides with "ExplosiveBarrel" tag
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			//Toggle "explode" on explosive barrel object
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

        if(collision.transform.tag == "Enemy")
        {
            string enemyName = collision.gameObject.name;
            //Dictionary<string, EntityEnemy> enemyDict = GameManager.Instance.GetEnemyDict();
            EntityPlayer player = BattleSys.Instance.GetPlayer();
            //获取被子弹达到的敌人逻辑实体
            EntityEnemy enemy = BattleSys.Instance.GetEnemyByName(enemyName);
            if(enemy != null)
            {
                //进入受击状态
                enemy.Hit();
                //敌人扣血
                enemy.Hp -= player.Props.damage;
                //如果敌人血量耗尽，敌人死亡
                if (enemy.Hp <= 0)
                {
                    enemy.Die();
                    enemy.battleMgr.RemoveEnemy(enemyName);
                    //GameManager.Instance.RemoveEnemy(enemy.Name);
                }
                //获取碰撞点
                ContactPoint contact = collision.contacts[0];
                //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Quaternion rotation = Quaternion.LookRotation(contact.normal);
                //Debug.Log("Rotation:" + rotation.eulerAngles + "Normal:" + contact.normal);
                //播放流血的粒子动画TODO
                enemy.PlayBloodEffect(contact.point, rotation);
                //销毁子弹
                Destroy(gameObject);

            }
        }
	}

	private IEnumerator DestroyTimer () 
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		//Wait for set amount of time
		yield return new WaitForSeconds (destroyAfter);
		//Destroy bullet object
		Destroy (gameObject);
	}
}
// ----- Low Poly FPS Pack Free Version -----