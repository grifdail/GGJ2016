#if UNITY_EDITOR

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;

using System;

public static class Uni2DUndo
{
	public static void RegisterFullObjectHierarchyUndo(UnityEngine.Object a_rObjectToUndo, string a_oUndoName)
	{
		try
		{
			#if AFTER_UNITY_4_3 
				#if UNITY_4_3
				If_Your_Are_Not_On_Unity_4_3_And_This_Method_Is_Called_Recompile_The_File_Uni2DUndo_cs(a_rObjectToUndo);
				#else
				RegisterFullObjectHierarchyUndoOnUnity_4_5AndBeyond(a_rObjectToUndo, a_oUndoName);
				#endif
			#endif
		}
		catch
		{
			RegisterFullObjectHierarchyUndoOnUnity_4_5AndBeyond(a_rObjectToUndo, a_oUndoName);
		}
	}
	
	public static void If_Your_Are_Not_On_Unity_4_3_And_This_Method_Is_Called_Recompile_The_File_Uni2DUndo_cs(UnityEngine.Object a_rObjectToUndo)
	{
		//Debug.Log("4_3");
		#if UNITY_4_3
		
		Transform rTransformToUndo = null;
		if(a_rObjectToUndo is GameObject)
		{
			rTransformToUndo = (a_rObjectToUndo as GameObject).transform;
		}
		else if(a_rObjectToUndo is Component)
		{
			rTransformToUndo = (a_rObjectToUndo as Component).transform;
		}
		
		Transform rParentSave = null;
		Vector3 f3LocalPositionSave = Vector3.zero;
		Quaternion oLocalRotationSave = Quaternion.identity;
		Vector3 f3LocalScaleSave = Vector3.one;
		if(rTransformToUndo != null)
		{
			rParentSave = rTransformToUndo.parent;
			f3LocalPositionSave = rTransformToUndo.localPosition;
			oLocalRotationSave = rTransformToUndo.localRotation;
			f3LocalScaleSave = rTransformToUndo.localScale;
		}
		Undo.RegisterFullObjectHierarchyUndo(a_rObjectToUndo);
		if(rParentSave != null)
		{
			rTransformToUndo.parent = rParentSave;
			rTransformToUndo.localPosition = f3LocalPositionSave;
			rTransformToUndo.localRotation = oLocalRotationSave;
			rTransformToUndo.localScale = f3LocalScaleSave;
		}
		#endif
	}
	
	public static void RegisterFullObjectHierarchyUndoOnUnity_4_5AndBeyond(UnityEngine.Object a_rObjectToUndo, string a_oUndoName)
	{
		//Debug.Log("4_5");
		#if AFTER_UNITY_4_3 && !UNITY_4_3
		Undo.RegisterFullObjectHierarchyUndo(a_rObjectToUndo, a_oUndoName);
		#endif
	}
}
#endif