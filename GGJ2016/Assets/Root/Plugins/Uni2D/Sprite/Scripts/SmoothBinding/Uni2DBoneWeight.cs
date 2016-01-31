using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Uni2DBoneWeight
{
	public int boneIndex0;
	
	public int boneIndex1;
	
	public int boneIndex2;
	
	public int boneIndex3;
	
	public float weight0;
	
	public float weight1;
	
	public float weight2;
	
	public float weight3;
	
	public Uni2DBoneWeight(BoneWeight a_oBoneWeight)
	{
		boneIndex0 = a_oBoneWeight.boneIndex0;
		boneIndex1 = a_oBoneWeight.boneIndex1;
		boneIndex2 = a_oBoneWeight.boneIndex2;
		boneIndex3 = a_oBoneWeight.boneIndex3;
		
		weight0 = a_oBoneWeight.weight0;
		weight1 = a_oBoneWeight.weight1;
		weight2 = a_oBoneWeight.weight2;
		weight3 = a_oBoneWeight.weight3;
	}
	
	public static implicit operator BoneWeight(Uni2DBoneWeight a_rBoneWeight)  // implicit digit to byte conversion operator
	{
		BoneWeight oBoneWeight = new BoneWeight();
		
		oBoneWeight.boneIndex0 = a_rBoneWeight.boneIndex0;
		oBoneWeight.boneIndex1 = a_rBoneWeight.boneIndex1;
		oBoneWeight.boneIndex2 = a_rBoneWeight.boneIndex2;
		oBoneWeight.boneIndex3 = a_rBoneWeight.boneIndex3;
		
		oBoneWeight.weight0 = a_rBoneWeight.weight0;
		oBoneWeight.weight1 = a_rBoneWeight.weight1;
		oBoneWeight.weight2 = a_rBoneWeight.weight2;
		oBoneWeight.weight3 = a_rBoneWeight.weight3;
		
		return oBoneWeight;
	}
	
	public static implicit operator Uni2DBoneWeight(BoneWeight a_oBoneWeight)  // implicit digit to byte conversion operator
	{
		Uni2DBoneWeight oBoneWeight = new Uni2DBoneWeight(a_oBoneWeight);
		
		return oBoneWeight;
	}
}