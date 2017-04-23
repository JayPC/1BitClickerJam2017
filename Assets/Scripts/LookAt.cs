using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
	public GameObject target;
	public float damping = 0;
	public bool slerpTurn;
	// Use this for initialization
	void Start () {
		target = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 lookPos = 2*(target.transform.position - transform.position);
		lookPos.y = 0;
		Quaternion qRotation;
		if (lookPos != Vector3.zero) {
			qRotation = Quaternion.LookRotation(-lookPos);
		} else {
			qRotation = Quaternion.identity;
		}
		
		if(slerpTurn){
			transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, Time.deltaTime * damping);
		} else {
			transform.rotation = qRotation; 
		}
	}
}