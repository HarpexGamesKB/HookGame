using UnityEngine;
using System.Collections;

public class Bricks : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
	//	GM.instance.DestroyBrick ();
		Destroy (gameObject);
	}
}
