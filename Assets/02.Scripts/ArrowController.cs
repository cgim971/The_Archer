using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Movement Property")]
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _speed = 5f;
    private Transform _player;
    [SerializeField] private float _attack;
    public float _ATTACK { set => _attack = value; }

    private void Start() => _rigid = GetComponent<Rigidbody>();

    void SetPlayer(Transform player)
    {
        _player = player;

        Vector3 dir = transform.position - _player.position;
        dir.y = 0;
        dir.Normalize();

        _rigid.velocity = dir * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("fist"))
        {
            other.GetComponent<WarriorMovement>().TakeDamage(_attack);
            Destroy(this.gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
