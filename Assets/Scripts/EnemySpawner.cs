using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public List<GameObject> enemyPrefabs;
	public static EnemySpawner self;
	public static bool playerIsInEncounter = false;
	public static GameObject currentEnemy;
	public GameObject enemyUIObject;
	public GameObject controlsUIObject;
	public GameObject navsUIObject;
	public GameObject compasUIObject;

	// Use this for initialization
	void Start () {
		if(self == null){
			self = this;
		} else {
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(currentEnemy != null && currentEnemy.GetComponent<Enemy>().health <= 0){
			Player.self.exp += currentEnemy.GetComponent<Enemy>().exp;
			Destroy(currentEnemy);
			currentEnemy = null; 
			playerIsInEncounter = false;
		}

		if(playerIsInEncounter && enemyUIObject != null){
			enemyUIObject.SetActive(true);
			controlsUIObject.SetActive(false);
			navsUIObject.SetActive(false);
			compasUIObject.SetActive(true);
			float damage = currentEnemy.GetComponent<Enemy>().damage;
			float damageScalar = currentEnemy.GetComponent<Enemy>().damageScalar;
			Player.self.health -= Time.deltaTime * (damage + (damage * (Player.self.level * damageScalar)));
		} else {
			enemyUIObject.SetActive(false);
			controlsUIObject.SetActive(true);
			navsUIObject.SetActive(true);
			compasUIObject.SetActive(true);
		}
	}

	public static void TestEncounter(Vector3 position){
		bool test = WorldMap.testForWalkable(position);
		if(test){
			EnemySpawner.StartEncounter(position);
		}
	}

	//This is called randomly on movement if the space in front of the player is not a urban village or occupied.
	public static void StartEncounter(Vector3 position){
		if(Random.Range(0.0f, 100.0f) <= 20.0f){
			int randomEnemy = (int)Random.Range(0, EnemySpawner.self.enemyPrefabs.Count);
			currentEnemy = (GameObject)Instantiate(EnemySpawner.self.enemyPrefabs[randomEnemy], position, Quaternion.Euler(0,Player.self.transform.eulerAngles.y-180, 0));

			currentEnemy.transform.position += new Vector3(0, currentEnemy.GetComponent<Enemy>().heightOffset, 0);
			playerIsInEncounter = true;
		}
	}
}
