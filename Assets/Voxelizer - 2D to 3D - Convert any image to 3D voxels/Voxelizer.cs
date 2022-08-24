#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Voxelizer : MonoBehaviour {

	public Texture2D imageToTrasform;
	public Material  materialReference;
	public GameObject objReference;
	public ParticleSystem particleEffect;
	public bool singleObject = false;

	public bool useProceduralGeneration = false;

	public bool excludeWhite = false;
	public bool excludeBlack = false;
	public bool excludeAlpha = false;

	[Range(0,255)]
	public int alphaThreshold = 20;

	public List<Color> excludeColors;

	public bool addRigidBody = false;
	public bool useGravity = false;
	public bool verticalPosition = true;

	void Start () {
		excludeColors = new List<Color> ();
		MeshFilter filter = GetComponent <MeshFilter> ();

		if (transform.childCount == 0 || (filter && filter.mesh.vertexCount == 0)){
			if (useProceduralGeneration) {
				StartCoroutine (GenerateProceduralVoxel ());
			} else {
				GenerateVoxelInstant ();
			}
		}
	}
		
	public void GenerateSingleObject(){
		
		if (transform.childCount == 0) {
			GenerateVoxelInstant ();
		}
		DestroyRenderer ();

		SavePrefabs savePrbs = transform.GetComponent <SavePrefabs> ();
		if (savePrbs) {
			savePrbs.Combine ();
		} else {
			gameObject.AddComponent <SavePrefabs> ().Combine ();
		}

		DestroyVoxel ();

	}
	public void GenerateVoxel(){
		if (singleObject) {
			GenerateSingleObject ();
		} else {
			GenerateVoxelInstant ();
		}
	}
		
	public void GenerateVoxelInstant() {

		if (transform.childCount > 0) {
			DestroyVoxel ();
		}

		Texture2D tex = imageToTrasform;
		Vector3 v3Start = new Vector3(-tex.width / 2.0f, 0.0f, -tex.height / 2.0f);
		Color32[] arcolors = tex.GetPixels32();

		for (int i = 0; i < tex.width; i++) {
			for (int j = 0; j < tex.height; j++) {
				Vector3 v3Pos = new Vector3(v3Start.x + i, 0.0f, v3Start.z + j);
				BuildColumn(v3Pos,arcolors[i + tex.width * j]);
			}
		} 

		foreach (Transform child in transform) {
			if (addRigidBody) {
				
				Rigidbody rigidBody = child.gameObject.GetComponent <Rigidbody> ();	
				Brick brick = child.gameObject.AddComponent<Brick>();

				if (!rigidBody) {
					rigidBody = child.gameObject.AddComponent<Rigidbody> ();
					rigidBody.isKinematic = true;
					rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
					
				}

				if (!useGravity) {
					rigidBody.useGravity = false;
				} else {
					rigidBody.useGravity = true;

				}
				brick.particle = particleEffect;
				brick.SetComponents();
			}
		}
	}

	IEnumerator GenerateProceduralVoxel() {

		Texture2D tex = imageToTrasform;

		Vector3 v3Start = new Vector3(-tex.width / 2.0f, 0.0f, -tex.height / 2.0f);

		Color32[] arcolors = tex.GetPixels32();

		for (int i = 0; i < tex.width; i++) {
			for (int j = 0; j < tex.height; j++) {
				Vector3 v3Pos = new Vector3(v3Start.x + i, 0.0f, v3Start.z + j);
				BuildColumn(v3Pos,arcolors[i + tex.width * j]);
			}
			yield return 0;
		} 

		foreach (Transform child in transform) {
			if (addRigidBody) {
				child.gameObject.AddComponent<Rigidbody> ();	
				if (!useGravity) {
					child.gameObject.GetComponent<Rigidbody> ().useGravity = false;
				}
			}
		}
	}
		
	void BuildColumn(Vector3 v3Pos, Color32 c32) {

		if (excludeAlpha && c32.a < alphaThreshold) {
			return;
		}

		if (excludeBlack && c32 == Color.black) {
			return;

		}
		if (excludeWhite && c32 == Color.white) {
			return;

		}
		if (excludeAlpha && c32.a/255.0f == 0) {
			return;
		}
		if (excludeColors == null) {
			excludeColors = new List<Color> ();
		}
		foreach (Color excludeColor in excludeColors){
			if( SimilarFloat (c32.r/255.0f, excludeColor.r)  &&  SimilarFloat (c32.g/255.0f, excludeColor.g) &&  SimilarFloat (c32.b/255.0f, excludeColor.b)){return;}
		}


		Material matColor = null;

		if (transform.childCount == 0) {
			matColor = new Material( materialReference );
			matColor.color = c32;
		} else {

			bool materialExist = false;
			foreach (Transform child in transform) {
				
				Material mat = child.GetComponent<Renderer> ().sharedMaterial;
				if( SimilarFloat (c32.r/255.0f, mat.color.r)  &&  SimilarFloat (c32.g/255.0f, mat.color.g) &&  SimilarFloat (c32.b/255.0f, mat.color.b)){
					matColor = mat;
					materialExist = true;
					break;
				}  	
			}
			if (!materialExist) {
				matColor = new Material (materialReference);
				matColor.color = c32;
			}
		}

		GameObject go;

		if(objReference){
			go =Instantiate( objReference);
		}else{
			go = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}


		go.transform.parent = transform;

		// ADD OPTIONS
		if(verticalPosition){
			go.transform.localPosition = new Vector3 (v3Pos.x, v3Pos.z,0);
		}else{
			go.transform.localPosition = new Vector3 (v3Pos.x, 0, v3Pos.z);
		}

		go.GetComponent<Renderer>().material = matColor;
	}



	//Destroy Voxel
	public void DestroyAll(){
		foreach (Transform child in transform) {
			DestroyImmediate (child.gameObject);
		}
		if (transform.childCount > 0) {
			DestroyVoxel ();
		}
		DestroyRenderer ();
	}

	public void DestroyVoxel(){

		foreach (Transform child in transform) {

			if (Application.isPlaying) {
				Destroy (child.gameObject);
			} else {
				DestroyImmediate (child.gameObject);

				if (transform.childCount > 0) {
					Debug.Log (" Child count Voxel" + transform.childCount);

					DestroyVoxel ();
				}
			}
		}
			
	}
		
	public void DestroyRenderer(){


		#if UNITY_EDITOR
			DestroyImmediate (transform.GetComponent <MeshFilter> () );
			DestroyImmediate (transform.GetComponent <MeshRenderer> () );
		#else
			Destroy (transform.GetComponent <MeshFilter> () );
			Destroy(transform.GetComponent <MeshRenderer> () );
		#endif


	}

	//Saving Voxel
	public void SaveVoxel(){

		if (transform.childCount > 0) {
			SavePrefabsMultipleObjects ();
		} else {
			SavePrefabs ();
		}

	}

	public void SavePrefabsMultipleObjects(){

		if (transform.GetComponent <SavePrefabs> () != null) {
			transform.GetComponent <SavePrefabs> ().CreatePrefabsMultipleObjects (transform.gameObject);
		} else {
			transform.gameObject.AddComponent<SavePrefabs> ().CreatePrefabsMultipleObjects (transform.gameObject);
		}

	}

	public void SavePrefabs(){
		if (transform.GetComponent <SavePrefabs> () != null) {
			transform.GetComponent <SavePrefabs> ().SaveAsPrefab (transform.gameObject);
		} else {
			transform.gameObject.AddComponent<SavePrefabs> ().SaveAsPrefab (transform.gameObject);
		}
	}

	// Utilities
	bool SimilarFloat ( float first, float second){
		if (Mathf.Abs (first - second) < 0.00001) {
			return true;
		}
		return false;
	}
		
}
#endif