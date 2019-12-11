
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ShootMode
{
    //单发
    SingleShot,
    //三连发
    TripleShot,
    //全自动
    Automatic,
}


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class FpsController : MonoBehaviour
{
    [Header("Arms")]
    [Tooltip("持有枪摄像头的游戏物体的transform组件"), SerializeField]
    private Transform arms;

    [Tooltip("武器和枪摄像头相对于fps控制器游戏物体的位置"), SerializeField]
    private Vector3 armPosition;

    [Header("Weapon")]
    [Tooltip("存储可用武器的游戏物体"), SerializeField]
    private GameObject[] weapons;

    [Header("Audio Clips")]
    [Tooltip("玩家走路时播放的音效"),SerializeField]
    private AudioClip walkingSound;

    [Tooltip("玩家跑步时播放的音效"), SerializeField]
    private AudioClip runningSound;

    [Tooltip("玩家受击时播放的音效"), SerializeField]
    private AudioClip hitSound;

    [Header("Movement Setting")]
    [Tooltip("玩家行走或瞄准时的移动速度"), SerializeField]
    private float walkingSpeed = 5.0f;

    [Tooltip("玩家奔跑时的移动速度"), SerializeField]
    private float runningSpeed = 8.0f;

    [Tooltip("玩家达到目标步行或奔跑速度所需要的时间"), SerializeField]
    private float movementSmoothness = 0.125f;

    [Tooltip("玩家跳跃时所施加的力"), SerializeField]
    private float jumpForce = 35.0f;

    [Header("LookSetting")]
    [Tooltip("fps控制器的转向速度"), SerializeField]
    private float mouseSensitivity = 7.0f;

    [Tooltip("fps达到目标转向速度的时间"), SerializeField]
    private float rotationSmoothness = 0.05f;

    [Tooltip("相机在垂直方向上能达到的最小角度"), SerializeField]
    private float minVerticalAngle = -90.0f;

    [Tooltip("相机在垂直方向上能达到的最大角度"), SerializeField]
    private float maxVerticalAngle = 90.0f;

    [Header("Fps Input")]
    [Tooltip("用户的输入设置"), SerializeField]
    private FpsInput fpsInput;

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private AudioSource _audioSource;
    private SmoothRotation _rotationX;
    private SmoothRotation _rotationY;
    private SmoothVelocity _velocityX;
    private SmoothVelocity _velocityZ;
    private bool _isGrounded;
    private bool canSwitchWeapon;
    private int curWeaponIndex;
    private BaseWeapon curWeapon;

    private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
    private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];

    private EntityPlayer entity;
    public EntityPlayer Entity
    {
        get { return entity; }
        set { entity = value; }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _collider = GetComponent<CapsuleCollider>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = walkingSound;
        _audioSource.loop = true;
        canSwitchWeapon = true;
        arms = AssignCharacterCamera();
        _rotationX = new SmoothRotation(RotationXRaw);
        _rotationY = new SmoothRotation(RotationYRaw);
        _velocityX = new SmoothVelocity();
        _velocityZ = new SmoothVelocity();
        //隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ValidateRotationRestriction();
        SetCurWeapon(0);
    }

    private Transform AssignCharacterCamera()
    {
        Transform t = this.transform;
        arms.SetPositionAndRotation(t.position, t.rotation);
        return arms;
    }

    private void SetCurWeapon(int index)
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            if(i == index)
            {
                weapons[i].SetActive(true);
                curWeapon = weapons[i].GetComponent<BaseWeapon>();
                //更新攻击力
                entity.Props.damage = curWeapon.damage;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 验证旋转限制
    /// </summary>
    private void ValidateRotationRestriction()
    {
        minVerticalAngle = ClampRotationRestriction(minVerticalAngle, -90, 90);
        maxVerticalAngle = ClampRotationRestriction(maxVerticalAngle, -90, 90);
        if (maxVerticalAngle >= minVerticalAngle) return;
        Debug.LogWarning("maxVerticalAngle should be greater than minVerticalAngle");
        //如果最大值比最小值小，交换
        float temp = minVerticalAngle;
        minVerticalAngle = maxVerticalAngle;
        maxVerticalAngle = temp;
    }

    /// <summary>
    /// 将旋转限制在规定范围内
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private float ClampRotationRestriction(float rotation,float min,float max)
    {
        if (rotation >= min && rotation <= max)
            return rotation;
        var message = String.Format("Rotation Should Between {0} And {1} Degrees", min, max);
        Debug.LogWarning(message);
        return Mathf.Clamp(rotation, min, max);
    }

    /// <summary>
    /// 检测人物是否在地面静止
    /// </summary>
    private void OnCollisionStay()
    {
        Bounds bounds = _collider.bounds;
        //bounds.extents表示的是_collider的center距离x,y,z方向边界的距离
        Vector3 extents = bounds.extents;
        float radius = extents.x - 0.01f;
        Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down, _groundCastResults, extents.y - radius * 0.5f,~0,QueryTriggerInteraction.Ignore);
        //如果没有与地面接触，返回
        if (!_groundCastResults.Any(hit => hit.collider != null && hit.collider != _collider))
            return;

        for (int i = 0; i < _groundCastResults.Length; i++)
        {
            _groundCastResults[i] = new RaycastHit();
        }

        _isGrounded = true;
    }

    /// <summary>
    /// 在FixedUpdate里执行游戏物理的运算，如人物移动，视角旋转等
    /// </summary>
    private void FixedUpdate()
    {
        RotateCameraAndCharacter();
        MoveCharacter();
        _isGrounded = false;
    }

    private void Update()
    {
        Jump();
        PlayFootStepSound();
        SwitchWeapon();
    }


    private void SwitchWeapon()
    {
        if(fpsInput.SwitchWeapon > 0 && canSwitchWeapon)
        {
            canSwitchWeapon = false;
            curWeaponIndex++;
            if(curWeaponIndex > weapons.Length - 1)
            {
                curWeaponIndex = 0;
            }
            SetCurWeapon(curWeaponIndex);
        }
        else if(fpsInput.SwitchWeapon < 0 && canSwitchWeapon)
        {
            canSwitchWeapon = false;
            curWeaponIndex--;
            if(curWeaponIndex < 0)
            {
                curWeaponIndex = weapons.Length - 1;
            }
            SetCurWeapon(curWeaponIndex);
        }
        else if(fpsInput.SwitchWeapon == 0)
        {
            canSwitchWeapon = true;
        }
    }

    /// <summary>
    /// 旋转主角和相机
    /// </summary>
    private void RotateCameraAndCharacter()
    {
        var rotationX = _rotationX.Update(RotationXRaw, rotationSmoothness);
        var rotationY = _rotationY.Update(RotationYRaw, rotationSmoothness);
        var clampedY = RestrictVerticalRotation(rotationY);
        _rotationY.Current = clampedY;
        var worldUp = arms.InverseTransformDirection(Vector3.up);
        //将arms绕worldUp旋转rotationX度，绕Vector3.Left旋转rotationY度
        var rotation = arms.rotation * Quaternion.AngleAxis(rotationX, worldUp) * Quaternion.AngleAxis(clampedY, Vector3.left);
        transform.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        arms.rotation = rotation;
    }

    private void MyRotateCameraAndCharacter()
    {
        var rotationX = NormalizeAngle(transform.eulerAngles.y) + _rotationX.Update(RotationXRaw, rotationSmoothness);
        var rotationY = NormalizeAngle(arms.eulerAngles.x) - _rotationY.Update(RotationYRaw, rotationSmoothness);
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle , maxVerticalAngle );

        transform.eulerAngles = new Vector3(0, rotationX, 0);
        arms.eulerAngles =  new Vector3(rotationY, rotationX,0);
    }
    

    /// <summary>
    /// 限制每帧垂直方向的旋转角度
    /// </summary>
    /// <param name="mouseY"></param>
    /// <returns></returns>
    private float RestrictVerticalRotation(float mouseY)
    {
        var currentAngle = NormalizeAngle(arms.eulerAngles.x);
        var minY = minVerticalAngle + currentAngle;
        var maxY = maxVerticalAngle + currentAngle;
        return Mathf.Clamp(mouseY,minY + 0.01f,maxY - 0.01f);
    }

    private float NormalizeAngle(float angleDegress)
    {
        while(angleDegress > 180.0f)
        {
            angleDegress -= 360.0f;
        }

        while(angleDegress  <= -180.0f)
        {
            angleDegress += 360.0f;
        }

        return angleDegress;
    }

    private void MoveCharacter()
    {
        var direction = new Vector3(fpsInput.Move, 0, fpsInput.Strafe).normalized;
        var worldDirection = transform.TransformDirection(direction);
        var velocity = worldDirection * (fpsInput.Run ? runningSpeed : walkingSpeed);
        //TODO
        var intersectsWall = CheckCollisionsWithWalls(velocity);
        if(intersectsWall)
        {
            _velocityX.Current = _velocityZ.Current = 0;
            return;
        }

       var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
       var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
       var rigidbodyVelocity = _rigidbody.velocity;
       var force = new Vector3(smoothX - rigidbodyVelocity.x, 0, smoothZ - rigidbodyVelocity.z);
       _rigidbody.AddForce(force, ForceMode.VelocityChange);
      

     // var velocityLocal = direction * (fpsInput.Run ? runningSpeed : walkingSpeed);
     // var smoothX = _velocityX.Update(velocityLocal.x, movementSmoothness);
     // var smoothZ = _velocityZ.Update(velocityLocal.z, movementSmoothness);
     // var rigidbodyVelocity = _rigidbody.velocity;
     // var force = new Vector3(smoothX - rigidbodyVelocity.x, 0, smoothZ - rigidbodyVelocity.z);
     // _rigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
        
    }
    
    private bool CheckCollisionsWithWalls(Vector3 velocity)
    {
        if (_isGrounded)
            return false;
        var bounds = _collider.bounds;
        var radius = _collider.radius;
        var halfHeight = _collider.height * 0.5f - radius * 1.0f;
        var point1 = bounds.center;
        point1.y += halfHeight;
        var point2 = bounds.center;
        point2.y -= halfHeight;
        Physics.CapsuleCastNonAlloc(point1, point2, radius, velocity.normalized, _wallCastResults, radius * 0.04f, ~0, QueryTriggerInteraction.Ignore);
        var collides = _wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider);
        if (!collides) return false;
        for(var i = 0; i < _wallCastResults.Length; i++)
        {
            _wallCastResults[i] = new RaycastHit();
        }
        return true;
    }

    private void Jump()
    {
        if (!_isGrounded || !fpsInput.Jump)
            return;
        _isGrounded = false;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void PlayFootStepSound()
    {
        if(_isGrounded && _rigidbody.velocity.sqrMagnitude > 0.1f)
        {
            AudioClip clip = fpsInput.Run ? runningSound : walkingSound;
            _audioSource.clip = clip;
            if(!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            if(_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }
    }

    public BaseWeapon GetCurWeapon()
    {
        return curWeapon;
    }
    private float RotationXRaw
    {
        get { return fpsInput.RotateX * mouseSensitivity; }
    }

    private float RotationYRaw
    {
        get { return fpsInput.RotateY * mouseSensitivity; }
    }


    private class SmoothRotation
    {
        private float _current;
        private float _currentVelocity;

        public SmoothRotation(float startAngle)
        {
            _current = startAngle;
        }

        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }

        public float Current
        {
            set { _current = value; }
        }
    }

    private class SmoothVelocity
    {
        private float _current;
        private float _currentVelocity;

        public float Update(float target,float smoothTime)
        {
            return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);    
        }

        public float Current
        {
            set { _current = value; }
        }
    }

   [Serializable]
    private class FpsInput
    {
        [Tooltip("用于将摄像机绕y轴旋转的虚拟轴的名称"), SerializeField]
        private string rotateX = "Mouse X";
        [Tooltip("用于将摄像机绕x轴旋转的虚拟轴的名称"), SerializeField]
        private string rotateY = "Mouse Y";
        [Tooltip("用于玩家左右移动的虚拟轴的名称"), SerializeField]
        private string move = "Horizontal";
        [Tooltip("用于玩家前后移动的虚拟轴的名称"), SerializeField]
        private string strafe= "Vertical";
        [Tooltip("用于切换武器的虚拟轴的名称"), SerializeField]
        private string switchWeapon = "Mouse ScrollWheel";
        [Tooltip("用于玩家奔跑的虚拟按键"), SerializeField]
        private string run = "Fire3";
        [Tooltip("用于玩家跳跃的虚拟按键"), SerializeField]
        private string jump = "Jump";

        public float RotateX
        {
            get { return Input.GetAxisRaw(rotateX); }
        }

        public float RotateY
        {
            get { return Input.GetAxisRaw(rotateY); }
        }

        public float Move
        {
            get { return Input.GetAxisRaw(move); }
        }

        public float Strafe
        {
            get { return Input.GetAxisRaw(strafe); }
        }

        public float SwitchWeapon
        {
            get { return Input.GetAxisRaw(switchWeapon); }
        }

        public bool Run
        {
            get { return Input.GetButton(run); }
        }

        public bool Jump
        {
            get { return Input.GetButtonDown(jump); }
        }

    }
}


