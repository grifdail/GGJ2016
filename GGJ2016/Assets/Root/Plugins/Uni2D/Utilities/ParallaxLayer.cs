using UnityEngine;
using System.Collections;

[AddComponentMenu("Uni2D/Utilities/ParallaxLayer")]
public class ParallaxLayer : MonoBehaviour 
{
	public enum EParallaxMode
	{
		Uniform,
		ByAxis,
	}
	
	public enum EMasterCameraSelectionMode
	{
		MainCamera,
		ByReference,
		ByName,
		ByTag,
	}
	
	[System.Serializable]
	public class FreezeAxes
	{
		public bool x;
		public bool y;
	}
	
	[System.Serializable]
	public class ParallaxByAxis
	{
		public float x = 0.5f;
		public float y = 0.5f;
	}
	
	// Parallax
	
	public EParallaxMode mode;
	
	// Uniform mode
	public float parallax = 0.5f;
	public FreezeAxes freezeAxes;
	
	// By Axis mode
	public ParallaxByAxis parallaxByAxis;
	
	// Selected camera
	
	public EMasterCameraSelectionMode masterCamera;
	
	// By reference
	public Camera masterCameraReference;
	
	// By name
	public string masterCameraName;
	
	// By tag
	public string masterCameraTag;
	
	private Vector3 m_f3CameraLastPosition;
	
	private void Start()
	{
		SelectCamera();
		m_f3CameraLastPosition = GetMasterCameraPosition();
	}
	
	private void LateUpdate()
	{
		UpdateParallax();
	}
	
	private void UpdateParallax()
	{
		if(masterCameraReference == null)
		{
			return;
		}
		
		Vector3 f3CameraPosition = GetMasterCameraPosition();
		
		Vector3 f3CameraMovement = f3CameraPosition - m_f3CameraLastPosition;
		
		// Apply parallax
		f3CameraMovement.z = 0.0f;
		switch(mode)
		{
			case ParallaxLayer.EParallaxMode.Uniform:
			{
				f3CameraMovement *= parallax;
				
				if(freezeAxes.x)
				{
					f3CameraMovement.x = 0.0f;
				}
				
				if(freezeAxes.y)
				{
					f3CameraMovement.y = 0.0f;
				}
			}
			break;
				
			case ParallaxLayer.EParallaxMode.ByAxis:
			{
				f3CameraMovement.x *= parallaxByAxis.x;
				f3CameraMovement.y *= parallaxByAxis.y;
			}
			break;
		}
		
		transform.position += f3CameraMovement;
		
		m_f3CameraLastPosition = f3CameraPosition;
	}
	
	private Vector3 GetMasterCameraPosition()
	{
		if(masterCameraReference == null)
		{
			return Vector3.zero;
		}
		
		return masterCameraReference.transform.position;
	}
	
	private void SelectCamera()
	{
		// Select camera
		Camera rSelectedCamera = null;
		switch(masterCamera)
		{
			case EMasterCameraSelectionMode.MainCamera:
			{
				rSelectedCamera = Camera.main;
			}
			break;
			
			case EMasterCameraSelectionMode.ByReference:
			{
				rSelectedCamera = masterCameraReference;
			}
			break;
			
			case EMasterCameraSelectionMode.ByName:
			{
				GameObject rMasterCameraGameObject = GameObject.Find(masterCameraName);
				if(rMasterCameraGameObject == null)
				{
					Debug.LogError("Can't find the master camera named : " + masterCameraName);
				}
				else if(rMasterCameraGameObject.GetComponent<Camera>() == null)
				{
					Debug.LogError(rMasterCameraGameObject + " doesn't have a camera, and thus can't be selected as a master camera");
				}
				else
				{
					rSelectedCamera = rMasterCameraGameObject.GetComponent<Camera>();
				}
			}
			break;
			
			case EMasterCameraSelectionMode.ByTag:
			{
				GameObject rMasterCameraGameObject = GameObject.FindGameObjectWithTag(masterCameraTag);
				if(rMasterCameraGameObject == null)
				{
					Debug.LogError("Can't find the master camera with the tag : " + masterCameraName);
				}
				else if(rMasterCameraGameObject.GetComponent<Camera>() == null)
				{
					Debug.LogError(rMasterCameraGameObject + " doesn't have a camera, and thus can't be selected as a master camera");
				}
				else
				{
					rSelectedCamera = rMasterCameraGameObject.GetComponent<Camera>();
				}
			}
			break;
		}
		
		// Once a camera has been selected update the public field to reflect the selection
		if(rSelectedCamera == null)
		{
			masterCameraReference = null;
			masterCameraName = "";
			masterCameraTag = "";
		}
		else
		{
			masterCameraReference = rSelectedCamera;
			masterCameraName = rSelectedCamera.name;
			masterCameraTag = rSelectedCamera.tag;
		}
	}
}
