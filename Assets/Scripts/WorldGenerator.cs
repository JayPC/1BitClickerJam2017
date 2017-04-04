using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
	public float[,] worldHeights;
	public float[,] temprature;
	public float[,] precip;

	public Dictionary<Vector3, GameObject> debugTiles;
	public GameObject tile;
	int   mapSize = 200;

	//Terrain
	public float terrainScale = 5f;
	public float terrainHeightScale = 100f;
	float terraincuttoff = -28;
	float terrainXOffset = 0;
	float terrainYOffset = 0;

	//Temprature
	public float tempScale = 4f;
	public float tempHeightScale = 2f;
	float tempXOffset = 0;
	float tempYOffset = 0;

	//weather
	public float precipScale = 30f;
	public float precipHeightScale = 2f;
	float precipXOffset = 0;
	float precipYOffset = 0;

	void Start () {
		worldHeights = new float[mapSize,mapSize];
		temprature = new float[mapSize,mapSize];
		precip = new float[mapSize,mapSize];
		debugTiles = new Dictionary<Vector3, GameObject>();

		terrainXOffset = Random.Range(0,300);
		terrainYOffset = Random.Range(0,300);
		tempXOffset = Random.Range(0,300);
		tempYOffset = Random.Range(0,300);
		precipXOffset =  Random.Range(0,300);
		precipYOffset =  Random.Range(0,300);

		worldHeights = makeMap(200, 1, 1, terrainXOffset, terrainYOffset);
		temprature = makeMap(200, 1, 1, tempXOffset, tempYOffset);
		precip = makeMap(200, 1, 1, precipXOffset, precipYOffset);

		for (int i = 0; i < worldHeights.GetLength(0); i++) 
		{
			for (int c = 0; c < worldHeights.GetLength(1); c++) 
			{
				//Make Terrain
				debugTiles.Add(new Vector3(c,i,0), Instantiate(tile, new Vector3(c,i,worldHeights[c,i]), Quaternion.identity));
				debugTiles[new Vector3(c,i,0)].transform.parent = this.transform;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		worldHeights = makeMap(200, terrainScale, terrainHeightScale, terrainXOffset, terrainYOffset);
		//temprature = makeMap(200, tempScale, tempHeightScale, tempXOffset, tempYOffset);
		//precip = makeMap(200, precipScale, precipHeightScale, precipXOffset, precipYOffset);
		for (int i = 0; i < worldHeights.GetLength(0); i++) 
		{
			for (int c = 0; c < worldHeights.GetLength(1); c++) 
			{
				debugTiles[new Vector3(c,i,0)].transform.position = new Vector3(c, i, worldHeights[c,i]);
				debugTiles[new Vector3(c,i,0)].GetComponent<SpriteRenderer>().color = new Color(1-worldHeights[c,i], 0, 0);
			}
		}
	}


	public float[,] makeMap(int mapSize, float scale, float heightScale, float xOffset, float yOffset){
		float[,]temp = new float[mapSize,mapSize];

		for (int i = 0; i < worldHeights.GetLength(0); i++) 
		{
			for (int c = 0; c < worldHeights.GetLength(1); c++) 
			{
				float xCoord = ((xOffset + (float)c) / worldHeights.GetLength(0)) * scale;
				float yCoord = ((yOffset + (float)i) / worldHeights.GetLength(1)) * scale;
				temp[c,i] = (Mathf.PerlinNoise(xCoord, yCoord) * heightScale);
			}
		}
		return temp;
	}
}
