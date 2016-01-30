using UnityEngine;
using System.Collections;   

public class AnimalsScript : MonoBehaviour
{      
    public float _speed = .05f;
    public float _chanceToMoveAfterIdle = .4f;
    public float _chanceToMoveAfterMoving = .7f;
    public float _minDist = 2f;
    public float _maxDist = 4f;

    public bool _isFleeing = false;
    public bool _isMoving = false;

    public bool _debugStartFleeing = false;
                                     
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
        //Debug pour activer la fuite
        if (_debugStartFleeing)
        {
            StartCoroutine(Fleeing());
            _debugStartFleeing = false;
        }                    
    }
              
    private IEnumerator MimicIdleAnim()
    {
        print("MimicIdleAnim");
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
        print("StartMoving");
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
        print("Moves");
        _isMoving = true;
                       
        Vector3 _destination = new Vector3(Random.Range(_minDist, _maxDist) * (Random.value > 0.5 ? -1 : 1) + transform.position.x, Random.Range(_minDist, _maxDist) + (Random.value > 0.5 ? -1 : 1) + transform.position.y, transform.position.z); 

        while (transform.position != _destination)
        {                                           
            this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
            yield return null;
        }                       

        yield return new WaitForSeconds(Random.Range(.1f, .5f));

        _isMoving = false;
        StartMoving(_chanceToMoveAfterMoving);
    }
          
    private IEnumerator Fleeing()
    {
        //Le temps que l'animal passe à courir avant de check sur le joueur le poursuit toujours
        //float _timeBeforeCheckingPlayer = 5f;


        Vector3 _destination = new Vector3(Random.Range(_minDist, _maxDist) * (Random.value > 0.5 ? -1 : 1) + transform.position.x, Random.Range(_minDist, _maxDist) + (Random.value > 0.5 ? -1 : 1) + transform.position.y, transform.position.z);

        while (transform.position != _destination)
        {
            //this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
            yield return null;
        }         
    }
}
