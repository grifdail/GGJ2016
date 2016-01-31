using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif				

// Reference
public class ReferenceObject : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	private bool m_bOwnResource;
	
	[HideInInspector]
	[SerializeField]
	private Object m_rReference;
	
	[HideInInspector]
	[SerializeField]
	// Owner
	private Object m_rOwner; 
	
	// Set Mesh
	public Object Reference
	{
		get
		{
			return m_rReference;
		}
		
		set
		{
			m_rReference = value;
		}
	}
	
	// Set Owner
	public Object Owner
	{
		get
		{
			return m_rOwner;
		}
	}
	
	// Create
	public void Create(Object a_rReference, Object a_rOwner, bool a_bOwnResource)
	{
		m_rReference = a_rReference;
		m_rOwner = a_rOwner;
		m_bOwnResource = a_bOwnResource;
	}
	
	// Destroy
	public static void SafeDestroy(ReferenceObject a_rRef)
	{
		if(a_rRef != null)
		{
			Object.DestroyImmediate(a_rRef);
		}
	}
	
	// On Destroy
	private void OnDestroy()
	{
		if(m_bOwnResource)
		{
			if(m_rReference != null)
			{
				#if UNITY_EDITOR
				if(EditorUtility.IsPersistent(m_rReference) == false)
				#endif	
				{
					DestroyImmediate(m_rReference);	
				}
			}
		}
	}
}
