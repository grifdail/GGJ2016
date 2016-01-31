using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Skinning utils
public static class Uni2DSkinningUtils
{
	public static Vector2[] ApplySkinBlend(Uni2DMesh2D a_rMesh2D, Transform[] a_rJointCurrentTransforms, Transform a_rOriginTransform, SkinQuality a_eSkinQuality = SkinQuality.Auto)
	{		
		Vector2[] rVertices = a_rMesh2D.vertices;
		int iVertexCount = rVertices.Length;		
		Vector3[] oVertices3D = new Vector3[iVertexCount];
		for(int i = 0; i < iVertexCount; ++i)
		{
			oVertices3D[i] = (Vector3)rVertices[i];
		}
		
		Vector3[] oBlendedVertices3D = new Vector3[iVertexCount];
		ApplySkinBlend(ref oBlendedVertices3D, oVertices3D, a_rMesh2D.bindposes, a_rMesh2D.GetBoneWeightStructs(), a_rJointCurrentTransforms, a_rOriginTransform, a_eSkinQuality);
		
		
		Vector2[] oBlendedVertices = new Vector2[iVertexCount];
		for(int i = 0; i < iVertexCount; ++i)
		{
			oBlendedVertices[i] = (Vector2)oBlendedVertices3D[i];
		}
		
		return oBlendedVertices;
	}
	
	public static void ApplySkinBlend(ref Vector3[] a_rBlendedVertices, Mesh a_rMesh, Transform[] a_rJointCurrentTransforms, Transform a_rOriginTransform, SkinQuality a_eSkinQuality = SkinQuality.Auto)
	{
		ApplySkinBlend(ref a_rBlendedVertices, a_rMesh.vertices, a_rMesh.bindposes, a_rMesh.boneWeights, a_rJointCurrentTransforms, a_rOriginTransform, a_eSkinQuality); 
	}
				
	public static void ApplySkinBlend(	ref Vector3[] a_rBlendedVertices,
										Vector3[] a_rVertices,
	                                	Matrix4x4[] a_rBindPoses,
	                                	BoneWeight[] a_rBoneWeights,
	                                	Transform[] a_rJointCurrentTransforms, Transform a_rOriginTransform, SkinQuality a_eSkinQuality = SkinQuality.Auto)
	{
		if(a_rJointCurrentTransforms.Length <= 0)
		{
			return;
		}
		
		// The skin quality is either 1, 2 or 4 bones
		int iSkinQuality;
		switch(a_eSkinQuality)
		{	
			case SkinQuality.Bone1:
			{
				iSkinQuality = 1;
			}
			break;
			
			case SkinQuality.Bone2:
			{
				iSkinQuality = 2;
			}
			break;
			
			case SkinQuality.Bone4:
			{
				iSkinQuality = 4;
			}
			break;
			
			case SkinQuality.Auto:
			default:
			{
				switch(QualitySettings.blendWeights)
				{
					case BlendWeights.OneBone:
					{
						iSkinQuality = 1;
					}
					break;
				
					case BlendWeights.TwoBones:
					{
						iSkinQuality = 2;
					}
					break;
				
					case BlendWeights.FourBones:
					default:
					{
						iSkinQuality = 4;
					}
					break;
				}
			}
			break;
		}
		
		int iVertexCount = a_rVertices.Length;
		for(int i = 0; i < iVertexCount; i++)
		{
			Vector3 f3VertexPosition = a_rVertices[i];
			BoneWeight oBoneWeight = a_rBoneWeights[i];
			
			Vector3	f3NewVertexPosition = Vector3.zero;
			
			// Get the bone weight components into an array
			float[] oBoneWeightValues = new float[iSkinQuality];
			int[] oBoneIndices = new int[iSkinQuality];
			for(int k = 0; k < iSkinQuality; k++)
			{
				float fBoneWeightValue = 0.0f;
				int iBoneIndex = 0;
				switch(k)
				{
					case 0:
					{
						fBoneWeightValue = oBoneWeight.weight0;
						iBoneIndex = oBoneWeight.boneIndex0;
					}
					break;
					
					case 1:
					{
						fBoneWeightValue = oBoneWeight.weight1;
						iBoneIndex = oBoneWeight.boneIndex1;
					}
					break;
					
					case 2:
					{
						fBoneWeightValue = oBoneWeight.weight2;
						iBoneIndex = oBoneWeight.boneIndex2;
					}
					break;
					
					case 3:
					{
						fBoneWeightValue = oBoneWeight.weight3;
						iBoneIndex = oBoneWeight.boneIndex3;
					}
					break;
				}
				
				oBoneWeightValues[k] = fBoneWeightValue;
				oBoneIndices[k] = iBoneIndex;
			}
			
			// Normalize the bone weights
			float fTotalWeightValue = 0.0f;
			for(int k = 0; k < iSkinQuality; k++)
			{
				fTotalWeightValue += oBoneWeightValues[k];
			}
			if(fTotalWeightValue != 0.0f)
			{
				for(int k = 0; k < iSkinQuality; k++)
				{
					oBoneWeightValues[k] /= fTotalWeightValue;
				}
			}
			
			// Transform the point
			for(int k = 0; k < iSkinQuality; k++)
			{
				int iBoneIndex = oBoneIndices[k];
				Transform rBoneTransform = a_rJointCurrentTransforms[iBoneIndex];
				
				Vector3 f3VertexPositionInfluencedByCurrentBone;
				if(rBoneTransform == null)
				{
					f3VertexPositionInfluencedByCurrentBone = f3VertexPosition;
				}
				else
				{
					Matrix4x4 oBoneCurrentPose = a_rOriginTransform.worldToLocalMatrix * rBoneTransform.localToWorldMatrix;
					Matrix4x4 oBindPose = a_rBindPoses[iBoneIndex];
					
					f3VertexPositionInfluencedByCurrentBone = (oBoneCurrentPose * oBindPose).MultiplyPoint(f3VertexPosition);
				}
				
				f3NewVertexPosition +=  f3VertexPositionInfluencedByCurrentBone * oBoneWeightValues[k];
			}
			
			a_rBlendedVertices[i] = f3NewVertexPosition;
		}
	}
}