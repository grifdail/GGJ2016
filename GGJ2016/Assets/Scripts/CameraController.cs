using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float speed = 10;
    private Camera _camera;
    private float defaultSize = 5;
    public float zoomRatio = 1;
    public Vector3 offset = Vector3.zero;
    [HideInInspector]
    public Bounds bound;
    public float cameraBoundSize = 10;
    

    //public Transform playerTransform;
    public PlayerController playerController;

    // Use this for initialization
    void Start () {
        _camera = this.GetComponent<Camera>();
        //defaultSize = _camera.orthographicSize;
        bound = GameObject.Find("SOL").GetComponent<Renderer>().bounds;
        bound.extents = new Vector3(bound.extents.x - cameraBoundSize, bound.extents.y - cameraBoundSize, 1000);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Vector3.Lerp(transform.position - offset, playerController.GetTargetPosition() + Vector3.forward * -10, speed * Time.deltaTime) + offset;

        pos = bound.ClosestPoint(pos);
            transform.position = pos;
        //_camera.orthographicSize = defaultSize + playerController.speed * zoomRatio;
	}
}
