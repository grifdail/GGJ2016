#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

using PivotType = Uni2DSprite.PivotType;
using SpriteScaleMode = Uni2DSprite.SpriteScaleMode;

public class Uni2DEditorSerializedSprite_InteractiveBuildSettings
{
	private SerializedObject m_rSerializedSprite = null;

	// Texture
	public SerializedSetting<Texture2D> serializedTexture = null;
	public SerializedSetting<string> serializedTextureGUID = null;
	public SerializedSetting<Uni2DTextureAtlas> serializedAtlas = null;
	public SerializedSetting<Material> serializedSharedMaterial = null;
	
	// Sprite
	public SerializedSetting<SpriteScaleMode> serializedSpriteScaleMode         = null;
	public SerializedSetting<float> serializedSpriteScale         = null;
	public SerializedSetting<Vector2> serializedSpriteScaleNotUniform         = null;
	public SerializedSetting<PivotType> serializedPivotType       = null;
	public SerializedSetting<Vector2> serializedPivotCustomCoords = null;
	public SerializedSetting<Color> serializedVertexColor         = null;

	// Physic
	public SerializedSetting<float> serializedExtrusionDepth = null;
	public SerializedSetting<bool> serializedIsKinematic     = null;
	public SerializedSetting<bool> serializedIsTrigger     = null;

	// Skeletal animation
	public SerializedSetting<float> serializedBoneInfluenceFalloff = null;
	public SerializedSetting<SkinQuality> serializedSkinQuality    = null;
	public SerializedSetting<Uni2DSprite.EPhysicsSkinMode> serializedPhysicsSkinMode    = null;
	public SerializedSetting<Uni2DSprite.ERenderSkinMode> serializedRenderSkinMode    = null;

	public SerializedObject SerializedSprite
	{
		get
		{
			return m_rSerializedSprite;
		}
	}

	public Uni2DEditorSerializedSprite_InteractiveBuildSettings( SerializedObject a_rSerializedSprite )
	{
		if( a_rSerializedSprite != null )
		{
			m_rSerializedSprite = a_rSerializedSprite;

			serializedTexture     	 = new SerializedSetting<Texture2D>( a_rSerializedSprite, "m_rSpriteSettings.textureContainer.m_rTexture" );
			serializedTextureGUID 	 = new SerializedSetting<string>( a_rSerializedSprite, "m_rSpriteSettings.textureContainer.m_oTextureGUID" );
			serializedAtlas       	 = new SerializedSetting<Uni2DTextureAtlas>( a_rSerializedSprite, "m_rSpriteSettings.atlas" );
			serializedSharedMaterial = new SerializedSetting<Material>(a_rSerializedSprite, "m_rSpriteSettings.sharedMaterial");
			
			serializedSpriteScaleMode = new SerializedSetting<SpriteScaleMode>( a_rSerializedSprite, "m_rSpriteSettings.spriteScaleMode"   );
			serializedSpriteScale = new SerializedSetting<float>( a_rSerializedSprite, "m_rSpriteSettings.spriteScale"   );
			serializedSpriteScaleNotUniform = new SerializedSetting<Vector2>( a_rSerializedSprite, "m_rSpriteSettings.spriteScaleNotUniform"   );
			
			serializedPivotType         = new SerializedSetting<PivotType>( a_rSerializedSprite, "m_rSpriteSettings.pivotType" );
			serializedPivotCustomCoords = new SerializedSetting<Vector2>( a_rSerializedSprite, "m_rSpriteSettings.pivotCustomCoords" );

			serializedVertexColor = new SerializedSetting<Color>( a_rSerializedSprite, "m_rSpriteSettings.vertexColor" );

			serializedExtrusionDepth = new SerializedSetting<float>( a_rSerializedSprite, "m_rSpriteSettings.extrusionDepth" );
			serializedIsKinematic    = new SerializedSetting<bool>( a_rSerializedSprite, "m_rSpriteSettings.isKinematic" );
			serializedIsTrigger    = new SerializedSetting<bool>( a_rSerializedSprite, "m_rSpriteSettings.isTrigger" );

			serializedBoneInfluenceFalloff = new SerializedSetting<float>( a_rSerializedSprite, "m_rSpriteSettings.boneInfluenceFalloff" );
			serializedSkinQuality = new SerializedSetting<UnityEngine.SkinQuality>( a_rSerializedSprite, "m_rSpriteSettings.skinQuality" );
			serializedPhysicsSkinMode = new SerializedSetting<Uni2DSprite.EPhysicsSkinMode>( a_rSerializedSprite, "m_rSpriteSettings.physicsSkinMode" );
			serializedRenderSkinMode = new SerializedSetting<Uni2DSprite.ERenderSkinMode>( a_rSerializedSprite, "m_rSpriteSettings.renderSkinMode" );
		}
	}

	public void ApplyValues( )
	{
		serializedTexture.Apply( );
		serializedTextureGUID.Apply( );
		serializedAtlas.Apply( );
		serializedSharedMaterial.Apply( );

		serializedSpriteScaleMode.Apply( );
		serializedSpriteScale.Apply( );
		serializedSpriteScaleNotUniform.Apply( );
		
		serializedVertexColor.Apply( );
		serializedExtrusionDepth.Apply( );
		serializedIsKinematic.Apply( );
		serializedIsTrigger.Apply();

		serializedPivotType.Apply( );
		serializedPivotCustomCoords.Apply( );

		serializedBoneInfluenceFalloff.Apply( );
		serializedSkinQuality.Apply( );
		serializedPhysicsSkinMode.Apply( );
		serializedRenderSkinMode.Apply( );
	}

	public bool AreValuesModified( )
	{
		return serializedTexture.IsDefined
			|| serializedTextureGUID.IsDefined
			|| serializedPivotCustomCoords.IsDefined
			|| serializedPivotType.IsDefined
			|| serializedVertexColor.IsDefined
			|| serializedSpriteScaleMode.IsDefined
			|| serializedSpriteScale.IsDefined
			|| serializedSpriteScaleNotUniform.IsDefined
			|| serializedExtrusionDepth.IsDefined
			|| serializedIsKinematic.IsDefined
			|| serializedIsTrigger.IsDefined
			|| serializedAtlas.IsDefined
			|| serializedSharedMaterial.IsDefined
			|| serializedBoneInfluenceFalloff.IsDefined
			|| serializedSkinQuality.IsDefined
			|| serializedPhysicsSkinMode.IsDefined
			|| serializedRenderSkinMode.IsDefined;
	}

	public void RevertValues( )
	{
		serializedTexture.Revert( );
		serializedTextureGUID.Revert( );

		serializedAtlas.Revert( );
		
		serializedSharedMaterial.Revert( );

		serializedSpriteScaleMode.Revert( );
		serializedSpriteScale.Revert( );
		serializedSpriteScaleNotUniform.Revert( );

		serializedVertexColor.Revert( );

		serializedExtrusionDepth.Revert( );
		serializedIsKinematic.Revert( );
		serializedIsTrigger.Revert();

		serializedPivotType.Revert( );
		serializedPivotCustomCoords.Revert( );

		serializedBoneInfluenceFalloff.Revert( );
		serializedSkinQuality.Revert( );
		serializedPhysicsSkinMode.Revert( );
		serializedRenderSkinMode.Revert( );
	}
}

#endif