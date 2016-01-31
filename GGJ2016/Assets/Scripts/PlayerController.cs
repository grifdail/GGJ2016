using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public float currentSpeed;      
    [HideInInspector]
    public bool isDragging = false;

    [HideInInspector]
    public bool isJumping = false;
    private float jumpProgression = 0;
    public float jumpDuration = 0;
    public float jumpBoost = 0;

    private List<Draggable> draggableInRange;

    private Vector3 _movement = Vector3.zero;

    public bool isCarryingACorpse = false;

    private Vector3 _saveScale;
                             
    void Start ()
    {
        _saveScale = this.transform.localScale;

        if (accelerationResponse == null)
        {
            Debug.LogError("GameObject doesn't have an accelerationResponse set");
            Destroy(this);
        }
        draggableInRange = new List<Draggable>();
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
            GetComponent<Animator>().SetBool("IsJumping", false);
            isJumping = false;
        }
        transform.Translate(_movement * Time.deltaTime * jumpBoost);
    }

    void UpdateRunning()
    {
        if (Input.GetButtonDown("A_1"))
        {
            UpdateContextAction();
        };
        float d2r = Mathf.Deg2Rad;
        Vector3 axes = new Vector3(Input.GetAxis("L_XAxis_1"), -Input.GetAxis("L_YAxis_1"), 0);
        targetSpeed = accelerationResponse.Evaluate(axes.magnitude);

        if (targetSpeed > 0.5f)
        {
            targetHeading = Mathf.Atan2(axes.y, axes.x) * Mathf.Rad2Deg;
        }

        if (Input.GetAxis("TriggersL_1") > 0.5f && !isDragging)
        {
            targetSpeed *= runBoost;   
        }

        speed = Mathf.Lerp(speed, targetSpeed, acc * Time.deltaTime);
        currentSpeed = maxSpeed * speed;
        heading = Mathf.LerpAngle(heading, targetHeading, rotationAcc * Time.deltaTime);

        _movement = currentSpeed * new Vector3(Mathf.Cos(heading * d2r), Mathf.Sin(heading * d2r), 0);
          
        /*RaycastHit2D hit = Physics2D.Raycast(transform.position, _movement, 1f);
        Debug.DrawRay(transform.position, _movement, Color.blue, 1f);
        if (hit.collider != null)
            Debug.Log(hit.collider.name);
        else*/          
        transform.Translate(_movement * Time.deltaTime);
                                   
        GetComponent<Animator>().SetFloat("Blend", currentSpeed/15f);

        if (Input.GetAxis("L_XAxis_1") > 0)
            transform.localScale = new Vector3(-_saveScale.x, _saveScale.y, _saveScale.z);
        else
            transform.localScale = new Vector3(_saveScale.x, _saveScale.y, _saveScale.z);
                      
    }

    void UpdateContextAction ()
    {
        foreach (Draggable stuf in draggableInRange)
        {
            //item.init(this);
            if (stuf != null)
            {
                isDragging = true;
                stuf.init(this);
                return;
            }
        }

        if (Input.GetAxis("TriggersL_1") > 0.5f && !isDragging)
        {
            GetComponent<Animator>().SetBool("IsJumping", true);
            isJumping = true;
            jumpProgression = jumpDuration;
        }
    }

    public Vector3 GetTargetPosition()
    {
        return transform.position + _movement;
    }

    public Vector3 GetDragTarget()
    {
        return transform.position + Vector3.forward * (_movement.x > 0f ? 1 : -1) ;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("draggable")) {
            draggableInRange.Add(collider.GetComponent<Draggable>());
        }
    }
    
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("draggable")) {
            draggableInRange.Remove(collider.GetComponent<Draggable>());
        }
    }         
}
