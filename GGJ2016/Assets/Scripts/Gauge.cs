using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {
                               
    public float _currentValueFaim = 100f;

    public GameObject Player;            

    public GameObject CurseurProgression;
    public GameObject CurseurFaim;

    public bool isFaim = false;

    void Start ()
    {
        if (isFaim)
        StartCoroutine(Faim());
    }

	// Update is called once per frame
	void Update () {
        if (!isFaim)
            CurseurProgression.transform.localPosition = new Vector3 (1.5f + 11 * (Player.transform.position.x/700f), CurseurProgression.transform.localPosition.y, CurseurProgression.transform.localPosition.z);

        if(isFaim)
            CurseurFaim.transform.localPosition = new Vector3(CurseurFaim.transform.localPosition.x, -3.25f + 0.038f * _currentValueFaim, CurseurFaim.transform.localPosition.z);
    }                                                                                        

    IEnumerator Faim()
    {
        while (true)
        { 
            float _timeTillFaim = 2f;

            while (_timeTillFaim > 0f)
            {
                _timeTillFaim -= Time.deltaTime;

                yield return null;
            }

            _currentValueFaim -= 1f;
        }  
    }
}
