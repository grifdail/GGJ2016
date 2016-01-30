using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 0;
    public AnimationCurve accelerationResponse = AnimationCurve.Linear(0, 0, 1, 1);
    public float acc = 1;
    public float rotationAcc = 1;
    public float runBoost = 1;
    public float speedRotationInfluence = 1;

    private float heading = 0;
    private float targetHeading = 0;
    [HideInInspector]
    public float speed = 0;
    private float targetSpeed = 0;

    [HideInInspector]
    public bool isJumping = false;
    private float jumpProgression = 0;
    public float jumpDuration = 0;
    public float jumpBoost = 0;

    private Vector3 _movement = Vector3.zero;

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
        if (isJumping)
        {
            UpdateJumping();
        } else
        {
            UpdateRunning();
        }
        
    }

    void UpdateJumping()
    {
        jumpProgression -= Time.deltaTime;
        if (jumpProgression<0)
        {
            isJumping = false;
        }
        transform.Translate(_movement * Time.deltaTime * jumpBoost);
    }

    void UpdateRunning()
    {
        float d2r = Mathf.Deg2Rad;
        Vector3 axes = new Vector3(Input.GetAxis("L_XAxis_1"), -Input.GetAxis("L_YAxis_1"), 0);
        targetHeading = Mathf.Atan2(axes.y, axes.x) * Mathf.Rad2Deg;
        targetSpeed = accelerationResponse.Evaluate(axes.magnitude);
        if (Input.GetAxis("TriggersL_1") > 0.5)
        {
            targetSpeed *= runBoost;
        }
        speed = Mathf.Lerp(speed, targetSpeed, acc * Time.deltaTime);
        float actualSpeed = maxSpeed * speed;
        heading = Mathf.LerpAngle(heading, targetHeading, rotationAcc * Time.deltaTime);
        _movement = actualSpeed * new Vector3(Mathf.Cos(heading * d2r), Mathf.Sin(heading * d2r), 0);
        transform.Translate(_movement * Time.deltaTime);
        if (Input.GetButtonDown("A_1"))
        {
            isJumping = true;
            jumpProgression = jumpDuration;
        };
    }

    public Vector3 GetTargetPosition()
    {
        return transform.position + _movement;
    }


}
