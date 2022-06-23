using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Property")]
    [SerializeField] private CharacterController _characterController = null;
    [Tooltip("Player Velocity")] private Vector3 _velocity;
    [SerializeField] private float _currentSpeed = 5f;
    [SerializeField] private float _rotateDamp = 5f;
    private float _gravity = 10f;
    Transform _cameraTransform;
    bool _isDead = false;
    PlayerJoystick _joystick;

    [Header("Animator Property")]
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AnimatorType _animatorType = AnimatorType.NONE;
    [SerializeField] private AnimationClip _shotClip;
    [SerializeField] private AnimationClip _hitClip;
    float _shotTime = 0;
    float _shotCurrentTime = 0;

    public enum AnimatorType { NONE, IDLE, RUN, SHOT, HIT, DEATH }

    [Header("Attack Property")]
    [SerializeField] private Transform _shotPos;
    [SerializeField] private GameObject _arrow;

    [Header("Player Property")]
    [SerializeField] private PlayerSave _playerSave;


    void Start()
    {
        _joystick = FindObjectOfType<PlayerJoystick>();

        _cameraTransform = Camera.main.transform;

        _velocity = Vector3.zero;
        _characterController = transform.GetComponentInParent<CharacterController>();
        _shotTime = _shotClip.length;

        _playerSave = GameManager.Instance._PLAYERSAVE;

        _playerSave._HP = _playerSave._MAXHP;

        GameManager.Instance.SetAttack();
        GameManager.Instance.SetDefense();
    }

    void Update()
    {
        if (_isDead) return;

        Move();

        Attack();
      // FindingTarget();
        PlayAnimator();

        _characterController.Move(_velocity * Time.deltaTime);
    }

    void Move()
    {
        if (_shotCurrentTime > 0)
        {
            _shotCurrentTime -= Time.deltaTime;
            _velocity = Vector3.zero;
        }

        if (_shotCurrentTime > 0) return;

        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        float h = _joystick.VecMove.x;
        float v = _joystick.VecMove.z;

        Vector3 forward = _cameraTransform.forward;
        forward.y = 0;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 index = forward * v + right * h;
        index.Normalize();

        if (index == Vector3.zero) _animatorType = AnimatorType.IDLE;
        else _animatorType = AnimatorType.RUN;

        if (index != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(index), _rotateDamp * Time.deltaTime);

        index *= _currentSpeed;

        if (_characterController.isGrounded) index.y = 0;
        else index.y -= _gravity * Time.deltaTime;

        _velocity = index;
    }

    void Attack()
    {
        //if (!Input.GetMouseButtonDown(0)) return;
        //_animatorType = AnimatorType.SHOT;
    }

    public void EndHit()
    {
        _isAttacking = true;
    }

    bool _isAttacking = true;
    public void Attacking()
    {
        if (!_isAttacking) return;
        GameObject newArrow = Instantiate(_arrow, null);
        newArrow.transform.position = _shotPos.position;
        Quaternion angle = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        newArrow.transform.rotation = angle;

        newArrow.transform.GetComponent<ArrowController>()._ATTACK = _playerSave._ATTACK;
        newArrow.transform.GetComponent<ArrowController>().SendMessage("SetPlayer", newArrow.transform.position - transform.position);
    }

    [Header("View Property")]
    [SerializeField] bool DebugMode = false;
    [Range(0f, 360f)][SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    void FindingTarget()
    {
        Vector3 myPos = transform.position + Vector3.up;

        float lookingAngle = transform.eulerAngles.y;
        Vector3 lookDir = AngleToDir(lookingAngle);
        lookDir.y = 0;
        Collider[] target = Physics.OverlapSphere(myPos, ViewRadius, _targetMask);

        if (target.Length == 0) return;
        Vector3 targetPos = target[0].transform.position;
        Vector3 index = (targetPos - myPos);
        index.y = 0;
        Vector3 targetDir = index.normalized;
        float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;

        if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, targetDir.magnitude, _obstacleMask))
        {
            if (target != null)
            {
                if (index.magnitude <= 10)
                {
                    transform.LookAt(target[0].transform);
                    _animatorType = AnimatorType.SHOT;
                    PlayAnimator();
                }
            }
            return;
        }

        return;
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }


    void PlayAnimator()
    {
        switch (_animatorType)
        {
            case AnimatorType.NONE:
                break;
            case AnimatorType.IDLE:
                _animator.SetBool("isRun", false);
                break;
            case AnimatorType.RUN:
                _animator.SetBool("isRun", true);
                break;
            case AnimatorType.SHOT:
                if (_shotCurrentTime > 0) return;
                _animator.SetTrigger("Shot");
                _shotCurrentTime = _shotTime;
                break;
            case AnimatorType.HIT:
                _animator.SetTrigger("GetHit");
                _isAttacking = false;
                break;
            case AnimatorType.DEATH:
                _animator.SetTrigger("Death");
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_playerSave._HP <= 0) return;
        _playerSave._HP -= (damage - (_playerSave._DEFENSE > 20 ? 19 : _playerSave._DEFENSE) / 20);
        _animatorType = AnimatorType.HIT;
        PlayAnimator();

        if (_playerSave._HP <= 0)
        {
            _animatorType = AnimatorType.DEATH;
            PlayAnimator();
            _isDead = true;
            StageManager.instance.Death();
        }
    }


    private void OnGUI()
    {
        if (_characterController != null)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            GUILayout.Label("플레이어 이동 : WASD, 플레이어 공격 : 마우스 좌클릭", labelStyle);

            GUILayout.Label("HP : " + _playerSave._HP.ToString(), labelStyle);

            GUILayout.Label("ATTACK : " + _playerSave._ATTACK.ToString(), labelStyle);

            GUILayout.Label("DEFENSE : " + _playerSave._DEFENSE.ToString(), labelStyle);
        }
    }
}