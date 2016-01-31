using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Uni2DMesh2D
{
	public Vector2[] vertices;
	public Matrix4x4[] bindposes;
	public Uni2DBoneWeight[] boneWeights;
	
	public BoneWeight[] GetBoneWeightStructs()
	{
		if(boneWeights == null)
		{
			return null;
		}
		
		int iBoneWeightCount = boneWeights.Length;
		
		BoneWeight[] oBoneWeights = new BoneWeight[iBoneWeightCount];
		for(int i = 0; i < iBoneWeightCount; ++i)
		{
			oBoneWeights[i] = boneWeights[i];
		}
		
		return oBoneWeights;
	}
}