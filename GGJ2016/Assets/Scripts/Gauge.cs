using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {

    public float max = 100;
    public float min = 0;
    public float value = 100;
    public float display = 0;

    public GameObject Player;
    public RectTransform rectTransform;

    // Use this for initialization
    void Start () {
        updateBar();

    }
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).localPosition = new Vector3 (1.5f + 11 * (Player.transform.position.x/700f), transform.GetChild(0).localPosition.y, transform.GetChild(0).localPosition.z);

    }

    public void updateBar()
    {
        //transform.localScale = new Vector3((value- min)/(max- min), 1, 1);
    }
}
