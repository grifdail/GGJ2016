using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float speed = 10;
    private Camera _camera;
    private float defaultSize = 5;
    public float zoomRatio = 1;

    //public Transform playerTransform;
    public PlayerController playerController;

    // Use this for initialization
    void Start () {
        _camera = this.GetComponent<Camera>();
        defaultSize = _camera.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, playerController.GetTargetPosition() + Vector3.forward * -10, speed * Time.deltaTime);
        _camera.orthographicSize = defaultSize + playerController.speed * zoomRatio;
	}
}
