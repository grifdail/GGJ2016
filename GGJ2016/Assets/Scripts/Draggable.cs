using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

    //public float weight = 1;
    public float dragSpeed = 2.5f;
    private bool isBeingDragged = false;

    public PlayerController dragTarget;
                     
	void Update () {
	    if (isBeingDragged && dragTarget != null)
        {       
            /*if (Input.GetButtonUp("A_1"))
            {
                dragTarget.isDragging = false;
                isBeingDragged = false;
            }   */
            
            transform.position = Vector3.Lerp(transform.position, dragTarget.GetDragTarget(), dragSpeed * Time.deltaTime);
        }
	}

    public void init(PlayerController target)
    {
        isBeingDragged = true;
        dragTarget = target;
    }

    void OnTriggerEnter2D (Collider2D other)
    {                   
        if (other.tag == "Cachette" && this.name == "Baby")
        {
            dragTarget.isDragging = false;
            isBeingDragged = false;
        }
    }
}
