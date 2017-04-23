using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum MoveDirection{
	NORTH,
	SOUTH,
	EAST,
	WEST,
	TURN_LEFT,
	TURN_RIGHT
}

public enum TurnDirection{
	NORTH,
	SOUTH,
	EAST,
	WEST
}

public class Player : MonoBehaviour {
	public static Player self;
	public bool isMoving;
	public bool isTurning;
	public static TurnDirection currentDirection;
	public Vector3 targetPosition;
	public Quaternion targetRotation;
	public float damping;
	public float slerpTime;
	public float slerpSpeed = 2;
	public float slerpTimeRotation;
	public float playerHeightOffset = 1.5f;
	public float enemyHeightOffset = 0.5f;
	public int level = 1;

	public float damage = 1.0f;
	public float baseDamage = 1.0f;
	public float damageScalar = 1.3f;


	public float exp = 0.0f;
	public float experienceMin = 0.0f;
	public float experienceMax = 100.0f;
	public float experienceScalar = 1.3f;

	public GameObject ExperienceImage;
	public GameObject ExperienceText;
	public GameObject ExperienceBGImage;

	public float health = 100;
	public float healthMax = 100.0f;
	public float healthScalar = 1.3f;
	public float healthRegenSpeed = 0.3f;

	public GameObject HealthImage;
	public GameObject HealthText;
	// Use this for initialization
	void Start () {
		if(self == null){
			self = this;
		} else {
			Destroy(this.gameObject);
		}

		int mapWidth = WorldGenerator.groundTiles.GetLength(0);
		int mapHeight = WorldGenerator.groundTiles.GetLength(1);
		int randX = Random.Range(0, mapWidth);
		int randY = Random.Range(0, mapHeight);
		this.transform.position = new Vector3((int)randX, (WorldGenerator.terrain[(int)randX, (int)randY]*GameManager.HeightScale)+playerHeightOffset, (int)randY);
		int count = 0;
		while(count <= 10000 && !WorldMap.testForWalkable(this.transform.position)){
			randX = Random.Range(0, mapWidth);
			randY = Random.Range(0, mapHeight);
			this.transform.position = new Vector3(randX, (WorldGenerator.terrain[(int)randX, (int)randY]*GameManager.HeightScale)+playerHeightOffset, randY);
			count++;
		}
		if(count >= 10000){
			Debug.Log("Couldn't find a landing pad");
		}
		targetPosition = this.transform.position;
		targetRotation = this.transform.rotation;

		self.level = 1;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(WorldGenerator.groundTiles[(int)this.transform.position.x, (int)this.transform.position.z].tileID);
		if(isMoving){
			this.transform.position = Vector3.Slerp(this.transform.position, targetPosition, slerpTime);
			slerpTime += Time.deltaTime * slerpSpeed;
			if(slerpTime >= 1f){
				isMoving = false;
			}
		}
		if(isTurning){
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, slerpTimeRotation);
			slerpTimeRotation += Time.deltaTime;
			if(slerpTimeRotation >= 1f){
				isTurning = false;
			}
		}
		damage = baseDamage + (self.level * damageScalar);
		
		if(exp >= experienceMax){
			experienceMin = experienceMax;
			experienceMax = experienceMax + (experienceMax * experienceScalar);
			level += 1;

			healthMax = healthMax + (healthMax * healthScalar);
			health = healthMax;
		}

		if(health < healthMax){
			health += Time.deltaTime * healthRegenSpeed;
		}

		float HealthBarWidth = ExperienceBGImage.GetComponent<Image>().rectTransform.rect.width * 0.8f;
		HealthImage.GetComponent<Image>().rectTransform.offsetMin = new Vector2(0,0);
		HealthImage.GetComponent<Image>().rectTransform.offsetMax = new Vector2((((health)/(healthMax))*HealthBarWidth)-HealthBarWidth,0);
		HealthText.GetComponent<Text>().text = "Health  " + (int)health + "/" + (int)healthMax;
		
		float ExperienceBarWidth = ExperienceBGImage.GetComponent<Image>().rectTransform.rect.width * 0.8f;
		ExperienceImage.GetComponent<Image>().rectTransform.offsetMin = new Vector2(0,0);
		ExperienceImage.GetComponent<Image>().rectTransform.offsetMax = new Vector2((((exp-experienceMin)/(experienceMax-experienceMin))*ExperienceBarWidth)-ExperienceBarWidth,0);
		ExperienceText.GetComponent<Text>().text = "Experience " + (int)exp + "/" + (int)experienceMax;
	}

	public void MovePlayer(MoveDirection dir){
		if(!EnemySpawner.playerIsInEncounter){
			switch (dir) 
			{
				case MoveDirection.NORTH:
				case MoveDirection.SOUTH:
				case MoveDirection.EAST:
				case MoveDirection.WEST:
					slerpTime = 0;
					isMoving = true;
					targetPosition = GetNewTarget(targetPosition, currentDirection, dir);
				break;
				case MoveDirection.TURN_LEFT:
					isTurning = true;
					TurnPlayer(false);
					slerpTimeRotation = 0;
				break;
				case MoveDirection.TURN_RIGHT:
					isTurning = true;
					TurnPlayer(true);
					slerpTimeRotation = 0;
				break;
			}
		}
	}

	public Vector3 GetNewTarget(Vector3 pos, TurnDirection turnDir, MoveDirection moveDir){
		Vector3 newPos = pos;
		switch (turnDir) 
		{
			case TurnDirection.NORTH:
				switch (moveDir) 
				{
					case MoveDirection.NORTH:
						newPos += new Vector3(0,0,1);
					break;
					case MoveDirection.SOUTH:
						newPos += new Vector3(0,0,-1);
					break;
					case MoveDirection.EAST:
						newPos += new Vector3(1,0,0);
					break;
					case MoveDirection.WEST:
						newPos += new Vector3(-1,0,0);
					break;
				}
			break;
			case TurnDirection.SOUTH:
				switch (moveDir) 
				{
					case MoveDirection.NORTH:
						newPos += new Vector3(0,0,-1);
					break;
					case MoveDirection.SOUTH:
						newPos += new Vector3(0,0,1);
					break;
					case MoveDirection.EAST:
						newPos += new Vector3(-1,0,0);
					break;
					case MoveDirection.WEST:
						newPos += new Vector3(1,0,0);
					break;
				}
			break;
			case TurnDirection.EAST:
				switch (moveDir) 
				{
					case MoveDirection.NORTH:
						newPos += new Vector3(1,0,0);
					break;
					case MoveDirection.SOUTH:
						newPos += new Vector3(-1,0,0);
					break;
					case MoveDirection.EAST:
						newPos += new Vector3(0,0,-1);
					break;
					case MoveDirection.WEST:
						newPos += new Vector3(0,0,1);
					break;
				}
			break;
			case TurnDirection.WEST:
				switch (moveDir) 
				{
					case MoveDirection.NORTH:
						newPos += new Vector3(-1,0,0);
					break;
					case MoveDirection.SOUTH:
						newPos += new Vector3(1,0,0);
					break;
					case MoveDirection.EAST:
						newPos += new Vector3(0,0,1);
					break;
					case MoveDirection.WEST:
						newPos += new Vector3(0,0,-1);
					break;
				}
			break;
		}

		float height = WorldGenerator.terrain[(int)newPos.x, (int)newPos.z];
		if(WorldMap.testForWalkable(newPos)){
			newPos.y = (height * GameManager.HeightScale)+enemyHeightOffset;
			EnemySpawner.TestEncounter(newPos + this.transform.forward);
			newPos.y = (height * GameManager.HeightScale)+playerHeightOffset;
			return newPos;
		} else {
			return pos;
		}
	}

	public void TurnPlayer(bool turnRight){
		if(turnRight){
			switch (currentDirection) 
			{
				case TurnDirection.NORTH:
					currentDirection = TurnDirection.EAST;
				break;
				case TurnDirection.SOUTH:
					currentDirection = TurnDirection.WEST;
				break;
				case TurnDirection.EAST:
					currentDirection = TurnDirection.SOUTH;
				break;
				case TurnDirection.WEST:
					currentDirection = TurnDirection.NORTH;
				break;
			}
			targetRotation *= Quaternion.Euler(0, 90, 0);
		} else{
			switch (currentDirection) 
			{
				case TurnDirection.NORTH:
					currentDirection = TurnDirection.WEST;
				break;
				case TurnDirection.SOUTH:
					currentDirection = TurnDirection.EAST;
				break;
				case TurnDirection.EAST:
					currentDirection = TurnDirection.NORTH;
				break;
				case TurnDirection.WEST:
					currentDirection = TurnDirection.SOUTH;
				break;
			}
			targetRotation *= Quaternion.Euler(0, -90, 0);
		}
	}
}
