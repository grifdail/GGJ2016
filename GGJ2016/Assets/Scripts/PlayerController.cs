using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 0;
    public AnimationCurve accelerationResponse = AnimationCurve.Linear(0, 0, 1, 1);
    public float acc = 1;
    public float rotationAcc = 1;


    private Vector3 heading = Vector3.left;
    private Vector3 targetHeading = Vector3.left;
    private float speed = 0;
    private float targetSpeed = 0;



	// Use this for initialization
	void Start () {
	    if (accelerationResponse == null)
        {
            Debug.LogError("GameObject does'nt have an accelerationResponse set");
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 axes = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        targetHeading = axes.normalized;
        targetSpeed = axes.magnitude;
        speed = Mathf.Lerp(speed, targetSpeed, acc * Time.deltaTime);
        float actualSpeed = maxSpeed * accelerationResponse.Evaluate(speed);
        heading = Vector3.RotateTowards(heading, targetHeading, rotationAcc * Time.deltaTime / speed, 0);
        transform.Translate(actualSpeed * heading * Time.deltaTime);
        //Debug.Log(actualSpeed);

        //transform.Translate(axes* Time.deltaTime);
	}
}
