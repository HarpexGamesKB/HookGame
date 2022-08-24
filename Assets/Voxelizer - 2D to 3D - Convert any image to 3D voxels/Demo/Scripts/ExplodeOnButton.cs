using UnityEngine;
using System.Collections;

public class ExplodeOnButton : MonoBehaviour
{
	public string buttonName = "Fire1";
	public float force = 100.0f;
	public float radius = 5.0f;
	public float upwardsModifier = 0.0f;
	public ForceMode forceMode;

	void Update ()
	{
		if(Input.GetButtonDown(buttonName) || Input.GetKeyDown(KeyCode.Space) )
		{
			Debug.Log (" Fire in the hole ");
			foreach(Collider col in Physics.OverlapSphere(transform.position, radius))
			{
				if (col.GetComponent<Rigidbody> () != null) {
					col.GetComponent<Rigidbody> ().AddExplosionForce (force, transform.position, radius, upwardsModifier, forceMode);
				} else {
					Debug.LogWarning ("Rigidbody not found");
				}
			}
		}
	}
}
