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

    [Header("Animator Property")]
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AnimatorType _animatorType = AnimatorType.NONE;
    [SerializeField] private AnimationClip _shotClip;
    float _shotTime = 0;
    float _shotCurrentTime = 0;

    public enum AnimatorType { NONE, IDLE, RUN, SHOT, HIT, DEATH }

    [Header("Attack Property")]
    [SerializeField] private Transform _shotPos;
    [SerializeField] private GameObject _arrow;

    [Header("Player Property")]
    [SerializeField] private float _hp;
    [SerializeField] private float _attack;
    public float _HP
    {
        get => _hp;
        set => _hp = value;
    }
    public float _ATTACK
    {
        get => _attack;
        set => _attack = value;
    }

    void Start()
    {
        _cameraTransform = Camera.main.transform;

        _velocity = Vector3.zero;
        _characterController = transform.GetComponentInParent<CharacterController>();
        _shotTime = _shotClip.length;

        _HP = 30f;
    }

    void Update()
    {
        if (_isDead) return;

        Move();

        Attack();
        PlayAnimator();

        _characterController.Move(_velocity * Time.deltaTime);

        if (Input.GetMouseButtonDown(1)) TakeDamage(1f);
    }

    void Move()
    {
        if (_shotCurrentTime > 0)
        {
            _shotCurrentTime -= Time.deltaTime;
            _velocity = Vector3.zero;
        }

        if (_shotCurrentTime > 0) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

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
        if (!Input.GetMouseButtonDown(0)) return;
        _animatorType = AnimatorType.SHOT;
    }

    public void Attacking()
    {
        GameObject newArrow = Instantiate(_arrow, null);
        newArrow.transform.position = _shotPos.position;
        Quaternion angle = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        newArrow.transform.rotation = angle;

        newArrow.transform.GetComponent<ArrowController>()._ATTACK = _ATTACK;
        newArrow.transform.GetComponent<ArrowController>().SendMessage("SetPlayer", transform);
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
                break;
            case AnimatorType.DEATH:
                _animator.SetTrigger("Death");
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (_HP <= 0) return;

        _HP -= damage;
        _animatorType = AnimatorType.HIT;
        PlayAnimator();

        if (_HP <= 0)
        {
            _animatorType = AnimatorType.DEATH;
            PlayAnimator();
            _isDead = true;
        }
    }


    private void OnGUI()
    {
        if (_characterController != null)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            GUILayout.Label("HP : " + _HP.ToString(), labelStyle);

            GUILayout.Label("ATTACK : " + _ATTACK.ToString(), labelStyle);
        }
    }
}