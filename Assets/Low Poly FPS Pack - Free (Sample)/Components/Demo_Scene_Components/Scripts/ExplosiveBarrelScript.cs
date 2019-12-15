using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ----- Low Poly FPS Pack Free Version -----
public class ExplosiveBarrelScript : MonoBehaviour {

	float randomTime;
	bool routineStarted = false;

	//Used to check if the barrel 
	//has been hit and should explode 
	public bool explode = false;

	[Header("Prefabs")]
	//The explosion prefab
	public Transform explosionPrefab;
	//The destroyed barrel prefab
	public Transform destroyedBarrelPrefab;

	[Header("Customizable Options")]
	//Minimum time before the barrel explodes
	public float minTime = 0.05f;
	//Maximum time before the barrel explodes
	public float maxTime = 0.25f;

    [Header("Explosion Options")]
    public int explosiondamage = 40;
	//How far the explosion will reach
	public float explosionRadius = 12.5f;
	//How powerful the explosion is
	public float explosionForce = 4000.0f;
	
	private void Update () {
		//Generate random time based on min and max time values
		randomTime = Random.Range (minTime, maxTime);

		//If the barrel is hit
		if (explode == true) 
		{
			if (routineStarted == false) 
			{
				//Start the explode coroutine
				StartCoroutine(Explode());
				routineStarted = true;
			} 
		}
	}
	
	private IEnumerator Explode () {
		//Wait for set amount of time
		yield return new WaitForSeconds(randomTime);

		//Spawn the destroyed barrel prefab
		Instantiate (destroyedBarrelPrefab, transform.position, 
		             transform.rotation); 

		//Explosion force
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		foreach (Collider hit in colliders) {
			
			//If the barrel explosion hits other barrels with the tag "ExplosiveBarrel"
			if (hit.transform.tag == "ExplosiveBarrel") 
			{
				//Toggle the explode bool on the explosive barrel object
				hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
			}
				
			//If the explosion hit the tag "Target"
			if (hit.transform.tag == "Target") 
			{
				//Toggle the isHit bool on the target object
				hit.transform.gameObject.GetComponent<TargetScript>().isHit = true;
			}

            if(hit.transform.tag == "Enemy")
            {
                string enemyName = hit.gameObject.name;
                EntityEnemy enemy = BattleSys.Instance.GetEnemyByName(enemyName);
                if (enemy != null)
                {
                    if (enemy.Hp >  0)
                    {
                        enemy.Hp -= explosiondamage;
                        if (enemy.Hp > 0)
                        {
                            //进入受击状态
                            enemy.Hit();
                        }
                        //如果敌人血量耗尽，敌人死亡
                        else
                        {
                            enemy.Die();
                            enemy.battleMgr.RemoveEnemy(enemyName);
                        }
                    }
                       
                }
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            //Add force to nearby rigidbodies
            if (rb != null)
                rb.AddExplosionForce(explosionForce * 50, explosionPos, explosionRadius);

        }

        //Raycast downwards to check the ground tag
        RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			//Instantiate explosion prefab at hit position
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		//Destroy the current barrel object
		Destroy (gameObject);
	}
}
// ----- Low Poly FPS Pack Free Version -----