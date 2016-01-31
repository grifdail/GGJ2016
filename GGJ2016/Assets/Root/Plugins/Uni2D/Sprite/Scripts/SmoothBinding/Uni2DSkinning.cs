#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
// Skinning component
public class Uni2DSkinning : MonoBehaviour
{
	// quality
	public SkinQuality quality;
	
	[SerializeField]
	[HideInInspector]
	// Bones
	private Transform[] m_oBones;
	
	[SerializeField]
	[HideInInspector]
	// Mesh filter
	private MeshFilter m_rMeshFilter;
	
	[SerializeField]
	[HideInInspector]
	// Mesh filter
	private List<MeshCollider> m_oMeshColliders;
	
	#if AFTER_UNITY_4_3
	[SerializeField]
	[HideInInspector]
	private PolygonCollider2D m_rPolygonCollider2D;
	#endif
	
	[SerializeField]
	[HideInInspector]
	// Pose Render Mesh
	private Mesh m_rPoseRenderMesh;
	
	[SerializeField]
	[HideInInspector]
	// Manipulated render mesh
	private ReferenceContainer m_oRefManipulatedRenderMesh = new ReferenceContainer();
	
	[SerializeField]
	[HideInInspector]
	// Pose Collider Meshes
	private Mesh[] m_oPoseColliderMeshes;
	
	[SerializeField]
	[HideInInspector]
	// Manipulated Collider Meshes
	private ReferenceContainer[] m_oRefManipulatedColliderMeshes;
	
	[SerializeField]
	[HideInInspector]
	// Sprite
	private Uni2DSprite m_rSprite;
	
	// Awake?
	private bool m_bAwakeHasBeenCalled;
	
	// Editable Render Mesh
	public Mesh EditableRenderMesh
	{
		get
		{
			EnsureMeshValidity(m_rSprite.SpriteData.renderMesh, m_rMeshFilter.sharedMesh, ref m_rPoseRenderMesh, ref m_oRefManipulatedRenderMesh);
			return m_oRefManipulatedRenderMesh.Reference as Mesh;
		}
	}
	
	// Bones
	public Transform[] Bones
	{
		set
		{
			m_oBones = value;
		}
		
		get
		{
			return m_oBones;
		}
	}
	
	// MeshColliders
	public List<MeshCollider> MeshColliders
	{
		set
		{
			m_oMeshColliders = value;
		}
		
		get
		{
			return m_oMeshColliders;
		}
	}
	
	#if AFTER_UNITY_4_3
	public PolygonCollider2D PolygonCollider2D
	{
		set
		{
			m_rPolygonCollider2D = value;
		}
		
		get
		{
			return m_rPolygonCollider2D;
		}
	}
	#endif
	
	// Mesh filter
	public MeshFilter MeshFilter
	{
		set
		{
			m_rMeshFilter = value;
		}
		
		get
		{
				return m_rMeshFilter;
		}
	}
	
	// Quality
	public SkinQuality Quality
	{
		set
		{
			quality = value;	
		}
	}
	
	// Add Skinning
	public static Uni2DSkinning AddSkinning(Uni2DSprite a_rSprite)
	{
		Uni2DSkinning rSkinning = a_rSprite.GetComponent<Uni2DSkinning>();
		if(rSkinning == null)
		{
			rSkinning = a_rSprite.gameObject.AddComponent<Uni2DSkinning>();
		}
		rSkinning.m_rSprite = a_rSprite;
		
		return rSkinning;
	}
	
	// Remove Skinning
	public static void RemoveSkinning(Uni2DSprite a_rSprite)
	{
		Uni2DSkinning rSkinning = a_rSprite.Skinning;
		if(rSkinning != null)
		{
			rSkinning.OnRemoveSkinning();
			DestroyImmediate(rSkinning);
		}
	}
	
	// Update All Skinning
	public void UpdateAllSkinning()
	{
		UpdateRenderMeshSkinning();
		UpdatePhysicsSkinning();
	}
	
	// Update Render Mesh Skinning
	public void UpdateRenderMeshSkinning()
	{
		if(m_rMeshFilter != null)
		{
			if(m_oBones != null)
			{				
				if(ApplySkinBlend(m_rSprite.SpriteData.renderMesh, m_rMeshFilter.sharedMesh, ref m_rPoseRenderMesh, ref m_oRefManipulatedRenderMesh))
				{
					m_rMeshFilter.sharedMesh = m_oRefManipulatedRenderMesh.Reference as Mesh;
				}
			}
		}
	}
	
	// Update Physics Skinning
	public void UpdatePhysicsSkinning()
	{
		if(m_oMeshColliders != null)
		{
			if(m_oBones != null)
			{
				int iColliderCount = MeshColliders.Count;
				if(iColliderCount > 0)
				{
					// Create Collider meshes arrays if needed
					if(m_oPoseColliderMeshes == null || m_oPoseColliderMeshes.Length != iColliderCount)
					{
						m_oPoseColliderMeshes = new Mesh[iColliderCount];
					}
					if(m_oRefManipulatedColliderMeshes == null || m_oRefManipulatedColliderMeshes.Length != iColliderCount)
					{
						// Destroy old references
						DestroyManipulatedColliderMeshReferences();
						
						// Recreate
						m_oRefManipulatedColliderMeshes = new ReferenceContainer[iColliderCount];
						for(int i = 0; i<iColliderCount; ++i)
						{
							ReferenceContainer rReferenceContainer = new ReferenceContainer();
							rReferenceContainer.Initialize(this, true);
							m_oRefManipulatedColliderMeshes[i] = rReferenceContainer;
						}
					}
					
					// Apply the skin blend on each collider
					int iColliderIndex = 0;
					List<Mesh> rColliderMeshes = m_rSprite.SpriteData.meshCollidersList;
					foreach(MeshCollider rMeshCollider in m_oMeshColliders)
					{
						if(rMeshCollider != null)
						{
							// Get The pose and manipulated mesh
							Mesh rPoseMesh = m_oPoseColliderMeshes[iColliderIndex];
							ReferenceContainer rRefManipulatedMesh = m_oRefManipulatedColliderMeshes[iColliderIndex];
							
							if(ApplySkinBlend(rColliderMeshes[iColliderIndex], rMeshCollider.sharedMesh, ref rPoseMesh, ref rRefManipulatedMesh))
							{
								// Update collider mesh
								rMeshCollider.sharedMesh = null;
								rMeshCollider.sharedMesh = rRefManipulatedMesh.Reference as Mesh;
								
								m_oPoseColliderMeshes[iColliderIndex] = rPoseMesh;
							}
						}
						
						++iColliderIndex;
					}
				}
			}
		}
		
		#if AFTER_UNITY_4_3
		if(PolygonCollider2D != null)
		{
			PolygonCollider2D.points = Uni2DSkinningUtils.ApplySkinBlend(m_rSprite.SpriteData.mesh2D, m_oBones, transform, quality);
		}
		#endif
	}
	
	// Force Anticipated Awake
	public void ForceAnticipatedAwake()
	{
		Awake();
	}
	
	// Awake
	private void Awake()
	{
		if(m_bAwakeHasBeenCalled == false)
		{
			m_rSprite = GetComponent<Uni2DSprite>();
			m_oRefManipulatedRenderMesh.Initialize(this, true);
			InitializeManipulatedColliderMeshReferences();
			m_bAwakeHasBeenCalled = true;
		}
	}
	
	// On destroy
	private void OnDestroy()
	{		
		m_oRefManipulatedRenderMesh.OnDestroy();
		DestroyManipulatedColliderMeshReferences();
	}
	
	// On Remove
	private void OnRemoveSkinning()
	{
		// Restore original meshes
		
		// Render
		if(m_rMeshFilter != null)
		{
			m_rMeshFilter.sharedMesh = m_rSprite.SpriteData.renderMesh;
		}
		
		// Physics
		if(m_oMeshColliders != null)
		{
			int iColliderIndex = 0;
			List<Mesh> rColliderMeshes = m_rSprite.SpriteData.meshCollidersList;
			foreach(MeshCollider rMeshCollider in m_oMeshColliders)
			{
				if(rMeshCollider != null)
				{
					rMeshCollider.sharedMesh = null;
					rMeshCollider.sharedMesh = rColliderMeshes[iColliderIndex];
				}
				
				++iColliderIndex;
			}
		}
	}
	
	// Initialize
	private void InitializeManipulatedColliderMeshReferences()
	{
		if(m_oRefManipulatedColliderMeshes != null)
		{
			foreach(ReferenceContainer rReferenceContainer in m_oRefManipulatedColliderMeshes)
			{
				rReferenceContainer.Initialize(this, true);
			}
		}
	}
	
	// Destroy
	private void DestroyManipulatedColliderMeshReferences()
	{
		if(m_oRefManipulatedColliderMeshes != null)
		{
			foreach(ReferenceContainer rReferenceContainer in m_oRefManipulatedColliderMeshes)
			{
				rReferenceContainer.OnDestroy();
			}
		}
	}
	
	// ApplySkinBlend
	private bool ApplySkinBlend(Mesh a_rSourceMesh, Mesh a_rUsedMesh, ref Mesh a_rPoseMesh, ref ReferenceContainer a_rRefManipulatedMesh)
	{
		if(EnsureMeshValidity(a_rSourceMesh, a_rUsedMesh, ref a_rPoseMesh, ref a_rRefManipulatedMesh))
		{
			// Renderer
			Vector3[] oVertices = a_rPoseMesh.vertices;
			Uni2DSkinningUtils.ApplySkinBlend(ref oVertices, a_rPoseMesh, m_oBones, transform, quality);
			Mesh rManipulatedMesh = a_rRefManipulatedMesh.Reference as Mesh;
			(rManipulatedMesh).vertices = oVertices;
			rManipulatedMesh.RecalculateBounds();
			
			return true;
		}
		else
		{
			return false;
		}
	}
	
	// Ensure Mesh Validity
	private bool EnsureMeshValidity(Mesh a_rSourceMesh, Mesh a_rUsedMesh, ref Mesh a_rPoseMesh, ref ReferenceContainer a_rRefManipulatedMesh)
	{
		if(a_rSourceMesh != null)
		{
			Object rManipulatedMesh = a_rRefManipulatedMesh.Reference;
			if(a_rPoseMesh == null || a_rPoseMesh != a_rSourceMesh || rManipulatedMesh == null || rManipulatedMesh != a_rUsedMesh)
			{
				a_rPoseMesh = a_rSourceMesh;
				if(rManipulatedMesh != null)
				{
					DestroyImmediate(rManipulatedMesh);
				}
				a_rRefManipulatedMesh.Reference = Uni2DEditorResourceCopyUtils.DuplicateMesh(a_rPoseMesh);
			}
			return true;
		}
		else
		{
			return false;
		}
	}
	
}