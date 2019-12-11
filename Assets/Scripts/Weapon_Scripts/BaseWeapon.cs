using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseWeapon : MonoBehaviour
{
    protected Animator anim;

    [Header("Gun Camera")]
    //Main gun camera
    public Camera gunCamera;

    [Header("Gun Camera Options")]
    [Tooltip("当瞄准时相机视野的变化速度")]
    public float fovSpeed;
    //Default camera field of view
    [Tooltip("默认的相机视野 (40 is recommended).")]
    public float defaultFov;
    public float aimFov;

    [Header("UI Weapon Name")]
    [Tooltip("当前武器名称，用于在UI上显示")]
    public string weaponName;
    protected string storedWeaponName;

    [Header("Weapon Sway")]
    //Enables weapon sway
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount;
    public float maxSwayAmount;
    public float swaySmoothValue;

    protected Vector3 initialSwayPosition;

    protected float lastFired;
    [Header("Weapon Setting")]
    [Tooltip("武器的射击模式")]
    protected ShootMode curShootMode;
    //当前武器可以切换的射击模式
    protected List<ShootMode> shootModeList = new List<ShootMode>();
    protected int curShootModeIndex = 0;
    [Tooltip("武器的开火速度，越高的值说明射速越快")]
    public float fireRate;
    [Tooltip("武器的连射速度，比开火速度略快")]
    public float dartleRate;
    protected bool isTripleShooting;
    [Tooltip("是否启用当子弹耗尽时自动装弹")]
    public bool autoReload;
    //Delay between shooting last bullet and reloading
    public float autoReloadDelay;
    //Check if reloading
    protected bool isReloading;

    //Holstering weapon
    protected bool hasBeenHolstered = false;
    //If weapon is holstered
    protected bool holstered;
    //Check if running
    protected bool isRunning;
    //Check if aiming
    protected bool isAiming;
    //Check if walking
    protected bool isWalking;
    //Check if inspecting weapon
    protected bool isInspecting;

    //How much ammo is currently left
    protected int currentAmmo;
    [Tooltip("武器的弹夹容量")]
    public int ammo;
    //Check if out of ammo
    protected bool outOfAmmo;

    [Header("Bullet Settings")]
    //Bullet
    [Tooltip("子弹攻击力")]
    public int damage;
    [Tooltip("子弹射出时需要给子弹施加多大的力")]
    public float bulletForce;
    [Tooltip("How long after reloading that the bullet model becomes visible " +
        "again, only used for out of ammo reload animations.")]
    public float showBulletInMagDelay;
    [Tooltip("The bullet model inside the mag, not used for all weapons.")]
    public SkinnedMeshRenderer bulletInMagRenderer;

    [Header("Grenade Settings")]
    public float grenadeSpawnDelay;

    [Header("Muzzleflash Settings")]
    public bool randomMuzzleflash = false;
    //min should always bee 1
    protected int minRandomValue;

    [Range(2, 25)]
    public int maxRandomValue = 5;

    protected int randomMuzzleflashValue;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleflashLight;
    public float lightDuration;

    [Header("Audio Source")]
    //Main audio source
    public AudioSource mainAudioSource;
    //Audio source used for shoot sound
    public AudioSource shootAudioSource;

    [System.Serializable]
    public class prefabs
    {
        [Header("Prefabs")]
        public Transform bulletPrefab;
        public Transform casingPrefab;
        public Transform grenadePrefab;
    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
        [Header("Spawnpoints")]
        //Array holding casing spawn points 
        //(some weapons use more than one casing spawn)
        //Casing spawn point array
        public Transform casingSpawnPoint;
        //Bullet prefab spawn from this point
        public Transform bulletSpawnPoint;

        public Transform grenadeSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip aimSound;
    }
    public soundClips SoundClips;

    protected bool soundHasPlayed = false;

    protected virtual void Awake()
    {
        //Set the animator component
        anim = GetComponent<Animator>();
        //Set current ammo to total ammo value
        currentAmmo = ammo;
        muzzleflashLight.enabled = false;
    }

    protected virtual void Start()
    {
        InitCanvasInfo();
        //Weapon sway
        initialSwayPosition = transform.localPosition;

        //Set the shoot sound to audio source
        shootAudioSource.clip = SoundClips.shootSound;
    }

    protected virtual void OnEnable()
    {
        InitCanvasInfo();
    }


    protected virtual void LateUpdate()
    {
        SwayWeapon();
    }

    protected virtual void Update()
    {

        AimUpdate();
        //If randomize muzzleflash is true, genereate random int values
        if (randomMuzzleflash == true)
        {
            randomMuzzleflashValue = Random.Range(minRandomValue, maxRandomValue);
        }
        //Set current ammo text from ammo int
        BattleSys.Instance.SetCurrentAmmo(currentAmmo);

        AnimationCheck();
        SwitchShootMode();

        UseKnifeUpdate();
        ThrowGrenadeUpdate();
        ReloadUpdate();
        ShootUpdate();
        InspectUpdate();
        ToggleWeaponHolsterUpdate();
        MoveUpdate();
    }

    protected virtual void InitCanvasInfo() {
        //Save the weapon name
        storedWeaponName = weaponName;
    }
    
    protected virtual void SwayWeapon()
    {
        //Weapon sway
        if (weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
            //Clamp movement to min and max values
            movementX = Mathf.Clamp
                (movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp
                (movementY, -maxSwayAmount, maxSwayAmount);
            //Lerp local pos
            Vector3 finalSwayPosition = new Vector3
                (movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp
                (transform.localPosition, finalSwayPosition +
                initialSwayPosition, Time.deltaTime * swaySmoothValue);
        }
    }

    /// <summary>
    /// 使用小刀操作
    /// </summary>
    protected virtual void UseKnifeUpdate()
    {
        //Play knife attack 1 animation when Q key is pressed
        if (Input.GetKeyDown(KeyCode.Q) && !isInspecting)
        {
            anim.Play("Knife Attack 1", 0, 0f);
        }
        //Play knife attack 2 animation when F key is pressed
        if (Input.GetKeyDown(KeyCode.F) && !isInspecting)
        {
            anim.Play("Knife Attack 2", 0, 0f);
        }

    }

    /// <summary>
    /// 投掷手雷操作
    /// </summary>
    protected virtual void ThrowGrenadeUpdate()
    {
        //Throw grenade when pressing G key
        if (Input.GetKeyDown(KeyCode.G) && !isInspecting)
        {
            StartCoroutine(GrenadeSpawnDelay());
            //Play grenade throw animation
            anim.Play("GrenadeThrow", 0, 0.0f);
        }
    }

    /// <summary>
    /// 观察武器操作
    /// </summary>
    protected virtual void InspectUpdate()
    {
        //Inspect weapon when pressing T key
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Inspect");
        }
    }

    protected virtual void ToggleWeaponHolsterUpdate()
    {
        //Toggle weapon holster when E key is pressed
        if (Input.GetKeyDown(KeyCode.E) && !hasBeenHolstered)
        {
            holstered = true;

            mainAudioSource.clip = SoundClips.holsterSound;
            mainAudioSource.Play();

            hasBeenHolstered = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasBeenHolstered)
        {
            holstered = false;

            mainAudioSource.clip = SoundClips.takeOutSound;
            mainAudioSource.Play();

            hasBeenHolstered = false;
        }
        //Holster anim toggle
        if (holstered == true)
        {
            anim.SetBool("Holster", true);
        }
        else
        {
            anim.SetBool("Holster", false);
        }
    }

    /// <summary>
    /// 移动操作
    /// </summary>
    protected virtual void MoveUpdate()
    {
        //Walking when pressing down WASD keys
        if (Input.GetKey(KeyCode.W) && !isRunning ||
            Input.GetKey(KeyCode.A) && !isRunning ||
            Input.GetKey(KeyCode.S) && !isRunning ||
            Input.GetKey(KeyCode.D) && !isRunning)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        //Running when pressing down W and Left Shift key
        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        //Run anim toggle
        if (isRunning == true)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    /// <summary>
    /// 瞄准操作
    /// </summary>
    protected virtual void AimUpdate()
    {
    }

    /// <summary>
    /// 装弹操作
    /// </summary>
    protected virtual void ReloadUpdate()
    {

    }

    /// <summary>
    /// 射击操作
    /// </summary>
    protected virtual void ShootUpdate()
    {
        switch (curShootMode)
        {
            case ShootMode.SingleShot:
                SingleShoot();
                break;
            case ShootMode.TripleShot:
                TripleShoot();
                break;
            case ShootMode.Automatic:
                AutomaticShoot();
                break;
            default:
                break;
        }

    }

    protected virtual void SingleShoot()
    {
        if(Input.GetMouseButtonDown(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
        {
            ShootOneBullet();
            //anim.Play("Fire", 0, 0f);

            //muzzleParticles.Emit(1);
            ////子弹减少
            //currentAmmo -= 1;

            ////播放射击音效
            //shootAudioSource.clip = SoundClips.shootSound;
            //shootAudioSource.Play();

            ////枪口闪光
            //StartCoroutine(MuzzleFlashLight());

            //if(!isAiming)
            //{
            //    anim.Play("Fire", 0, 0f);

            //    muzzleParticles.Emit(1);

            //    if(enableSparks == true)
            //    {
            //        sparkParticles.Emit(Random.Range(1, 6));
            //    }
            //}
            //else
            //{
            //    anim.Play("Aim Fire", 0, 0f);

            //    if(!randomMuzzleflash)
            //    {
            //        muzzleParticles.Emit(1);
            //    }
            //    else if(randomMuzzleflash)
            //    {
            //        if(randomMuzzleflashValue == 1)
            //        {
            //            if(enableSparks)
            //            {
            //                sparkParticles.Emit(Random.Range(1, 6));
            //            }
            //            if(enableMuzzleflash)
            //            {
            //                muzzleParticles.Emit(1);
            //                StartCoroutine(MuzzleFlashLight());
            //            }
            //        }
            //    }
            //}
            ////Spawn bullet at bullet spawnpoint
            //var bullet = (Transform)Instantiate(
            //    Prefabs.bulletPrefab,
            //    Spawnpoints.bulletSpawnPoint.transform.position,
            //    Spawnpoints.bulletSpawnPoint.transform.rotation);

            ////Add velocity to the bullet
            //bullet.GetComponent<Rigidbody>().velocity =
            //bullet.transform.forward * bulletForce;

            ////Spawn casing prefab at spawnpoint
            //Instantiate(Prefabs.casingPrefab,
            //    Spawnpoints.casingSpawnPoint.transform.position,
            //    Spawnpoints.casingSpawnPoint.transform.rotation);
        }
    }

    protected virtual void TripleShoot()
    {
        if(Input.GetMouseButtonDown(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && !isTripleShooting)
        {
            isTripleShooting = true;
            int count = currentAmmo >= 3 ? 3 : currentAmmo;
            StartCoroutine(TripleShootCoroutine(count));
        }
    }


    protected virtual IEnumerator TripleShootCoroutine(int round)
    {
        ShootOneBullet();
        for (int i = 0; i < round - 1; i++)
        {
            yield return new WaitForSeconds(1 / dartleRate);
            ShootOneBullet();
        }
        isTripleShooting = false;
    }


    protected virtual void AutomaticShoot()
    {
        if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning)
        {
            //Shoot automatic
            if (Time.time - lastFired > 1 / fireRate)
            {
                lastFired = Time.time;

                //Remove 1 bullet from ammo
                currentAmmo -= 1;

                shootAudioSource.clip = SoundClips.shootSound;
                shootAudioSource.Play();

                if (!isAiming) //if not aiming
                {
                    anim.Play("Fire", 0, 0f);
                    //If random muzzle is false
                    if (!randomMuzzleflash &&
                        enableMuzzleflash == true)
                    {
                        muzzleParticles.Emit(1);
                        //Light flash start
                        StartCoroutine(MuzzleFlashLight());
                    }
                    else if (randomMuzzleflash == true)
                    {
                        //Only emit if random value is 1
                        if (randomMuzzleflashValue == 1)
                        {
                            if (enableSparks == true)
                            {
                                //Emit random amount of spark particles
                                sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                            }
                            if (enableMuzzleflash == true)
                            {
                                muzzleParticles.Emit(1);
                                //Light flash start
                                StartCoroutine(MuzzleFlashLight());
                            }
                        }
                    }
                }
                else //if aiming
                {

                    anim.Play("Aim Fire", 0, 0f);

                    //If random muzzle is false
                    if (!randomMuzzleflash)
                    {
                        muzzleParticles.Emit(1);
                        //If random muzzle is true
                    }
                    else if (randomMuzzleflash == true)
                    {
                        //Only emit if random value is 1
                        if (randomMuzzleflashValue == 1)
                        {
                            if (enableSparks == true)
                            {
                                //Emit random amount of spark particles
                                sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                            }
                            if (enableMuzzleflash == true)
                            {
                                muzzleParticles.Emit(1);
                                //Light flash start
                                StartCoroutine(MuzzleFlashLight());
                            }
                        }
                    }
                }

                //Spawn bullet from bullet spawnpoint
                var bullet = (Transform)Instantiate(
                    Prefabs.bulletPrefab,
                    Spawnpoints.bulletSpawnPoint.transform.position,
                    Spawnpoints.bulletSpawnPoint.transform.rotation);

                //Add velocity to the bullet
                bullet.GetComponent<Rigidbody>().velocity =
                    bullet.transform.forward * bulletForce;

                //Spawn casing prefab at spawnpoint
                Instantiate(Prefabs.casingPrefab,
                    Spawnpoints.casingSpawnPoint.transform.position,
                    Spawnpoints.casingSpawnPoint.transform.rotation);
            }
        }
    }


    protected virtual void ShootOneBullet()
    {
        anim.Play("Fire", 0, 0f);

        muzzleParticles.Emit(1);
        //子弹减少
        currentAmmo -= 1;

        //播放射击音效
        shootAudioSource.clip = SoundClips.shootSound;
        shootAudioSource.Play();

        //枪口闪光
        StartCoroutine(MuzzleFlashLight());

        if (!isAiming)
        {
            anim.Play("Fire", 0, 0f);

            muzzleParticles.Emit(1);

            if (enableSparks == true)
            {
                sparkParticles.Emit(Random.Range(1, 6));
            }
        }
        else
        {
            anim.Play("Aim Fire", 0, 0f);

            if (!randomMuzzleflash)
            {
                muzzleParticles.Emit(1);
            }
            else if (randomMuzzleflash)
            {
                if (randomMuzzleflashValue == 1)
                {
                    if (enableSparks)
                    {
                        sparkParticles.Emit(Random.Range(1, 6));
                    }
                    if (enableMuzzleflash)
                    {
                        muzzleParticles.Emit(1);
                        StartCoroutine(MuzzleFlashLight());
                    }
                }
            }
        }
        //Spawn bullet at bullet spawnpoint
        var bullet = (Transform)Instantiate(
            Prefabs.bulletPrefab,
            Spawnpoints.bulletSpawnPoint.transform.position,
            Spawnpoints.bulletSpawnPoint.transform.rotation);

        //Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity =
        bullet.transform.forward * bulletForce;

        //Spawn casing prefab at spawnpoint
        Instantiate(Prefabs.casingPrefab,
            Spawnpoints.casingSpawnPoint.transform.position,
            Spawnpoints.casingSpawnPoint.transform.rotation);
    }
    /// <summary>
    /// 切换射击模式
    /// </summary>
    public virtual void SwitchShootMode()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            int length = shootModeList.Count;
            curShootModeIndex++;
            curShootModeIndex = curShootModeIndex % length;
            curShootMode = shootModeList[curShootModeIndex];
            BattleSys.Instance.SetCurrentShootMode(curShootMode);
            BattleSys.Instance.ShowSwitchShootModeTips(curShootMode);
        }
    }

    protected virtual IEnumerator GrenadeSpawnDelay()
    {
        //Wait for set amount of time before spawning grenade
        yield return new WaitForSeconds(grenadeSpawnDelay);
        //Spawn grenade prefab at spawnpoint
        Instantiate(Prefabs.grenadePrefab,
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
    }

    protected virtual IEnumerator AutoReload()
    {
        //Wait set amount of time
        yield return new WaitForSeconds(autoReloadDelay);

        if (outOfAmmo == true)
        {
            //Play diff anim if out of ammo
            anim.Play("Reload Out Of Ammo", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
            mainAudioSource.Play();

            //If out of ammo, hide the bullet renderer in the mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = false;
                //Start show bullet delay
                StartCoroutine(ShowBulletInMag());
            }
        }
    }

    //Reload
    protected virtual void Reload()
    {
        if (outOfAmmo == true)
        {
            //Play diff anim if out of ammo
            anim.Play("Reload Out Of Ammo", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
            mainAudioSource.Play();

            //If out of ammo, hide the bullet renderer in the mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = false;
                //Start show bullet delay
                StartCoroutine(ShowBulletInMag());
            }
        }
        else
        {
            //Play diff anim if ammo left
            anim.Play("Reload Ammo Left", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
            mainAudioSource.Play();

            //If reloading when ammo left, show bullet in mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = true;
            }
        }

    }


    //Enable bullet in mag renderer after set amount of time
    protected IEnumerator ShowBulletInMag()
    {
        //Wait set amount of time before showing bullet in mag
        yield return new WaitForSeconds(showBulletInMagDelay);
        bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    protected IEnumerator MuzzleFlashLight()
    {
        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }

    protected void AnimationCheck()
    {
        //Check if reloading
        //Check both animations
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Reload Out Of Ammo") ||info.IsName("Reload Ammo Left"))
        {
            isReloading = true;
            if(info.normalizedTime >= 1.0f)
            {
                currentAmmo = ammo;
                outOfAmmo = false;
            }
        }
        else
        {
            isReloading = false;
        }

        //Check if inspecting weapon
        if (info.IsName("Inspect"))
        {
            isInspecting = true;
        }
        else
        {
            isInspecting = false;
        }
    }

    public void ShakeCamera(float duration,float strength = 3,int vibrato = 10,float randomness = 90)
    {
        gameObject.GetComponentInChildren<Camera>().DOShakePosition(duration, strength, vibrato, randomness);
    }
}
