using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

    public float weight = 1;
    public float dragSpeed = 1;
    private bool isBeingDragged = false;

    public PlayerController dragTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (isBeingDragged && dragTarget != null)
        {

            if (Input.GetButtonUp("A_1"))
            {
                dragTarget.isDragging = false;
                isBeingDragged = false;
            }
            
            transform.position = Vector3.Lerp(transform.position, dragTarget.GetDragTarget(), dragSpeed * Time.deltaTime);
        }
	}

    public void init(PlayerController target)
    {
        isBeingDragged = true;
        dragTarget = target;
    }
}
