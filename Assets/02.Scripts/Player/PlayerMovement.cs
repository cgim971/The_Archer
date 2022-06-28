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

    [SerializeField] private Renderer[] _skinParts;

    [Header("Animator Property")]
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AnimatorType _animatorType = AnimatorType.NONE;
    [SerializeField] private AnimationClip _shotClip;
    [SerializeField] private AnimationClip _hitClip;
    float _shotTime = 0;
    float _shotCurrentTime = 0;

    [SerializeField] private GameObject _dieEffect;
    [SerializeField] private GameObject _effect;

    public enum AnimatorType { NONE, IDLE, RUN, SHOT, HIT, DEATH }

    [Header("Attack Property")]
    [SerializeField] private Transform _shotPos;
    [SerializeField] private GameObject _arrow;

    [Header("Player Property")]
    [SerializeField] private PlayerSave _playerSave;

    bool _isStart = false;
    bool _isEnd = false;

    GameObject target = null;

    [SerializeField] List<GameObject> enemys;
    void Attack()
    {
        if (_velocity != Vector3.zero) return;
        target = enemys[0];
        if (target == null) return;

        foreach (GameObject enemy in enemys)
        {
            float distance = (enemy.transform.position - transform.position).magnitude;

            if (distance < (target.transform.position - transform.position).magnitude)
            {
                target = enemy;
            }
        }

        target.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        transform.LookAt(target.transform.position);

        _animatorType = AnimatorType.SHOT;
    }

    void Start()
    {
        _joystick = FindObjectOfType<PlayerJoystick>();

        _cameraTransform = Camera.main.transform;

        _velocity = Vector3.zero;
        _characterController = transform.GetComponentInParent<CharacterController>();
        _shotTime = _shotClip.length;

        _playerSave = GameManager.Instance._PLAYERSAVE;

        Equip();

        StartCoroutine(StartTime());
    }

    void Equip()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i == 1) continue;
            _skinParts[i].gameObject.SetActive(false);
        }

        foreach (ItemSave itemSave in GameManager.Instance._PLAYERSAVE._itemList._itemSaves)
        {
            if (itemSave._EQUIPMENTITEM)
            {
                _skinParts[(int)itemSave._ITEMTYPE - 1].gameObject.SetActive(true);
                _skinParts[(int)itemSave._ITEMTYPE - 1].GetComponent<Renderer>().material = GameManager.Instance.Materials((int)itemSave._ITEMTYPE, (int)itemSave._ITEMTIER - 1);
            }
        }
    }

    IEnumerator StartTime()
    {
        for (int i = 3; i > 0; i--)
        {
            PlayerUI.instance.Count(i);
            yield return new WaitForSeconds(1f);
        }
        PlayerUI.instance.Count(0);
        _isStart = true;
    }

    void ClearEnemy()
    {

        if (_isEnd) return;

        int i = 0;
        for (i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
                i = 0;
            }
        }

        if (enemys.Count == 0)
        {
            _isEnd = true;
            _velocity = Vector3.zero;
            StageManager.instance.Victory();
            return;
        }
    }

    void Update()
    {
        ClearEnemy();

        if (!_isStart) return;
        if (_isDead) return;
        if (_isEnd) return;

        Move();
        Attack();
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

        _currentSpeed = _playerSave._SPEED;

        index *= _currentSpeed;

        if (_characterController.isGrounded) index.y = 0;
        else index.y -= _gravity * Time.deltaTime;

        _velocity = index;
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
        GameObject effect = Instantiate(_effect, null);
        effect.transform.position = transform.position;

        PlayerUI.instance.UpdateUI();
        if (_playerSave._HP <= 0)
        {

            GameObject dieeffect = Instantiate(_dieEffect, null);
            dieeffect.transform.position = transform.position;

            _animatorType = AnimatorType.DEATH;
            PlayAnimator();
            _isDead = true;
            StageManager.instance.Defeat();
        }
    }


    private void OnGUI()
    {
        //if (_characterController != null)
        //{
        //    var labelStyle = new GUIStyle();
        //    labelStyle.fontSize = 50;
        //    labelStyle.normal.textColor = Color.black;
        //    GUILayout.Label("플레이어 이동 : WASD, 플레이어 공격 : 마우스 좌클릭", labelStyle);

        //    GUILayout.Label("HP : " + _playerSave._HP.ToString(), labelStyle);

        //    GUILayout.Label("MAXHP : " + _playerSave._MAXHP.ToString(), labelStyle);

        //    GUILayout.Label("ATTACK : " + _playerSave._ATTACK.ToString(), labelStyle);

        //    GUILayout.Label("DEFENSE : " + _playerSave._DEFENSE.ToString(), labelStyle);

        //    GUILayout.Label("SPEED : " + _playerSave._SPEED.ToString(), labelStyle);
        //}
    }
}