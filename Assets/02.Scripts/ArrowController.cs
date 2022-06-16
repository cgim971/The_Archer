using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Movement Property")]
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _speed = 5f;
    private Transform _player;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    void SetPlayer(Transform player)
    {
        _player = player;

        Vector3 dir = transform.position - _player.position;
        dir.y = 0;
        dir.Normalize();

        _rigid.velocity = dir * _speed;
    }
}
