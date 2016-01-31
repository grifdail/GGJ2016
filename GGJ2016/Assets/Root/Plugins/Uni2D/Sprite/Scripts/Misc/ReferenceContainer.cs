using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif				

[Serializable]
// Reference
public class ReferenceContainer
{
	// The create reference delegate
	public delegate UnityEngine.Object CreateReferenceDelegate();
	
	// The create reference
	private CreateReferenceDelegate m_rCreateReference;
	// Owner
	private UnityEngine.Object m_rOwner;

	// Own the resource?
	private bool m_bOwnResource;
	
	[HideInInspector]
	[SerializeField]
	// Saved manipulated render mesh
	private ReferenceObject m_oRef;
	
	// Saved manipulated render mesh last
	private ReferenceObject m_oRef_Last;
	
	public UnityEngine.Object Reference
	{
		get
		{
			CheckForReferenceValidity();
			return m_oRef.Reference;
		}
		
		set
		{
			CheckForReferenceValidity();
			m_oRef.Reference = value;
		}
	}
	
	// Initialize
	public void Initialize(UnityEngine.Object a_rOwner, bool a_bOwnResource, CreateReferenceDelegate a_rCreateReference = null)
	{
		m_rOwner = a_rOwner;
		m_bOwnResource = a_bOwnResource;
		m_rCreateReference = a_rCreateReference;
	}
	
	// On Destroy
	public void OnDestroy()
	{
		CheckForLostReferences();
		ReferenceObject.SafeDestroy(m_oRef);
	}
	
	// Check fro lost references
	private void CheckForLostReferences()
	{
		if(m_oRef_Last != m_oRef)
		{
			if(m_oRef_Last != null && m_oRef_Last.Owner == m_rOwner)
			{
				UnityEngine.Object.DestroyImmediate(m_oRef_Last);
			}
			m_oRef_Last = m_oRef;
		}
	}
	
	// Check for reference validity
	private bool CheckForReferenceValidity()
	{
		if(m_oRef == null || m_oRef.Owner != m_rOwner)
		{
			m_oRef = ScriptableObject.CreateInstance<ReferenceObject>();
			m_oRef.Create(CreateReference(), m_rOwner, m_bOwnResource);
#if UNITY_EDITOR
			EditorUtility.SetDirty(m_rOwner);
#endif				
			CheckForLostReferences();
			
			return false;
		}
		return true;
	}
	
	// Create Reference
	private UnityEngine.Object CreateReference()
	{
		if(m_rCreateReference == null)
		{
			return null;
		}
		else
		{
			return m_rCreateReference();
		}
	}
}
