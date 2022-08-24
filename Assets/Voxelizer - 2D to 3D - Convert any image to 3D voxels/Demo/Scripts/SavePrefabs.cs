
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

/// <summary>
/// Creates a prefab from a selected game object.
/// </summary>
/// 

class SavePrefabs  : MonoBehaviour {
	
	public void CreatePrefabsMultipleObjects(GameObject go){

		if (!Directory.Exists ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs")) {
			AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels", "SavedPrefabs");
			AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Objects");
			AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Meshes");
			AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Materials");	
		}	

		GameObject prefabs = go;

		List<Material> materials = new List<Material> ();

		foreach (Transform child in prefabs.transform) {
			Material mat = child.GetComponent<Renderer> ().sharedMaterial;
			if(!materials.Contains (mat)){	materials.Add (mat);}
		}
			
		foreach (Material mat in materials) {
			
			string matPath = "Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs/Materials/" + prefabs.name +" "+ mat.color.ToString ()+".mat" ;
			AssetDatabase.CreateAsset(mat, matPath);
			AssetDatabase.SaveAssets();
		}

		string goPath = "Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs/Objects/" + prefabs.name + ".prefab";
		PrefabUtility.CreatePrefab(goPath,prefabs,UnityEditor.ReplacePrefabOptions.Default);
	}
		

	public Mesh CombineMesh(GameObject go)
	{

		MeshFilter[] mfChildren = go.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[mfChildren.Length];

		MeshRenderer[] mrChildren = go.GetComponentsInChildren<MeshRenderer>();

		MeshRenderer mrSelf = go.GetComponent<MeshRenderer>();
		MeshFilter mfSelf = go.GetComponent<MeshFilter>();

		List<Material> materials = new List<Material>();
		for (int i = 0; i < mrChildren.Length; i++) {
			Material mat = mrChildren[i].sharedMaterial;
			materials.Add(mat);
		}
		mrSelf.materials = materials.ToArray();

		Mesh newMesh = new Mesh();

		MeshRenderer meshRendererCombine = go.GetComponent<MeshRenderer> ();

		for (int i = 0; i < mfChildren.Length; i++){
			if (!meshRendererCombine)
				meshRendererCombine = go.AddComponent<MeshRenderer> ();   

			combine[i].mesh = mfChildren[i].sharedMesh;
			combine[i].transform = mfChildren[i].transform.localToWorldMatrix;

		}

		newMesh.CombineMeshes(combine, true, true);
		mfSelf.mesh = newMesh;

		return mfSelf.sharedMesh;
	}
		
	public void SaveAsPrefab (GameObject go) {

		try {
			MeshFilter filter = go.GetComponent<MeshFilter> ();
			Mesh mesh;
			if (filter != null) {
				mesh = filter.sharedMesh;
			} else {
				mesh = CombineMesh(go);
			}

			if (!Directory.Exists ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs")) {
				AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels", "SavedPrefabs");
				AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Objects");
				AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Meshes");
				AssetDatabase.CreateFolder ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs", "Materials");	
			}
				
			List<Material> materials = new List<Material> ();
			foreach (Material mat in go.GetComponent<Renderer> ().sharedMaterials) {
				if(!materials.Contains (mat)){	materials.Add (mat);}
			}

			foreach (Material mat in materials) {
				string matPath = "Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs/Materials/" + go.name +" "+ mat.color.ToString ()+".mat" ;//   +"RGB(" + (int)(mat.color.r/255.0) +"," + (int)(mat.color.g/255.0) +","+ + (int)(mat.color.b/255.0) +")"+".mat";
				AssetDatabase.CreateAsset(mat, matPath);
			}


			AssetDatabase.CreateAsset(mesh, "Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs/Meshes/"+go.name+".asset");
			PrefabUtility.CreatePrefab ("Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/SavedPrefabs/Objects/"+go.name+".prefab", gameObject,ReplacePrefabOptions.Default);
			AssetDatabase.SaveAssets();

			Debug.Log("[Voxelizer] Object saved correctly!");

		} catch (System.Exception ex) {
			Debug.LogWarning ("[Voxelizer] Error saving object: "+ex);
		}

	}

	public void Combine()
	{
		// Find all mesh filter submeshes and separate them by their cooresponding materials
		ArrayList materials = new ArrayList();
		ArrayList combineInstanceArrays = new ArrayList();

		GameObject obj = transform.gameObject;

		MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();

		foreach( MeshFilter meshFilter in meshFilters )
		{
			MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

			// Handle bad input
			if(!meshRenderer) { 
				Debug.LogError("MeshFilter does not have a coresponding MeshRenderer."); 
				continue; 
			}
				
			if(meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount) { 
				Debug.LogError("Mismatch between material count and submesh count. Is this the correct MeshRenderer?"); 
				continue; 
			}

			for(int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
			{
				int materialArrayIndex = 0;
				for(materialArrayIndex = 0; materialArrayIndex < materials.Count; materialArrayIndex++)
				{
					if(materials[materialArrayIndex] == meshRenderer.sharedMaterials[s])
						break;
				}

				if(materialArrayIndex == materials.Count)
				{
					materials.Add(meshRenderer.sharedMaterials[s]);
					combineInstanceArrays.Add(new ArrayList());
				}
				Matrix4x4 myTransform = transform.worldToLocalMatrix;		

				CombineInstance combineInstance = new CombineInstance();
				combineInstance.transform = myTransform * meshRenderer.transform.localToWorldMatrix;
				combineInstance.subMeshIndex = s;
				combineInstance.mesh = meshFilter.sharedMesh;
				(combineInstanceArrays[materialArrayIndex] as ArrayList).Add( combineInstance );
			}
		}


		// For MeshFilter
		{
			// Get / Create mesh filter
			MeshFilter meshFilterCombine = gameObject.GetComponent<MeshFilter>();
			if(!meshFilterCombine)
				meshFilterCombine = gameObject.AddComponent<MeshFilter>();

			// Combine by material index into per-material meshes
			// also, Create CombineInstance array for next step
			Mesh[] meshes = new Mesh[materials.Count];
			CombineInstance[] combineInstances = new CombineInstance[materials.Count];

			for( int m = 0; m < materials.Count; m++ )
			{
				CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];

				meshes[m] = new Mesh();
				meshes[m].CombineMeshes( combineInstanceArray, true, true );



				combineInstances[m] = new CombineInstance();
				combineInstances[m].mesh = meshes[m];
				combineInstances[m].subMeshIndex = 0;
			}

			// Combine into one
			meshFilterCombine.sharedMesh = new Mesh();
			meshFilterCombine.sharedMesh.CombineMeshes( combineInstances, false, false );

			// Destroy other meshes
			foreach( Mesh mesh in meshes )
			{
				mesh.Clear();
				DestroyImmediate(mesh);
			}
		}

		// For MeshRenderer
		{
			// Get / Create mesh renderer
			MeshRenderer meshRendererCombine = gameObject.GetComponent<MeshRenderer>();
			if(!meshRendererCombine)
				meshRendererCombine = gameObject.AddComponent<MeshRenderer>();    

			// Assign materials
			Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
			meshRendererCombine.materials = materialsArray;    
		}
	}

}
#endif