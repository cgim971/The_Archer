using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Movement Property")]
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _attack;
    public float _ATTACK { set => _attack = value; }

    private void Start() => _rigid = GetComponent<Rigidbody>();

    void SetPlayer(Vector3 dir)
    {
        dir.y = 0;
        dir.Normalize();

        _rigid.velocity = dir * _speed;

        GetComponent<AudioSource>().volume = GameManager.Instance._PLAYERSAVE._effectVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("fist"))
        {
            other.GetComponentInParent<WarriorMovement>().TakeDamage(_attack);
            Destroy(this.gameObject);
        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
