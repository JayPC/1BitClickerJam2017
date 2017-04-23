using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Fight(){
		float damage = Player.self.damage;
		if(EnemySpawner.currentEnemy != null){
			EnemySpawner.currentEnemy.GetComponent<Enemy>().health -= damage;
		}
	}
}
