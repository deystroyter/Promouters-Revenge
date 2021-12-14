using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelExitScript : MonoBehaviour
{
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.ToLower() == "player")
        {
            GameManager.Instance.ExitLevel();
        }
    }
}