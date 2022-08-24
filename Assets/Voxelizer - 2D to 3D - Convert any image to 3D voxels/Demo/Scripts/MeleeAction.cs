using UnityEngine;
using System.Collections;

public class MeleeAction : MonoBehaviour {
	public string buttonName = "Fire1";

	Animation anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
	}
	// Update is called once per frame
	void Update () {
	if(Input.GetButtonDown(buttonName) ){
		anim.Play ("SwordSwing");
		}
	}
}

