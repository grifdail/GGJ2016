using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {

    public float max = 100;
    public float min = 0;
    public float value = 100;
    public float display = 0;

    public RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        updateBar();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateBar()
    {
        transform.localScale = new Vector3((value- min)/(max- min), 1, 1);
    }
}
