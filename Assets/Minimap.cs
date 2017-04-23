using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {
	// public List<GameObject> groundTiles;
	// public List<GameObject> treeTiles;
	// public List<GameObject> villageTiles;
	// public List<GameObject> buildingTiles;
	// public List<GameObject> dungeonTiles;
	// public List<GameObject> npcTiles;

	// private Vector3 playerPosition;
	// public int minimapRenderDistance = 10;

	// public Dictionary<Vector2, GameObject> LoadedGroundTiles;
	// public Dictionary<Vector2, GameObject> LoadedObjectTiles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// playerPosition = Player.self.gameObject.transform.position;
		// int x = (int)playerPosition.x;
		// int y = (int)playerPosition.y;

		// for(int i = (int)playerPosition.y-minimapRenderDistance; i <= (int)playerPosition.y+minimapRenderDistance; i++){
		// 	for(int c = (int)playerPosition.x-minimapRenderDistance; c <= (int)playerPosition.x+minimapRenderDistance; c++){
		// 		if(c >= 0 && c < WorldGenerator.groundTiles.GetLength(0) && i >= 0 && i < WorldGenerator.groundTiles.GetLength(1)){
		// 			if(!LoadedGroundTiles.ContainsKey(new Vector2(c,i))){
		// 				if(groundTiles[WorldGenerator.groundTiles[c,i].tileID] != null){
		// 					GameObject temp = (GameObject)Instantiate(groundTiles[WorldGenerator.groundTiles[c,i].tileID], new Vector3(c,WorldGenerator.terrain[c,i]*GameManager.HeightScale,i), groundTiles[WorldGenerator.groundTiles[c,i].tileID].transform.rotation);
		// 					LoadedGroundTiles.Add(new Vector2(c,i), temp);
		// 				}
		// 			}
		// 			if(!LoadedObjectTiles.ContainsKey(new Vector2(c,i))){
						


		// 				if(treeTiles[WorldGenerator.treeTiles[c,i].tileID] != null && WorldGenerator.treeTiles[c,i].tileID != 0){
		// 					GameObject temp = (GameObject)Instantiate(treeTiles[WorldGenerator.treeTiles[c,i].tileID-1], new Vector3(c,(WorldGenerator.terrain[c,i]*GameManager.HeightScale)+0.5f,i), treeTiles[WorldGenerator.treeTiles[c,i].tileID-1].transform.rotation);
		// 					LoadedObjectTiles.Add(new Vector2(c,i), temp);
		// 				}
		// 			}
		// 		}
		// 	}
		// }
	}
}
