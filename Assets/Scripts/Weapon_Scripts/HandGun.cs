using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : BaseWeapon
{
    [Header("Weapon Settings")]

    public float sliderBackTimer = 1.58f;
    private bool hasStartedSliderBack;

    protected override void Awake()
    {
        base.Awake();
        shootModeList.Add(ShootMode.SingleShot);
        shootModeList.Add(ShootMode.TripleShot);
        curShootMode = shootModeList[curShootModeIndex];
        //GameManager.Instance.SetCurrentShootMode(curShootMode);
    }


    protected override void InitCanvasInfo()
    {
        base.InitCanvasInfo();
        //Set Weapon Image
        BattleSys.Instance.InitPlayerPanel(weaponName, ammo, curShootMode, PathDefine.HandGunIcon);
    }

    protected override void AimUpdate()
    {
        base.AimUpdate();
        //Aiming
        //Toggle camera FOV when right click is held down
        if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting)
        {

            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                aimFov, fovSpeed * Time.deltaTime);

            isAiming = true;

            anim.SetBool("Aim", true);

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

            //Set slider back
            anim.SetBool("Out Of Ammo Slider", true);
            //Increase layer weight for blending to slider back pose
            anim.SetLayerWeight(1, 1.0f);
        }
        else
        {
            //When ammo is full, show weapon name again
            BattleSys.Instance.SetCurrentWeapon(storedWeaponName);
            //Toggle bool
            outOfAmmo = false;
            //anim.SetBool ("Out Of Ammo", false);
            anim.SetLayerWeight(1, 0.0f);
        }
        //Reload 
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting)
        {
            //Reload
            Reload();

            if (!hasStartedSliderBack)
            {
                hasStartedSliderBack = true;
                StartCoroutine(HandgunSliderBackDelay());
            }
        }
    }

    //protected override void ShootUpdate()
    //{
    //    base.ShootUpdate();
    //    //Shooting 
    //    if (Input.GetMouseButtonDown(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
    //    {
    //        anim.Play("Fire", 0, 0f);

    //        muzzleParticles.Emit(1);

    //        //Remove 1 bullet from ammo
    //        currentAmmo -= 1;

    //        shootAudioSource.clip = SoundClips.shootSound;
    //        shootAudioSource.Play();

    //        //Light flash start
    //        StartCoroutine(MuzzleFlashLight());

    //        if (!isAiming) //if not aiming
    //        {
    //            anim.Play("Fire", 0, 0f);

    //            muzzleParticles.Emit(1);

    //            if (enableSparks == true)
    //            {
    //                //Emit random amount of spark particles
    //                sparkParticles.Emit(Random.Range(1, 6));
    //            }
    //        }
    //        else //if aiming
    //        {
    //            anim.Play("Aim Fire", 0, 0f);

    //            //If random muzzle is false
    //            if (!randomMuzzleflash)
    //            {
    //                muzzleParticles.Emit(1);
    //                //If random muzzle is true
    //            }
    //            else if (randomMuzzleflash == true)
    //            {
    //                //Only emit if random value is 1
    //                if (randomMuzzleflashValue == 1)
    //                {
    //                    if (enableSparks == true)
    //                    {
    //                        //Emit random amount of spark particles
    //                        sparkParticles.Emit(Random.Range(1, 6));
    //                    }
    //                    if (enableMuzzleflash == true)
    //                    {
    //                        muzzleParticles.Emit(1);
    //                        //Light flash start
    //                        StartCoroutine(MuzzleFlashLight());
    //                    }
    //                }
    //            }
    //        }

    //        //Spawn bullet at bullet spawnpoint
    //        var bullet = (Transform)Instantiate(
    //            Prefabs.bulletPrefab,
    //            Spawnpoints.bulletSpawnPoint.transform.position,
    //            Spawnpoints.bulletSpawnPoint.transform.rotation);

    //        //Add velocity to the bullet
    //        bullet.GetComponent<Rigidbody>().velocity =
    //        bullet.transform.forward * bulletForce;

    //        //Spawn casing prefab at spawnpoint
    //        Instantiate(Prefabs.casingPrefab,
    //            Spawnpoints.casingSpawnPoint.transform.position,
    //            Spawnpoints.casingSpawnPoint.transform.rotation);
    //    }
    //}

    private IEnumerator HandgunSliderBackDelay()
    {
        //Wait set amount of time
        yield return new WaitForSeconds(sliderBackTimer);
        //Set slider back
        anim.SetBool("Out Of Ammo Slider", false);
        //Increase layer weight for blending to slider back pose
        anim.SetLayerWeight(1, 0.0f);

        hasStartedSliderBack = false;
    }
}
