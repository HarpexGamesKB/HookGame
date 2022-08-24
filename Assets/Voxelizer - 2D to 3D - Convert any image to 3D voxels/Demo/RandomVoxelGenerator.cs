#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RandomVoxelGenerator : MonoBehaviour {
	public string buttonName = "Fire1";
	public Vector3 spawnPosition;
	public PhysicMaterial phyMat;

	public Material material;
	private List<string> filesPath;
	public string assetDirectory = "Assets/Voxelizer - 2D to 3D - Convert any image to 3D voxels/Pictures/Fantasy";

	void Start(){
		filesPath = new List<string>();
		foreach (string file in System.IO.Directory.GetFiles(assetDirectory,"*.png")){
			filesPath.Add (file);
		}
	}

	void Update () {

		if(Input.GetButtonDown(buttonName) || Input.GetKeyDown(KeyCode.Space) ){

		string file = (string)filesPath[Random.Range(0, filesPath.Count)];
			Object[] data = AssetDatabase.LoadAllAssetsAtPath(file);
			Texture2D text = (Texture2D)data [0];
			GameObject voxelGameObj = new GameObject ();
			Voxelizer vox = voxelGameObj.AddComponent <Voxelizer>();
			vox.transform.position = spawnPosition;
			voxelGameObj.name = "Spawn";
			vox.materialReference = material;
			vox.imageToTrasform = text;
			vox.excludeAlpha = true;
			vox.singleObject = true;
			vox.useGravity = false;
			vox.addRigidBody = false;
			vox.GenerateSingleObject ();
			voxelGameObj.AddComponent <MeshCollider>().sharedMesh =voxelGameObj.GetComponent<MeshCollider>().sharedMesh ;
			voxelGameObj.GetComponent <MeshCollider> ().convex = true; 
			voxelGameObj.GetComponent <MeshCollider> ().material = phyMat; 

			voxelGameObj.AddComponent <Rigidbody>().useGravity = true;
			voxelGameObj.GetComponent <Rigidbody>().isKinematic= false;
		}

	}
}
#endif