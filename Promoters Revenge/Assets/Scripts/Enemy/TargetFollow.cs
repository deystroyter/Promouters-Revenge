using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    private Transform _target;
    public float speed;

    // Start is called before the first frame update
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private IEnumerator FollowWithDelay(float distance)
    {
        if (distance > 5) Follow();
        yield return new WaitForSeconds(1f);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 heading = _target.position - transform.position;
        float distance = heading.magnitude;
        StartCoroutine("FollowWithDelay", distance);
    }

    private void Follow()
    {
        //RE-CODE THIS
        this.transform.LookAt(_target);
        transform.position = Vector3.Slerp(transform.position, transform.position + transform.forward * 0.1f, speed * Time.deltaTime);
    }

    private void Test()
    {
        // Gets a vector that points from the player's position to the _target's.
        //Vector3 heading = _target.position - transform.position;
        //float distance = heading.magnitude;
        //Vector3 direction = heading / distance; // This is now the normalized direction.
    }
}