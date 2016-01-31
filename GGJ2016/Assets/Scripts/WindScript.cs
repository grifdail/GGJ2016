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
        transform.rotation = Quaternion.Euler(0f, 0f, CurrentDirection);
        StartCoroutine(Wind());  
        StartCoroutine(WindSprite());
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
            transform.rotation = Quaternion.Euler(0f, 0f, CurrentDirection);

            if (Random.Range(0f, 1f) < .1f)
                _turnOnTheRight = !_turnOnTheRight;

            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    IEnumerator WindSprite()
    {                     //Il est 7h30, je fais du code sale si je veux :P
        Animator _anim = GetComponentInChildren<Animator>();

        while (true)
        {
            _anim.SetBool("Activate", true);

            yield return new WaitForSeconds(.2f);

            _anim.SetBool("Activate", false);

            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }
}
