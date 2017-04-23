using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class WorldMap : MonoBehaviour {
	public int renderDistance;
	public List<GameObject> groundTiles;
	public List<GameObject> treeObjects;
	public List<GameObject> buildingObjects;
	public List<GameObject> npcObjects;

	public Dictionary<Vector2, GameObject> LoadedGroundTiles;
	public Dictionary<Vector2, GameObject> LoadedObjectTiles;
	public Vector2 oldPosition;
	// Use this for initialization
	void Start () {
		LoadedGroundTiles = new Dictionary<Vector2, GameObject>();
		LoadedObjectTiles = new Dictionary<Vector2, GameObject>();
		oldPosition = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 currentPosition = new Vector2((int)Player.self.gameObject.transform.position.x, (int)Player.self.gameObject.transform.position.z);
		if(oldPosition != currentPosition){
			updateTiles(currentPosition);
		}
	}

	public void updateTiles(Vector2 currentPosition){
		renderDistance = DitherTransparency.renderDistance;
		for(int i = (int)currentPosition.y-renderDistance; i <= (int)currentPosition.y+renderDistance; i++){
			for(int c = (int)currentPosition.x-renderDistance; c <= (int)currentPosition.x+renderDistance; c++){
				if(c >= 0 && c < WorldGenerator.groundTiles.GetLength(0) && i >= 0 && i < WorldGenerator.groundTiles.GetLength(1)){
					if(!LoadedGroundTiles.ContainsKey(new Vector2(c,i))){
						if(groundTiles[WorldGenerator.groundTiles[c,i].tileID] != null){
							GameObject temp = (GameObject)Instantiate(groundTiles[WorldGenerator.groundTiles[c,i].tileID], new Vector3(c,WorldGenerator.terrain[c,i]*GameManager.HeightScale,i), groundTiles[WorldGenerator.groundTiles[c,i].tileID].transform.rotation);
							LoadedGroundTiles.Add(new Vector2(c,i), temp);
						}
					}

					if(!LoadedObjectTiles.ContainsKey(new Vector2(c,i))){
						if(buildingObjects[WorldGenerator.buildingTiles[c,i].tileID] != null && WorldGenerator.buildingTiles[c,i].tileID != 0){
							GameObject temp = (GameObject)Instantiate(buildingObjects[WorldGenerator.buildingTiles[c,i].tileID-1], new Vector3(c,(WorldGenerator.terrain[c,i]*GameManager.HeightScale)+0.5f,i), buildingObjects[WorldGenerator.buildingTiles[c,i].tileID-1].transform.rotation);
							LoadedObjectTiles.Add(new Vector2(c,i), temp);
						} else if(treeObjects[WorldGenerator.treeTiles[c,i].tileID] != null && WorldGenerator.treeTiles[c,i].tileID != 0){
							GameObject temp = (GameObject)Instantiate(treeObjects[WorldGenerator.treeTiles[c,i].tileID-1], new Vector3(c,(WorldGenerator.terrain[c,i]*GameManager.HeightScale)+0.5f,i), treeObjects[WorldGenerator.treeTiles[c,i].tileID-1].transform.rotation);
							LoadedObjectTiles.Add(new Vector2(c,i), temp);
						}
					}
				}
			}
		}
		
		List<Vector2> groundKeys = new List<Vector2>(LoadedGroundTiles.Keys);
		foreach (Vector2 key in groundKeys) 
		{
			GameObject temp = LoadedGroundTiles[key];
			if(temp.transform.position.x > currentPosition.x+renderDistance || temp.transform.position.x < currentPosition.x-renderDistance || 
				temp.transform.position.z > currentPosition.y+renderDistance || temp.transform.position.z < currentPosition.y-renderDistance){
				Destroy(temp);
				LoadedGroundTiles.Remove(key);
			}
		}

		List<Vector2> treeKeys = new List<Vector2>(LoadedObjectTiles.Keys);
		foreach (Vector2 key in treeKeys) 
		{
			GameObject temp = LoadedObjectTiles[key];
			if(temp.transform.position.x > currentPosition.x+renderDistance || temp.transform.position.x < currentPosition.x-renderDistance || 
				temp.transform.position.z > currentPosition.y+renderDistance || temp.transform.position.z < currentPosition.y-renderDistance){
				Destroy(temp);
				LoadedObjectTiles.Remove(key);
			}
		}
	}

	public static int[] walkableTileIDs = {1,2,3,4,5};
	public static bool testForWalkable(Vector3 testPoint){
		Debug.Log("Test For Walkable Building: " + WorldGenerator.buildingTiles[(int)testPoint.x, (int)testPoint.z].tileID);
		Debug.Log("Test For Walkable Tree: " + WorldGenerator.treeTiles[(int)testPoint.x, (int)testPoint.z].tileID);
		if(walkableTileIDs.Contains(WorldGenerator.groundTiles[(int)testPoint.x, (int)testPoint.z].tileID) &&
			WorldGenerator.buildingTiles[(int)testPoint.x, (int)testPoint.z].tileID == 0 &&
			WorldGenerator.treeTiles[(int)testPoint.x, (int)testPoint.z].tileID == 0){
			return true;
		}
		return false;
	}
}
