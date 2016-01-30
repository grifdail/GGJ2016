using UnityEngine;
using System.Collections;   

public class AnimalsScript : MonoBehaviour
{
    public float _speed = .05f;
    public float _speedRun = .15f;
    public float _chanceToMoveAfterIdle = .4f;
    public float _chanceToMoveAfterMoving = .7f;
    public float _minDist = 2f;
    public float _maxDist = 4f;  
    public float _minDistRun = 2f;
    public float _maxDistRun = 4f;

    //Distances vision
    //Voit le félin lorsqu'il est immobile ou s'enfuit
    public float _distVision = 15f;
    //Voit le félin lorsqu'il est trop proche de lui (lors d'une fuite), changeant de direction
    public float _distMini = 5f; 

    public GameObject Player;

    public bool _isFleeing = false;
    public bool _isMoving = false;             
                                     
    void Start()
    {
        //Lance l'animation d'Idle  
        //Ne bouge pas tant qu'il joue son anim
        //Lorsqu'il atteint une fin de boucle, il lance la fonction depuis l'animation
        //A la place je fais une Coroutine pour l'instant

        StartCoroutine(MimicIdleAnim()); 
    }

    void Update ()
    {                                     
        if (!_isFleeing && CheckPlayer(_distVision))
            StartFleeing();                
    }
    
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distVision);
    }
                           
    private IEnumerator MimicIdleAnim()
    {      
        float _timeAnimation = 3.5f;
        while (_timeAnimation > 0f)
        {
            _timeAnimation -= Time.deltaTime;
            yield return null;
        }

        StartMoving(_chanceToMoveAfterIdle);
    }

    public void StartMoving(float _chancesToMove)
    {                          
        if (Random.Range(0f, 1f) > _chancesToMove)
        {
            StartCoroutine(Moves());
        }   
        else
        {
            //RestartAnim
            StartCoroutine(MimicIdleAnim());
        }
    }

    private IEnumerator Moves ()
    {                      
        _isMoving = true;
                       
        Vector3 _destination = new Vector3(Random.Range(_minDist, _maxDist) * (Random.value > 0.5f ? -1 : 1) + transform.position.x, Random.Range(_minDist, _maxDist) + (Random.value > 0.5 ? -1 : 1) + transform.position.y, transform.position.z); 

        while (transform.position != _destination)
        {                                           
            this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
            yield return null;
        }                       

        yield return new WaitForSeconds(Random.Range(.1f, .5f));

        _isMoving = false;
        StartMoving(_chanceToMoveAfterMoving);
    }

    private void StartFleeing ()
    {             
        StopAllCoroutines();
        StartCoroutine(Fleeing());   
    }
          
    private IEnumerator Fleeing()
    {
        _isFleeing = true;

        while (_isFleeing)
        {
            //Le temps que l'animal passe à courir avant de check sur le joueur le poursuit toujours
            float _timeBeforeCheckingPlayer = Random.Range(4f, 6f);
            float _timeTillChangeDirection = Random.Range(_minDistRun, _maxDistRun);
            Vector3 _destination = (transform.position - Player.transform.position) * 100f;
                          
            while (_timeBeforeCheckingPlayer > 0f)
            {
                _timeBeforeCheckingPlayer -= Time.deltaTime;

                this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speedRun);

                if (_timeTillChangeDirection > 0f)
                {
                    _timeTillChangeDirection -= Time.deltaTime; 
                    yield return null;
                }
                else
                {
                    _timeTillChangeDirection = Random.Range(_minDistRun, _maxDistRun);

                    float angle = Mathf.Atan2(_destination.y, _destination.x) * Mathf.Rad2Deg;
                    angle += Random.Range(20f, 40f) * (Random.value > 0.5f ? -1 : 1);    
                    _destination = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * _destination.magnitude; 
                }

                if ((transform.position - Player.transform.position).magnitude < _distMini)
                {
                    StartCoroutine(Fleeing());
                    yield break;
                }                              
            }

            if (!CheckPlayer(_distVision))
            {
                _isFleeing = false;
            }
        }

        StartCoroutine(MimicIdleAnim());
    }

    private bool CheckPlayer (float _dist)
    {
        float _distPlayer = (transform.position - Player.transform.position).magnitude;

        if (_distPlayer < _dist)
        {
            return true;
        }
        else
            return false; 
    }
}
