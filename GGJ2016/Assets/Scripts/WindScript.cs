using UnityEngine;
using System.Collections;

public class WindScript : MonoBehaviour {

    public GameObject Player;

    //de 0° à 360°
    public int CurrentDirection;
    private bool _turnOnTheRight = true;

    void Start ()  
    {
        CurrentDirection = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(CurrentDirection, -90, 0);
        StartCoroutine(Wind());  
    }

    void Update ()
    {
        transform.position = Player.transform.position;
    }

    IEnumerator Wind ()
    {
          while (true)
        {
            CurrentDirection += Random.Range(10, 20) * (_turnOnTheRight ? 1 : -1);
            transform.rotation = Quaternion.Euler(CurrentDirection, -90, 0);

            if (Random.Range(0f, 1f) < .1f)
                _turnOnTheRight = !_turnOnTheRight;

            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }    
}
