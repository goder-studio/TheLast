using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRifle : BaseWeapon
{
    protected override void Awake()
    {
        base.Awake();
        shootModeList.Add(ShootMode.Automatic);
        shootModeList.Add(ShootMode.SingleShot);
        curShootMode = shootModeList[curShootModeIndex];
        //GameManager.Instance.SetCurrentShootMode(curShootMode);
    }

    protected override void InitCanvasInfo()
    {
        base.InitCanvasInfo();
        BattleSys.Instance.InitPlayerPanel(weaponName, ammo, curShootMode,PathDefine.AutomaticRifleIcon);
    }

    protected override void AimUpdate()
    {
        base.AimUpdate();
        if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting)
        {

            isAiming = true;
            //Start aiming
            anim.SetBool("Aim", true);

            //When right click is released
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                aimFov, fovSpeed * Time.deltaTime);

            if (!soundHasPlayed)
            {
                mainAudioSource.clip = SoundClips.aimSound;
                mainAudioSource.Play();

                soundHasPlayed = true;
            }
        }
        else
        {
            //When right click is released
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                defaultFov, fovSpeed * Time.deltaTime);

            isAiming = false;
            //Stop aiming
            anim.SetBool("Aim", false);

            soundHasPlayed = false;
        }
        //Aiming end
    }

    protected override void ReloadUpdate()
    {
        base.ReloadUpdate();
        //If out of ammo
        if (currentAmmo <= 0)
        {
            //Show out of ammo text
            BattleSys.Instance.SetCurrentWeapon("OUT OF AMMO");
            //Toggle bool
            outOfAmmo = true;
            //Auto reload if true
            if (autoReload == true && !isReloading)
            {
                StartCoroutine(AutoReload());
            }
        }
        else
        {
            //When ammo is full, show weapon name again
            BattleSys.Instance.SetCurrentWeapon(storedWeaponName);
            //Toggle bool
            outOfAmmo = false;
            //anim.SetBool ("Out Of Ammo", false);
        }

        //Reload 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting)
        {
            //Reload
            Reload();
        }
    }

    //protected override void ShootUpdate()
    //{
    //    base.ShootUpdate();

    //    if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
    //    {
    //        //Shoot automatic
    //        if (Time.time - lastFired > 1 / fireRate)
    //        {
    //            lastFired = Time.time;

    //            //Remove 1 bullet from ammo
    //            currentAmmo -= 1;

    //            shootAudioSource.clip = SoundClips.shootSound;
    //            shootAudioSource.Play();

    //            if (!isAiming) //if not aiming
    //            {
    //                anim.Play("Fire", 0, 0f);
    //                //If random muzzle is false
    //                if (!randomMuzzleflash &&
    //                    enableMuzzleflash == true)
    //                {
    //                    muzzleParticles.Emit(1);
    //                    //Light flash start
    //                    StartCoroutine(MuzzleFlashLight());
    //                }
    //                else if (randomMuzzleflash == true)
    //                {
    //                    //Only emit if random value is 1
    //                    if (randomMuzzleflashValue == 1)
    //                    {
    //                        if (enableSparks == true)
    //                        {
    //                            //Emit random amount of spark particles
    //                            sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
    //                        }
    //                        if (enableMuzzleflash == true)
    //                        {
    //                            muzzleParticles.Emit(1);
    //                            //Light flash start
    //                            StartCoroutine(MuzzleFlashLight());
    //                        }
    //                    }
    //                }
    //            }
    //            else //if aiming
    //            {

    //                anim.Play("Aim Fire", 0, 0f);

    //                //If random muzzle is false
    //                if (!randomMuzzleflash)
    //                {
    //                    muzzleParticles.Emit(1);
    //                    //If random muzzle is true
    //                }
    //                else if (randomMuzzleflash == true)
    //                {
    //                    //Only emit if random value is 1
    //                    if (randomMuzzleflashValue == 1)
    //                    {
    //                        if (enableSparks == true)
    //                        {
    //                            //Emit random amount of spark particles
    //                            sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
    //                        }
    //                        if (enableMuzzleflash == true)
    //                        {
    //                            muzzleParticles.Emit(1);
    //                            //Light flash start
    //                            StartCoroutine(MuzzleFlashLight());
    //                        }
    //                    }
    //                }
    //            }

    //            //Spawn bullet from bullet spawnpoint
    //            var bullet = (Transform)Instantiate(
    //                Prefabs.bulletPrefab,
    //                Spawnpoints.bulletSpawnPoint.transform.position,
    //                Spawnpoints.bulletSpawnPoint.transform.rotation);

    //            //Add velocity to the bullet
    //            bullet.GetComponent<Rigidbody>().velocity =
    //                bullet.transform.forward * bulletForce;

    //            //Spawn casing prefab at spawnpoint
    //            Instantiate(Prefabs.casingPrefab,
    //                Spawnpoints.casingSpawnPoint.transform.position,
    //                Spawnpoints.casingSpawnPoint.transform.rotation);
    //        }
    //    }
    //}
}
