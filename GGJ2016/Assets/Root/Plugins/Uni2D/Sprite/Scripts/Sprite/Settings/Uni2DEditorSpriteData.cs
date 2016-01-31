// Warning : Uni2D only supported on Unity 3.5.7 and higher
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SpriteScaleMode = Uni2DSprite.SpriteScaleMode;

/*
 * Uni2DEditorSpriteData
 * 
 * Contains the data generated and used by an Uni2DSprite (mesh collider, sprite mesh, material, etc.)
 * 
 * Editor use only.
 * 
 */
[System.Serializable]
public class Uni2DEditorSpriteData
{
	///// Sprite data /////
	// Sprite width computed from sprite scale and texture width
	public float spriteWidth                             = 0.0f;
	// Sprite height computed from sprite scale and texture height
	public float spriteHeight                            = 0.0f;
	// The render mesh used by the game object
	public Mesh renderMesh                               = null;
	// The render mesh original vertex pos.
	public Vector3[ ] renderMeshVertices                 = null;
	// The render mesh normalized UVs
	public Vector2[ ] renderMeshUVs                      = null;
	// The material used by the render mesh
	public Material renderMeshMaterial                   = null;
	// The generated material
	// Used by default if no atlases and no shared material 
	public Material generatedMaterial                   = null;
	// The computed pivot used by the sprite and physic meshes
	public Vector2 pivotCoords                           = Vector2.zero;
	
	// The computed scale used by the sprite and physic meshes
	public SpriteScaleMode scaleMode                     = SpriteScaleMode.Uniform;
	public float scale                                   = Uni2DSpriteUtils.mc_fSpriteUnitToUnity;
	public Vector2 scaleNotUniform                       = Vector2.one * Uni2DSpriteUtils.mc_fSpriteUnitToUnity;

	///// Physic /////
	// Mesh collider triangle count (cache purpose only)
	public int colliderTriangleCount                     = 0;
	// (Compound mode only)
	// The game object parent of mesh collider game objects
	public GameObject meshCollidersRootGameObject        = null;
	// The mesh(es) built from the sprite texture
	public List<Mesh> meshCollidersList                  = new List<Mesh>( );
	// The mesh collider components
	public List<MeshCollider> meshColliderComponentsList = new List<MeshCollider>( );
	//public bool physicIsDirty                            = true;

	#if AFTER_UNITY_4_3
	// 2D
	public PolygonCollider2D polygonCollider2D = null;
	public Uni2DMesh2D mesh2D;
	#endif
	
	public Vector2 Scale
	{
		get
		{
			switch(scaleMode)
			{
				case SpriteScaleMode.NotUniform:
				{
					return scaleNotUniform;	
				}
				
				case SpriteScaleMode.Uniform:
				default:
				{
					return scale * Vector2.one;	
				}
			}
		}
		
		set
		{
			scaleNotUniform = value;
			scale = scaleNotUniform.x;
		}
	}
	
	public Vector2 ScaledPivotCoords
	{
		get
		{
			Vector2 f2Pivot = this.pivotCoords;
			f2Pivot.x *= Scale.x;
			f2Pivot.y *= Scale.y;
			return  f2Pivot;
		}
	}
	
#if UNITY_EDITOR
	// Default constructor
	public Uni2DEditorSpriteData( )
	{
		// Default values used
	}

	// Shallow copy constructor
	public Uni2DEditorSpriteData( Uni2DEditorSpriteData a_rSpriteData )
	{
		this.spriteWidth                 = a_rSpriteData.spriteWidth;
		this.spriteHeight                = a_rSpriteData.spriteHeight;
		this.renderMesh                  = a_rSpriteData.renderMesh;
		this.renderMeshMaterial          = a_rSpriteData.renderMeshMaterial;
		this.generatedMaterial			 = a_rSpriteData.generatedMaterial;
		this.renderMeshVertices          = a_rSpriteData.renderMeshVertices;
		this.renderMeshUVs               = a_rSpriteData.renderMeshUVs;
		this.pivotCoords                 = a_rSpriteData.pivotCoords;
		this.scaleMode                   = a_rSpriteData.scaleMode;
		this.scale                       = a_rSpriteData.scale;
		this.scaleNotUniform             = a_rSpriteData.scaleNotUniform;
		this.colliderTriangleCount       = a_rSpriteData.colliderTriangleCount;
		this.meshCollidersRootGameObject = a_rSpriteData.meshCollidersRootGameObject;
		this.meshCollidersList           = new List<Mesh>( a_rSpriteData.meshCollidersList );
		this.meshColliderComponentsList  = new List<MeshCollider>( a_rSpriteData.meshColliderComponentsList );
		#if AFTER_UNITY_4_3
		this.polygonCollider2D = a_rSpriteData.polygonCollider2D;
		this.mesh2D = a_rSpriteData.mesh2D;
		#endif
		//this.physicIsDirty = a_rSpriteData.physicIsDirty;
	}

	// Returns true if the Uni2DSprite data have been generated
	// for a given physic mode
	public bool AreDataGenerated(Uni2DEditorSpriteSettings a_rSpriteSettings)
	{
		bool bArePhysicsDataGenerated = true;
		if(a_rSpriteSettings.physicsMode != Uni2DSprite.PhysicsMode.NoPhysics)
		{
			if(a_rSpriteSettings.dimensionMode == Uni2DSprite.DimensionMode._2D)
			{
				#if AFTER_UNITY_4_3
				bArePhysicsDataGenerated = polygonCollider2D != null && mesh2D != null;
				#endif
			}
			else
			{
				bArePhysicsDataGenerated = ( this.meshCollidersList                            != null
				                            && this.meshCollidersList.Contains( null )          == false
				                            && this.meshColliderComponentsList                  != null
				                            && this.meshColliderComponentsList.Contains( null ) == false);
			}
		}

		
		return this.renderMesh     != null
			&& this.renderMeshMaterial != null
			&& bArePhysicsDataGenerated;
	}

	// Returns true if and only if a_rObject is not null and if data are equal
	public override bool Equals( System.Object a_rObject )
	{
		return this.Equals( a_rObject as Uni2DEditorSpriteData );
	}

	// Same as above
	public bool Equals( Uni2DEditorSpriteData a_rSpriteData )
	{
		return a_rSpriteData != null
			&& this.colliderTriangleCount       == a_rSpriteData.colliderTriangleCount			// Not sure if relevant...
			&& this.spriteWidth                 == a_rSpriteData.spriteWidth
			&& this.spriteHeight                == a_rSpriteData.spriteHeight
			&& this.renderMesh                  == a_rSpriteData.renderMesh
			&& this.renderMeshVertices.Equals( a_rSpriteData.renderMeshVertices )
			&& this.renderMeshUVs.Equals( a_rSpriteData.renderMeshUVs )
			&& this.renderMeshMaterial          == a_rSpriteData.renderMeshMaterial
			&& this.generatedMaterial           == a_rSpriteData.generatedMaterial
			&& this.pivotCoords                 == a_rSpriteData.pivotCoords
			&& this.scaleMode                   == a_rSpriteData.scaleMode
			&& this.scale                       == a_rSpriteData.scale
			&& this.scaleNotUniform             == a_rSpriteData.scaleNotUniform
			#if AFTER_UNITY_4_3
			&& this.polygonCollider2D == a_rSpriteData.polygonCollider2D
			&& this.mesh2D == a_rSpriteData.mesh2D
			#endif
			&& this.meshCollidersRootGameObject == a_rSpriteData.meshCollidersRootGameObject
			&& this.meshCollidersList.Equals( a_rSpriteData.meshCollidersList )
			&& this.meshColliderComponentsList.Equals( a_rSpriteData.meshColliderComponentsList );
	}

	// Avoids warning
	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	// Has shared resources
	public bool HasSharedResources()
	{
		return EditorUtility.IsPersistent( this.renderMesh )
			//|| EditorUtility.IsPersistent( this.spriteQuadMaterial )
			|| ( this.meshCollidersList != null && Uni2DEditorUtils.IsThereAtLeastOnePersistentObject( this.meshCollidersList ) );
	}
	
	// Clean Resources
	public void CleanResources()
	{
		SafeDestroyImmediate(renderMesh);
		if(meshCollidersList != null)
		{
			foreach(Mesh rColliderMesh in meshCollidersList)
			{
				SafeDestroyImmediate(rColliderMesh);
			}
		}
	}
	
	// Safe destroy immediate
	private static void SafeDestroyImmediate(Object a_rObject)
	{
		if(a_rObject != null && EditorUtility.IsPersistent(a_rObject) == false)
		{
			Object.DestroyImmediate(a_rObject, false);
		}
	}
#endif
}
