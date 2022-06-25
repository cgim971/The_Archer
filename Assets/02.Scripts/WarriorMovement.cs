using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    [Header("View Property")]
    [SerializeField] bool DebugMode = false;
    [Range(0f, 360f)][SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstacleMask;

    [Header("Movement Property")]
    [SerializeField] private CharacterController _characterController;
    [Tooltip("Player Velocity")] private Vector3 _velocity;
    [SerializeField] private float _currentSpeed = 0f;
    [SerializeField] private float _targetChaseSpeed = 5f;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _rotateDamp = 5f;
    private float _gravity = 10f;
    Transform _cameraTransform;
    bool _isMoving = false;
    GameObject _target = null;
    Vector3 pos = Vector3.zero;
    bool _isDead = false;

    [Header("Animator Property")]
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AnimatorType _animatorType = AnimatorType.NONE;
    [SerializeField] private AnimationClip _leftFunchClip;
    [SerializeField] private AnimationClip _rightFunchClip;


    public enum AnimatorType { NONE, IDLE, RUN, FUNCH, HIT, DEATH }

    [Header("Attack Property")]
    [SerializeField] private float _funchRange = 1f;
    [SerializeField] private int _funchIndex = 0;
    [SerializeField] private Collider[] _funchCol;
    float _leftFunchTime = 0;
    float _leftFunchCurrentTime = 0;
    float _rightFunchTime = 0;
    float _rightFunchCurrentTime = 0;

    [Header("Warrior Property")]
    [SerializeField] private float _hp;
    [SerializeField] private float _attack;
    public float _HP
    {
        get => _hp;
        set
        {
            _hp = value;

            if (_hp <= 0)
            {
                _animatorType = AnimatorType.DEATH;
                PlayAnimator();

                StageManager.instance.Victory();
            }
        }
    }
    public float _maxHp = 5;

    bool _isStart = false;
    void StartTime()
    {
        _isStart = true;
        StartCoroutine(RandomIndex());
    }

    void Start()
    {
        _cameraTransform = Camera.main.transform;

        _velocity = Vector3.zero;
        _characterController = transform.GetComponentInParent<CharacterController>();
        _leftFunchTime = _leftFunchClip.length;
        _rightFunchTime = _rightFunchClip.length;

        _HP = _maxHp;

        _target = FindObjectOfType<PlayerMovement>().transform.parent.gameObject;
        PlayerUI.instance.EnemyUI(_maxHp, _HP);

        Invoke("StartTime", 3);
    }

    private void Update()
    {
        if (!_isStart) return;

        if (_isDead) return;

        FindingTarget();
        Move();
        PlayAnimator();

        _characterController.Move(_velocity * Time.deltaTime);
    }

    IEnumerator RandomIndex()
    {
        while (true)
        {
            if (_isMoving)
            {
                yield return new WaitForSeconds(3f);
                _isMoving = false;
            }

            yield return new WaitUntil(() => !_isMoving);

            FindingTarget();

            if (_target == null)
            {
                pos.x = (Random.Range(0f, 2f) - 1);
                pos.z = (Random.Range(0f, 2f) - 1);

            }
        }
    }

    bool _isCheck = false;
    void CheckWall2() => _isCheck = false;
    // 수정 필요
    void CheckWall()
    {
        if (_isCheck) return;
        Ray ray1 = new Ray(transform.position + new Vector3(0.5f, 0f, 0f), transform.forward + new Vector3(0.5f, 0f, 0f));
        Ray ray2 = new Ray(transform.position - new Vector3(0.5f, 0f, 0f), transform.forward - new Vector3(0.5f, 0f, 0f));

        if (Physics.Raycast(ray1, 1.1f, _obstacleMask) || Physics.Raycast(ray2, 1.1f, _obstacleMask))
        {
            _isCheck = true;
            pos.x *= -1;
            pos.z *= -1;

            Invoke("CheckWall2", 1);
        }
    }

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
            this._target = target[0].gameObject;
            if (target != null)
            {
                if (index.magnitude <= _funchRange)
                {
                    transform.LookAt(this._target.transform);
                    Vector3 a = new Vector3(0, transform.eulerAngles.y, 0);
                    transform.localEulerAngles = a;
                    pos = Vector3.zero;
                    _animatorType = AnimatorType.FUNCH;
                    PlayAnimator();
                }
                else pos = targetDir;
            }
            return;
        }

        this._target = null;
        return;
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    void Move()
    {
        CheckWall();

        if (_leftFunchCurrentTime > 0)
        {
            _leftFunchCurrentTime -= Time.deltaTime;
            _velocity = Vector3.zero;
        }
        else if (_rightFunchCurrentTime > 0)
        {
            _rightFunchCurrentTime -= Time.deltaTime;
            _velocity = Vector3.zero;
        }

        if (_leftFunchCurrentTime > 0) return;
        else if (_rightFunchCurrentTime > 0) return;

        Vector3 forward = _cameraTransform.forward;
        forward.y = 0;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float h = pos.x;
        float v = pos.z;

        Vector3 index = forward * v + right * h;
        index.Normalize();

        if (index == Vector3.zero) _animatorType = AnimatorType.IDLE;
        else _animatorType = AnimatorType.RUN;

        if (index != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(index), _rotateDamp * Time.deltaTime);

        if (_target == null) _currentSpeed = _walkSpeed;
        else _currentSpeed = _targetChaseSpeed;

        index *= _currentSpeed;
        if (_characterController.isGrounded) index.y = 0;
        else index.y -= _gravity * Time.deltaTime;

        _velocity = index;

        _isMoving = true;
    }

    void Funch()
    {
        if (_leftFunchCurrentTime > 0)
        {
            if (_leftFunchCurrentTime - 0.1f <= 0f)
            {
                _funchIndex++;
            }
            else
            {
                _funchIndex = 0;
                return;
            }
        }
        if (_rightFunchCurrentTime > 0) return;

        if (_funchIndex == 0) _leftFunchCurrentTime = _leftFunchTime;
        else if (_funchIndex >= 1) _rightFunchCurrentTime = _rightFunchTime;

        _animator.SetFloat("FunchIndex", _funchIndex);
        _animator.SetTrigger("Funch");

        if (_funchIndex >= 1) _funchIndex = 0;
    }

    void PlayAnimator()
    {
        switch (_animatorType)
        {
            case AnimatorType.IDLE:
                _animator.SetBool("isRun", false);
                break;
            case AnimatorType.RUN:
                _animator.SetBool("isRun", true);
                break;
            case AnimatorType.FUNCH:
                Funch();
                break;
            case AnimatorType.HIT:
                _animator.SetTrigger("GetHit");
                break;
            case AnimatorType.DEATH:
                _animator.SetTrigger("Death");
                StopCoroutine(RandomIndex());
                Invoke("Dead", 3f);
                break;
        }

    }

    public void TakeDamage(float damage)
    {
        if (_HP <= 0) return;

        _HP -= damage;
        PlayerUI.instance.EnemyUI(_maxHp, _HP);
        _animatorType = AnimatorType.HIT;
        PlayAnimator();

        if (_HP <= 0)
        {
            _animatorType = AnimatorType.DEATH;
            PlayAnimator();
            _isDead = true;
        }
    }

    public void SetIsFunch(int index) => _funchCol[index].GetComponent<WarriorFunch>().SetIsFunch();

    void Dead() => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        if (!DebugMode) return;
        Vector3 myPos = transform.position + Vector3.up;
        Gizmos.DrawWireSphere(myPos, ViewRadius);

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);
        lookDir.y = 0;

        Debug.DrawRay(myPos, rightDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * ViewRadius, Color.cyan);

        Collider[] target = Physics.OverlapSphere(myPos, ViewRadius, _targetMask);

        if (target.Length == 0) return;
        Vector3 targetPos = target[0].transform.position;
        Vector3 index = (targetPos - myPos);
        index.y = 0;
        Vector3 targetDir = index.normalized;
        float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
        if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, _obstacleMask))
        {
            if (DebugMode) Debug.DrawLine(myPos, targetPos, Color.red);
        }
    }
}
