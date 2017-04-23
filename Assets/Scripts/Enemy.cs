using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	public float heightOffset = 0.5f;
	public Text healthText;

	public float health = 9.0f;
	public float baseHealth = 9.0f;
	public float healthScalar = 1.3f;
	private float maxHealth;

	public float exp = 10.0f;
	public float baseExp = 10.0f;
	public float expScalar = 1.3f;

	public float damage = 1;
	public float damageScalar = 1;

	void Start(){
		health = baseHealth + (healthScalar * Player.self.level); // 10 base health + 1 health per level default
		maxHealth = health;
		exp = baseExp + (expScalar * Player.self.level);
	}
	void Update(){
		healthText.text = "Health " + health + "/" +maxHealth;
	}
}
