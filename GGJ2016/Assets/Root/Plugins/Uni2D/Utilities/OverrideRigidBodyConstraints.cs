using UnityEngine;
using System.Collections;

[AddComponentMenu("Uni2D/Utilities/OverrideRigidBodyConstraints")]
[ExecuteInEditMode()]
public class OverrideRigidBodyConstraints : MonoBehaviour 
{
	[System.Serializable]
	public class FreezeAxes
	{
		public bool x;
		public bool y;
		public bool z;
	}
	
	public FreezeAxes freezePosition = new FreezeAxes();
	public FreezeAxes freezeRotation = new FreezeAxes();
	
	private void Update() 
	{
		if(GetComponent<Rigidbody>() != null)
		{
			RigidbodyConstraints eRigidBodyConstraint = RigidbodyConstraints.None;
			
			// Position
			if(freezePosition.x)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezePositionX;
			}
			
			if(freezePosition.y)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezePositionY;
			}
			
			if(freezePosition.z)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezePositionZ;
			}
			
			// Rotation
			if(freezeRotation.x)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezeRotationX;
			}
			
			if(freezeRotation.y)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezeRotationY;
			}
			
			if(freezeRotation.z)
			{
				eRigidBodyConstraint |= RigidbodyConstraints.FreezeRotationZ;
			}
			
			GetComponent<Rigidbody>().constraints = eRigidBodyConstraint;
		}
	}
}
