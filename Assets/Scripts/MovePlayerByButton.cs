using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerByButton : MonoBehaviour {
	public MoveDirection direction;

	public void PressButton(){
		Player.self.SendMessage("MovePlayer", direction, SendMessageOptions.RequireReceiver);
	}
}
