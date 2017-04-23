using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompasNeedle : MonoBehaviour {
	public Quaternion targetRotation;
	public TurnDirection oldTurn;
	private float slerpTimeRotation;
	float turnDamping = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Player.currentDirection != oldTurn){
			slerpTimeRotation = 0;
			oldTurn = Player.currentDirection;
		}

		if(slerpTimeRotation <= 1.0f){
			switch(Player.currentDirection){
				case TurnDirection.NORTH:
					targetRotation = Quaternion.Euler(new Vector3(0,0,0));
				break;
				case TurnDirection.SOUTH:
					targetRotation = Quaternion.Euler(new Vector3(0,0,180));
				break;
				case TurnDirection.EAST:
					targetRotation = Quaternion.Euler(new Vector3(0,0,-90));
				break;
				case TurnDirection.WEST:
					targetRotation = Quaternion.Euler(new Vector3(0,0,90));
				break;
			}
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, slerpTimeRotation);
			slerpTimeRotation += Time.deltaTime * turnDamping;
		}
	}
}
