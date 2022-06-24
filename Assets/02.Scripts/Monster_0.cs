using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_0 : MonoBehaviour
{
    public GameObject _player;

    public NavMeshAgent _nvAgent;
    private void Start()
    {
        _nvAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        _nvAgent.SetDestination(_player.transform.position); 
    }
}
