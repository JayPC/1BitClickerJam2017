using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HeightColor 
{
	public string name;
	[Range(0,1)]
	public float terrainHeight;
	public Color terrainColor;
	public int tileID;
}

public enum MapType{
	TERRAIN,
	TEMPRATURE,
	PRECIP,
	VILLAGE,
	URBAN
}

public struct GroundTile 
{
	public int tileID;
}

public struct BuildingTile
{
	public int tileID;
}

public struct TreeTile 
{
	public int tileID;
}

public class WorldGenerator : MonoBehaviour {
	public int mapSize = 1000;
	public MapType drawCurrentMap;


	[Header("Terrain")]
	public static float[,] terrain;
	public float terrainScale = 5f;
	public float terrainHeightScale = 100f;
	public HeightColor[] terrainHeightColors = new HeightColor[6];

	[Header("Temp")]
	public static float[,] temprature;
	public float tempScale = 4f;
	public float tempHeightScale = 2f;
	public HeightColor[] tempratureHeightColors = new HeightColor[6];

	[Header("Precip")]
	public static float[,] precip;
	public float precipScale = 30f;
	public float precipHeightScale = 2f;
	public HeightColor[] precipHeightColors = new HeightColor[6];

	[Header("Village")]
	public static float[,] village;
	public float villageScale = 30f;
	public float villageHeightScale = 2f;
	public HeightColor[] villageHeightColors = new HeightColor[6];

	[Header("Urban")]
	public static float[,] urban;
	public float urbanScale = 30f;
	public float urbanHeightScale = 2f;
	public HeightColor[] urbanHeightColors = new HeightColor[6];

	[Space]
	float terrainXOffset = 0;
	float terrainYOffset = 0;
	float tempXOffset = 0;
	float tempYOffset = 0;
	float precipXOffset = 0;
	float precipYOffset = 0;
	float villageXOffset = 0;
	float villageYOffset = 0;
	float urbanXOffset = 0;
	float urbanYOffset = 0;

	//Tile Maps
	public static GroundTile[,] groundTiles;
	public static BuildingTile[,] buildingTiles;
	public static TreeTile[,] treeTiles;

	//These should be deletable
	Texture2D terrainTex;
	Texture2D tempratureTex;
	Texture2D precipTex;
	Texture2D villageTex;
	Texture2D urbanTex;	

	//These should be deletable
	Sprite terrainSprite;
	Sprite tempratureSprite;
	Sprite precipSprite;
	Sprite villageSprite;
	Sprite urbanSprite;

	void Awake () {
		terrain = new float[mapSize,mapSize];
		temprature = new float[mapSize,mapSize];
		precip = new float[mapSize,mapSize];
		village = new float[mapSize,mapSize];
		urban = new float[mapSize,mapSize];

		terrainXOffset = Random.Range(0,mapSize*mapSize);
		terrainYOffset = Random.Range(0,mapSize*mapSize);
		tempXOffset = Random.Range(0,mapSize*mapSize);
		tempYOffset = Random.Range(0,mapSize*mapSize);
		precipXOffset =  Random.Range(0,mapSize*mapSize);
		precipYOffset =  Random.Range(0,mapSize*mapSize);

		villageXOffset =  Random.Range(0,mapSize*mapSize);
		villageYOffset =  Random.Range(0,mapSize*mapSize);

		urbanXOffset =  Random.Range(0,mapSize*mapSize);
		urbanYOffset =  Random.Range(0,mapSize*mapSize);


		//Make Noise Maps
		terrain = makeMap(mapSize, terrainScale, terrainHeightScale, terrainXOffset, terrainYOffset);
		terrain = taperMapAtEdges(terrain);
		terrain = normalizeMap(terrain);

		temprature = makeMap(mapSize, tempScale, tempHeightScale, tempXOffset, tempYOffset);
		temprature = normalizeMap(temprature);

		precip = makeMap(mapSize, precipScale, precipHeightScale, precipXOffset, precipYOffset);
		precip = normalizeMap(precip);

		village = makeMap(mapSize, villageScale, villageHeightScale, villageXOffset, villageYOffset);
		village = normalizeMap(village);

		urban = makeMap(mapSize, urbanScale, urbanHeightScale, urbanXOffset, urbanYOffset);
		urban = normalizeMap(urban);

		//Make Tile Maps

		groundTiles = makeTilesFromTerrain(terrain, terrainHeightColors);
		treeTiles = makeTreesFromMaps(groundTiles, temprature, precip);
		buildingTiles = makeBuildingsFromMaps(groundTiles, urban, urbanHeightColors, village, villageHeightColors);






		terrainTex    = createTextureFromMap(terrain, terrainHeightColors);
		tempratureTex = createTextureFromMap(temprature, tempratureHeightColors);
		precipTex     = createTextureFromMap(precip, precipHeightColors);
		villageTex    = createTextureFromMap(village, villageHeightColors);
		urbanTex      = createTextureFromMap(urban, urbanHeightColors);

		terrainSprite    = Sprite.Create(terrainTex, new Rect(0, 0, terrainTex.width, terrainTex.height), new Vector2(0.5f,0.5f),100);
		tempratureSprite = Sprite.Create(tempratureTex, new Rect(0, 0, tempratureTex.width, tempratureTex.height), new Vector2(0.5f,0.5f),100);
		precipSprite     = Sprite.Create(precipTex, new Rect(0, 0, precipTex.width, precipTex.height), new Vector2(0.5f,0.5f),100);
		villageSprite    = Sprite.Create(villageTex, new Rect(0, 0, villageTex.width, villageTex.height), new Vector2(0.5f,0.5f),100);
		urbanSprite      = Sprite.Create(urbanTex, new Rect(0, 0, urbanTex.width, urbanTex.height), new Vector2(0.5f,0.5f),100);

	}
	
	// Update is called once per frame
	void Update () {
		switch (drawCurrentMap) 
		{
			case MapType.TERRAIN:
				this.GetComponent<SpriteRenderer>().sprite = terrainSprite;
				break;
			case MapType.TEMPRATURE:
				this.GetComponent<SpriteRenderer>().sprite = tempratureSprite;
				break;
			case MapType.PRECIP:
				this.GetComponent<SpriteRenderer>().sprite = precipSprite;
				break;
			case MapType.VILLAGE:
				this.GetComponent<SpriteRenderer>().sprite = villageSprite;
				break;
			case MapType.URBAN:
				this.GetComponent<SpriteRenderer>().sprite = urbanSprite;
				break;
		}
	}

//  ██████╗ ███████╗███╗   ██╗███████╗██████╗  █████╗ ████████╗ ██████╗ ██████╗ ███████╗
// ██╔════╝ ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗██╔════╝
// ██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██████╔╝███████║   ██║   ██║   ██║██████╔╝███████╗
// ██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██╔══██║   ██║   ██║   ██║██╔══██╗╚════██║
// ╚██████╔╝███████╗██║ ╚████║███████╗██║  ██║██║  ██║   ██║   ╚██████╔╝██║  ██║███████║
//  ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝

	public float[,] makeMap(int mapSize, float scale, float heightScale, float xOffset, float yOffset){
		float[,]temp = new float[mapSize,mapSize];

		for (int i = 0; i < terrain.GetLength(0); i++) 
		{
			for (int c = 0; c < terrain.GetLength(1); c++) 
			{
				float xCoord = ((xOffset + (float)c) / terrain.GetLength(0)) * scale;
				float yCoord = ((yOffset + (float)i) / terrain.GetLength(1)) * scale;
				temp[c,i] = (Mathf.PerlinNoise(xCoord, yCoord) * heightScale);
			}
		}
		return temp;
	}

	public float[,] normalizeMap(float[,] currentMap){
		float lowestPoint = 1000;
		float highestPoint = 0;

		//find highest and lowest point
		for (int i = 0; i < currentMap.GetLength(0); i++) 
		{
			for (int c = 0; c < currentMap.GetLength(1); c++) 
			{
				if(currentMap[c,i] < lowestPoint){
					lowestPoint = currentMap[c,i];
				}

				if(currentMap[c,i] > lowestPoint){
					highestPoint = currentMap[c,i];
				}
			}
		}

		//normalize map
		for (int i = 0; i < currentMap.GetLength(0); i++) 
		{
			for (int c = 0; c < currentMap.GetLength(1); c++) 
			{
				currentMap[c,i] = 1-Mathf.InverseLerp(lowestPoint, highestPoint, currentMap[c,i]);
			}
		}
		return currentMap;
	}

	public float[,] taperMapAtEdges(float[,] currentMap){
		//Values coming in will be 0-1 with 1 being the highest value
		//
		for (int i = 0; i < terrain.GetLength(0); i++) 
		{
			for (int c = 0; c < terrain.GetLength(1); c++) 
			{
				//Make Terrain
				float widthDistance = ((mapSize/2) - c); // 100-10=90 100-11=89 100-90 = 10
				float heightDistance = ((mapSize/2) - i);

				float distanceFromCenter = Mathf.Sqrt(Mathf.Pow(widthDistance, 2f) + Mathf.Pow(heightDistance, 2f))/13;
				currentMap[c,i] = currentMap[c,i]*2 + Mathf.Pow(distanceFromCenter, 2f);
			}
		}
		return currentMap;
	}


// ████████╗███████╗██╗  ██╗████████╗██╗   ██╗██████╗ ███████╗    ███████╗████████╗██╗   ██╗███████╗███████╗
// ╚══██╔══╝██╔════╝╚██╗██╔╝╚══██╔══╝██║   ██║██╔══██╗██╔════╝    ██╔════╝╚══██╔══╝██║   ██║██╔════╝██╔════╝
//    ██║   █████╗   ╚███╔╝    ██║   ██║   ██║██████╔╝█████╗      ███████╗   ██║   ██║   ██║█████╗  █████╗  
//    ██║   ██╔══╝   ██╔██╗    ██║   ██║   ██║██╔══██╗██╔══╝      ╚════██║   ██║   ██║   ██║██╔══╝  ██╔══╝  
//    ██║   ███████╗██╔╝ ██╗   ██║   ╚██████╔╝██║  ██║███████╗    ███████║   ██║   ╚██████╔╝██║     ██║     
//    ╚═╝   ╚══════╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚══════╝   ╚═╝    ╚═════╝ ╚═╝     ╚═╝     
																										 
	//This should be deletable when we finish the game.
	public Texture2D createTextureFromMap(float[,] currentMap, HeightColor[] heightColors){
		Texture2D noiseTex = new Texture2D(currentMap.GetLength(0), currentMap.GetLength(1));
		Color[] colorArray = new Color[currentMap.GetLength(0)*currentMap.GetLength(1)];
		int offset = 0;
		for (int i = 0; i < currentMap.GetLength(0); i++) 
		{
			for (int c = 0; c < currentMap.GetLength(1); c++) 
			{
				foreach (HeightColor hc in heightColors) 
				{
					if(currentMap[c,i] <= hc.terrainHeight){
						///Debug.Log(hc.terrainColor);
						colorArray[offset] = hc.terrainColor;
						break;
					}
				}
				offset++;
			}
		}
		noiseTex.SetPixels(colorArray);
		noiseTex.Apply();
		return noiseTex;
	}


// ████████╗██╗██╗     ███████╗     ██████╗ ███████╗███╗   ██╗███████╗██████╗  █████╗ ████████╗ ██████╗ ██████╗ 
// ╚══██╔══╝██║██║     ██╔════╝    ██╔════╝ ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗
//    ██║   ██║██║     █████╗      ██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██████╔╝███████║   ██║   ██║   ██║██████╔╝
//    ██║   ██║██║     ██╔══╝      ██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██╔══██║   ██║   ██║   ██║██╔══██╗
//    ██║   ██║███████╗███████╗    ╚██████╔╝███████╗██║ ╚████║███████╗██║  ██║██║  ██║   ██║   ╚██████╔╝██║  ██║
//    ╚═╝   ╚═╝╚══════╝╚══════╝     ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝
//    
	public GroundTile[,] makeTilesFromTerrain(float[,] terrain, HeightColor[] heightColors){
		GroundTile[,] terrainTiles = new GroundTile[terrain.GetLength(0),terrain.GetLength(1)];
		for (int i = 0; i < terrain.GetLength(0); i++) 
		{
			for (int c = 0; c < terrain.GetLength(1); c++) 
			{	
				foreach (HeightColor hc in heightColors) 
				{
					if(terrain[c,i] <= hc.terrainHeight){
						///Debug.Log(hc.terrainColor);
						terrainTiles[c,i].tileID = hc.tileID;
						break;
					} else {
						terrainTiles[c,i].tileID = 0;
					}
				}
			}
		}
		return terrainTiles;
	}

	public TreeTile[,] makeTreesFromMaps(GroundTile[,] groundTiles, float[,] temprature, float[,] precip){
		TreeTile[,] trees = new TreeTile[groundTiles.GetLength(0), groundTiles.GetLength(1)];

		for (int i = 0; i < groundTiles.GetLength(0); i++) 
		{
			for (int c = 0; c < groundTiles.GetLength(1); c++) 
			{
				if(groundTiles[c,i].tileID == 3){
					//if it's a 3 we know it's a forest tile.
					if(Random.Range(0.0f, 1.0f) >= 0.8f){
						if(temprature[c,i] > 0.9f && precip[c,i] > 0.5f){
							trees[c,i].tileID = 3; //ice tree
						} else if(temprature[c,i] > 0.4f && precip[c,i] > 0.7f){
							trees[c,i].tileID = 2; //swamp tree
						} else {
							trees[c,i].tileID = 1; //regular tree
						}
					} else {
						trees[c,i].tileID = 0; //No Tree
					}
				}
			}
		}

		return trees;
	}

	public BuildingTile[,] makeBuildingsFromMaps(GroundTile[,] groundTiles, float[,] urban, HeightColor[] urbanColors, float[,] village, HeightColor[] villageColors ){
		BuildingTile[,] buildings = new BuildingTile[groundTiles.GetLength(0), groundTiles.GetLength(1)];
		int buildingCount = 0;
		for (int i = 0; i < groundTiles.GetLength(0); i++) 
		{
			for (int c = 0; c < groundTiles.GetLength(1); c++) 
			{	
				foreach (HeightColor hc in urbanColors) 
				{
					if(urban[c,i] <= hc.terrainHeight){
						///Debug.Log(hc.terrainColor);
						if(hc.tileID != 0){
							buildingCount++;
							buildings[c,i].tileID = hc.tileID;
						} else {
							buildings[c,i].tileID = 0;
						}
						break;
					} else {
						buildings[c,i].tileID = 0;
					}
				}

				foreach (HeightColor hc in villageColors) 
				{
					if(village[c,i] <= hc.terrainHeight){
						if(hc.tileID == 1){
							if(Random.Range(0.0f, 1.0f) >= 0.8f){
								buildings[c,i].tileID = 1;
							}
						} else if(hc.tileID == 2){
							if(Random.Range(0.0f, 1.0f) >= 0.6f){
								buildings[c,i].tileID = 1;
							}
						} else {
							buildings[c,i].tileID = 0;
						}
						break;
					} else {
						buildings[c,i].tileID = 0;
					}
				}
			}
		}
		Debug.Log(buildingCount);

		return buildings;
	}


	public BuildingTile[,] makeDungeons(GroundTile[,] groundTiles, float[,] urban, HeightColor[] urbanColors){
		BuildingTile[,] buildings = new BuildingTile[groundTiles.GetLength(0), groundTiles.GetLength(1)];

		return buildings;
	}

}
