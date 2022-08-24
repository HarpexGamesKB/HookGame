using UnityEngine;
using System.Collections;

public class CollisionDestroy : MonoBehaviour {

	bool destroiable = false;
	public bool useGravity = false;

	void OnCollisionEnter(Collision collision){
		if (!destroiable) {
			destroiable = true;
			Destroy (gameObject.GetComponent<Rigidbody>());

			foreach (Transform child in transform) {

				Rigidbody rigidBody = child.gameObject.GetComponent <Rigidbody> ();	

				if (!rigidBody) {
					rigidBody = child.gameObject.AddComponent<Rigidbody> ();	
				}
				if (!useGravity) {
					rigidBody.useGravity = false;
				} else {
					rigidBody.useGravity = true;

					}
				}
		}
	}
		
}
