using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{      
    private float _speed = 5f;
    private float _timeSinceMoved = 0f;

    private bool Fleeing = false;

    // Use this for initialization
    void Start()
    {                          
        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {              
        while (!Fleeing)
        {

        }

        yield return null;
    }
}
