#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class MultiUnityVersionSupportUtility
{

#if UNITY_EDITOR
	public static bool InAnimationMode()
	{
		#if AFTER_UNITY_4_3
		return AnimationMode.InAnimationMode();
		#else
		return AnimationUtility.InAnimationMode();
		#endif
	}
#endif
}
