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
    public float _minTimeRun = 2f;
    public float _maxTimeRun = 4f;
    public float _minAngle = 20f;
    public float _maxAngle = 40f;

    //Distances vision
    //Voit le félin lorsqu'il est immobile ou s'enfuit
    public float _distVision = 15f;
    //Voit le félin lorsqu'il est trop proche de lui (lors d'une fuite), changeant de direction
    public float _distMini = 5f;

    public GameObject Player;
    public GameObject Wind;

    private bool _isFleeing = false;         

    private Vector3 _posStart;
    public float _radius = 15f;
    private float _debugRadiusDetection;

    void Start()
    {                                               
        //Lance l'animation d'Idle  
        //Ne bouge pas tant qu'il joue son anim
        //Lorsqu'il atteint une fin de boucle, il lance la fonction depuis l'animation
        //A la place je fais une Coroutine pour l'instant

        StartCoroutine(MimicIdleAnim());

        _posStart = this.transform.position;
    }

    void Update ()
    {                              
        if ((transform.position - Player.transform.position).magnitude < _distMini *3f)
        {
            if (!_isFleeing && CheckPlayer(_distVision))
                StartFleeing();  
        }
    }         
   
    void OnTriggerEnter2D (Collider2D coll)
    {                                 
        if (coll.gameObject.tag == "Player")
        {                
            Player.GetComponent<PlayerController>().isCarryingACorpse = true;

            if (GetComponentInParent<HoardScript>() != null)
            {
                GetComponentInParent<HoardScript>()._hoard.Remove(this.gameObject);
                GetComponentInParent<HoardScript>().StartCoroutine(GetComponentInParent<HoardScript>().DestroyHoard());
            }
            Destroy(this.gameObject);
        }
    }      
                
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, _distVision);    
        // Gizmos.DrawWireSphere(_posStart, _radius);

        Gizmos.DrawWireSphere(transform.position, _debugRadiusDetection);
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
        if (Random.Range(0f, 1f) < _chancesToMove)
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
        Vector3 _destination = new Vector3(Random.Range(_minDist, _maxDist) * (Random.value > 0.5f ? -1 : 1) + transform.position.x, Random.Range(_minDist, _maxDist) + (Random.value > 0.5 ? -1 : 1) + transform.position.y, transform.position.z);
                                                                                   
        if ((_destination - _posStart).magnitude > _radius)
        {                                                      
            _destination = new Vector3(_posStart.x - transform.position.x /4f, _posStart.y - transform.position.y / 4f, transform.position.z);
        }

        while (transform.position != _destination && !Physics2D.Raycast(transform.position, _destination - transform.position, _speed*2))
        {                       
            this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speed);
            yield return null;
        }                       

        yield return new WaitForSeconds(Random.Range(.1f, .5f));
                             
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
            float _timeTillChangeDirection = Random.Range(_minTimeRun, _maxTimeRun);
            Vector3 _destination = (transform.position - Player.transform.position) * 100f;
                          
            while (_timeBeforeCheckingPlayer > 0f)
            {
                _timeBeforeCheckingPlayer -= Time.deltaTime;
                               
                if (!Physics2D.Raycast(transform.position, _destination - transform.position, _speedRun * 2f))
                {                              
                    this.transform.position = Vector3.MoveTowards(transform.position, _destination, _speedRun);
                }                       

                if (_timeTillChangeDirection > 0f)
                {
                    _timeTillChangeDirection -= Time.deltaTime; 
                    yield return null;
                }
                else
                {
                    _timeTillChangeDirection = Random.Range(_minTimeRun, _maxTimeRun);

                    float angle = Mathf.Atan2(_destination.y, _destination.x) * Mathf.Rad2Deg;
                    angle += Random.Range(_minAngle, _maxAngle) * (Random.value > 0.5f ? -1 : 1);    
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
        //Plusieurs paramètres : 
        // - la distance du joueur
        // - s'il fait du bruit et la vision (donc sa vitesse, les herbes et les brindilles) Pour simplifier on considère que l'herbe réduit le bruit, pas qu'elle cache le joueur
        // - le sens du vent       
        float _distPlayer = (transform.position - Player.transform.position).magnitude;

        // On ajoute jusqu'à 100% de la distance de vision si le joueur fait beaucoup de bruit
        float percentNoise = Player.GetComponent<PlayerController>().currentSpeed /15f;
                                
        //On ajoute jusqu'à 100% de la distance de vision si le joueur est contre le vent
        float percentOdor = 0f; 
        float anglePerso = Mathf.Atan2(Player.transform.position.y - transform.position.y, Player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        anglePerso = anglePerso + ((anglePerso < 0) ? 360 : 0);

         if (anglePerso % 360 < (Wind.GetComponent<WindScript>().CurrentDirection) + 20f && anglePerso % 360 > (Wind.GetComponent<WindScript>().CurrentDirection) - 20f)
            percentOdor = 100f;
                             
        _debugRadiusDetection = _dist + _dist * (percentNoise + percentOdor);

        if (_distPlayer < _dist + _dist * (percentNoise + percentOdor))
        {
            return true;
        }
        else
            return false; 
    }
}
