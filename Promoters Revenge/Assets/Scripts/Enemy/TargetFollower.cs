using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TargetFollower : MonoBehaviour
{
    private GameObject _target;
    private NavMeshAgent _navMeshAgent;
    private GameObject _player;

    [SerializeField] private float _followingDelayTime = 0.1f; //Time to react to changing target position

    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _target = _player;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FollowingWithDelay());
    }

    public void PauseFollow()
    {
        _navMeshAgent.isStopped = true;
    }

    public void ContinueFollow()
    {
        _navMeshAgent.isStopped = false;
    }


    private IEnumerator FollowingWithDelay()
    {
        do
        {
            try
            {
                Follow();
            }
            catch (MissingReferenceException e)
            {
                _target = _player;
                Follow();
            }

            yield return new WaitForSeconds(_followingDelayTime);
        } while (true);
    }

    private void Follow()
    {
        _navMeshAgent.SetDestination(_target.transform.position);
        Console.WriteLine(_target.transform.position);
    }

    public void FollowForSeconds(GameObject target, float seconds)
    {
        StartCoroutine(ChangeTargetWithDelay(_target, seconds));
        _target = target;
    }

    private IEnumerator ChangeTargetWithDelay(GameObject newTarget, float changeTargetDelayTime)
    {
        yield return new WaitForSeconds(changeTargetDelayTime);
        _target = newTarget;
        yield break;
    }
}