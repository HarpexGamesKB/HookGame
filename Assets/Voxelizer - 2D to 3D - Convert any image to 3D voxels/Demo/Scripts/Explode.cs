using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class Explode : MonoBehaviour {
	public float radius = 100.0F;
	public float power = 100.0F;

	void Start() {
		Rigidbody rb = transform.GetComponent<Rigidbody>();
		rb.AddExplosionForce(power, transform.position, radius, 3.0F);
	}
}