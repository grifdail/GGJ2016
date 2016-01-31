using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
// Bone position
public class Uni2DBonePosition
{
	// local position
	public Vector3 localPosition;
	
	// local rotation
	public Quaternion localRotation;
	
	// local scale
	public Vector3 localScale;
	
	[HideInInspector]
	[SerializeField]
	// Initialized?
	private bool m_bIsInitialized;
	
	// Is Initialized?
	public bool IsInitialized
	{
		get
		{
			return m_bIsInitialized;
		}
	}
	
	// Set Position
	public void SetPosition(Transform a_rTransform)
	{
		a_rTransform.localPosition = localPosition;
		a_rTransform.localRotation = localRotation;
		a_rTransform.localScale = localScale;
	}
	
	// Save Position
	public void SavePosition(Transform a_rTransform)
	{
		localPosition = a_rTransform.localPosition;
		localRotation = a_rTransform.localRotation;
		localScale = a_rTransform.localScale;
		m_bIsInitialized = true;
	}
	
	// Save Position If Needed
	// Return true of a change occured
	public bool SavePositionIfNeeded(Transform a_rTransform)
	{
		bool bChanged = false;
		
		Vector3 f3LocalPosition = a_rTransform.localPosition;
		if(localPosition != f3LocalPosition)
		{
			localPosition = f3LocalPosition;
			bChanged = true;
			m_bIsInitialized = true;
		}
		
		Quaternion oLocalRotation = a_rTransform.localRotation;
		if(localRotation != oLocalRotation)
		{
			localRotation = oLocalRotation;
			bChanged = true;
			m_bIsInitialized = true;
		}
		
		Vector3 f3LocalScale = a_rTransform.localScale;
		if(localScale != f3LocalScale)
		{
			localScale = f3LocalScale;
			bChanged = true;
			m_bIsInitialized = true;
		}
		
		return bChanged;
	}
	
	// Copy From
	public void CopyFrom(Uni2DBonePosition a_rOtherBonePosition)
	{
		localPosition = a_rOtherBonePosition.localPosition;
		localRotation = a_rOtherBonePosition.localRotation;
		localScale = a_rOtherBonePosition.localScale;
		m_bIsInitialized = true;
	}
	
	// Offset position
	public void OffsetPosition(Vector3 a_f3Offset)
	{
		localPosition += a_f3Offset;
	}
}