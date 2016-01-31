using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ParallaxLayer))]
[CanEditMultipleObjects]
public class ParallaxLayerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		// Parallax mode
		SerializedProperty rSerializedParallaxMode = serializedObject.FindProperty("mode");
		EditorGUILayout.PropertyField(rSerializedParallaxMode);
		
		// Selection Mode Parameter
		if(rSerializedParallaxMode.hasMultipleDifferentValues == false)
		{
			ParallaxLayer.EParallaxMode eParallaxMode = (ParallaxLayer.EParallaxMode)rSerializedParallaxMode.intValue;
			
			switch(eParallaxMode)
			{
				case ParallaxLayer.EParallaxMode.Uniform:
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("parallax"), true);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("freezeAxes"), true);
				}
				break;
				
				case ParallaxLayer.EParallaxMode.ByAxis:
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("parallaxByAxis"), true);
				}
				break;
			}
		}
		
		EditorGUI.BeginDisabledGroup(Application.isPlaying);
		{			
			// Camera Selection Mode
			SerializedProperty rSerializedMasterCameraSelectionMode = serializedObject.FindProperty("masterCamera");
			EditorGUILayout.PropertyField(rSerializedMasterCameraSelectionMode, true);
			
			// Selection Mode Parameter
			if(rSerializedMasterCameraSelectionMode.hasMultipleDifferentValues == false)
			{
				ParallaxLayer.EMasterCameraSelectionMode eSelectionMode = (ParallaxLayer.EMasterCameraSelectionMode)rSerializedMasterCameraSelectionMode.intValue;
				
				switch(eSelectionMode)
				{
					case ParallaxLayer.EMasterCameraSelectionMode.ByReference:
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("masterCameraReference"), true);
					}
					break;
					
					case ParallaxLayer.EMasterCameraSelectionMode.ByName:
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("masterCameraName"), true);
					}
					break;
					
					case ParallaxLayer.EMasterCameraSelectionMode.ByTag:
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("masterCameraTag"), true);
					}
					break;
				}
				
				if(eSelectionMode != ParallaxLayer.EMasterCameraSelectionMode.ByReference && Application.isPlaying)
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("masterCameraReference"), true);
				}
			}
		}
		
		serializedObject.ApplyModifiedProperties();
	}
}