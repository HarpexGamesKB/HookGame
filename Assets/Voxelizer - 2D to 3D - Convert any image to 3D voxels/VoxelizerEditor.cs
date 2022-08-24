using UnityEngine;
using System.Collections;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(Voxelizer))]
public class VoxelizerEditor : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Voxelizer myScript = (Voxelizer)target;
		if(GUILayout.Button("Generate Voxel"))
		{
			myScript.GenerateVoxel();
		}	
		if(GUILayout.Button("Save Voxel"))
		{
			myScript.SaveVoxel();
		}

		if(GUILayout.Button("Destroy Voxel"))
		{
			myScript.DestroyAll();
		}

	}

}
#endif