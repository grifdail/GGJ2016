using UnityEngine;
using System.Collections;
using System;

public class NightScript : MonoBehaviour {

    public float _percentOfDay = 0f;

    void Start ()
    {
        StartCoroutine(Day());
    }

    private IEnumerator Day()
    {
        while (true)
        {
            while (_percentOfDay < 100f)
            {
                _percentOfDay += 0.25f;

                if (_percentOfDay > 69f)
                    GetComponent<SpriteRenderer>().color = new Color (GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a + 1.8f/255f);
                                            
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(5f);

            while (_percentOfDay > 0)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a - 1f/255f);

                yield return null;
            }

            _percentOfDay = 0;
        }
    }
}
