using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorFunch : MonoBehaviour
{
    float _damage = 1;
    bool _isFunch = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isFunch) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<PlayerMovement>().TakeDamage(_damage);
        }
    }

    public void SetIsFunch()
    {
        _isFunch = !_isFunch;
    }
}
