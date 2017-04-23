using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherTransparency : MonoBehaviour {
	public GameObject target;
	public static int renderDistance = 10;

	// Use this for initialization
	void Start () {
		target = Camera.main.gameObject;
		this.GetComponent<Renderer>().material.SetFloat("_Transparency", 0);
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Renderer>().material.SetFloat("_Transparency", renderDistance-Vector3.Distance(this.transform.position, target.transform.position));
	}
}
