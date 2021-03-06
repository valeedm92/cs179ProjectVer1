﻿using UnityEngine;
using System.Collections;

public class NewSpawnWaves : MonoBehaviour {
	public GameObject[] enemy;
	public GameObject[] outEnemy;
	public GameObject enemyOut;
	public int spawnCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float waveDuration;
	public int waveNo;
	public int numEnemiesRemaining = 0;
	public int maxEnemyOnScreen = 0;
	private float waveTime = 30.0f;
	Vector3 startPoint = new Vector3(58.5f, 0.0f, -24.4f);
	public int waveCompleted;
	public int advanced;
	// Use this for initialization
	GameObject theGC;
	public float healthMultiplier;
	public float speedMultiplier;

	void Start () {
		waveDuration = waveTime;
		waveNo = 1;
		waveCompleted = 0;
		maxEnemyOnScreen = 40;
		numEnemiesRemaining = 0;
		theGC = GameObject.Find ("GameController");
		advanced = 0;
		healthMultiplier = 1.0f;
		speedMultiplier = 1.0f;
		StartCoroutine (SpawnEasyWaves(waveDuration,spawnCount,healthMultiplier,speedMultiplier));
	}
	
	// Update is called once per frame
	void Update () {
		waveDuration -= Time.deltaTime;
		if((waveDuration < 0.0f && numEnemiesRemaining == 0))
		{
			waveNo++;
			waveCompleted++;
			if(advanced > 0)
				waveCompleted += advanced;
			Debug.Log("NEW WAVE === wave number: " + waveNo);
			waveDuration = waveTime + 15;
			waveTime += 15;
			spawnCount+=1;
			if(waveNo % 4 == 0)
			{
				healthMultiplier += 0.5f;
				speedMultiplier += 0.1f;
			}
			StartCoroutine(SpawnEasyWaves(waveDuration,spawnCount,healthMultiplier,speedMultiplier));
			if(waveNo % 2 == 0)
				StartCoroutine(SpawnOutsideWavesTop(waveDuration-15,spawnCount/2,healthMultiplier,speedMultiplier));
			if(waveNo % 3 == 0)
				StartCoroutine(SpawnOutsideWavesBottom(waveDuration-15,spawnCount/2,healthMultiplier,speedMultiplier));

			advanced = 0;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			advanced++;
			waveNo++;

			Debug.Log("NEW WAVE === wave number: " + waveNo);
			waveDuration = waveTime + 15;
			waveTime += 15;
			spawnCount+=1;
			if(waveNo % 4 == 0)
			{
				healthMultiplier += 0.5f;
				speedMultiplier += 0.1f;
			}
			StartCoroutine(SpawnEasyWaves(waveDuration,spawnCount,healthMultiplier,speedMultiplier));
			if(waveNo % 2 == 0)
				StartCoroutine(SpawnOutsideWavesTop(waveDuration-15,spawnCount/2,healthMultiplier,speedMultiplier));
			if(waveNo % 3 == 0)
				StartCoroutine(SpawnOutsideWavesBottom(waveDuration-15,spawnCount/2,healthMultiplier,speedMultiplier));

		}

	}

	IEnumerator SpawnEasyWaves(float waveDur, int enemyCount, float hMult, float sMult)
	{
		Debug.Log ("EASY WAVE");
		yield return new WaitForSeconds (startWait);
		waveDur -= startWait;
		waveDur -= Time.deltaTime;
		int selectEnemy = Random.Range (0, enemy.Length);
		while(waveDur >= 0)
		{
			Debug.Log("WAVE ACTIVE: " + waveDur + "WAVE DURATION: " + waveDuration);
			yield return new WaitForSeconds(waveWait);
			waveDur -= waveWait;
			//waveDur -= Time.deltaTime;
			if(waveDur < 0)
				break;
			if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
				break;
			//waveDur -= Time.deltaTime;
			for(int i = 0; i < enemyCount; i++)
			{
				numEnemiesRemaining++;
				GameObject spawnedEnemy;
				if(selectEnemy < enemy.Length-1)
					spawnedEnemy = (GameObject)Instantiate(enemy[selectEnemy], startPoint,Quaternion.identity);
				else 
					spawnedEnemy = (GameObject)Instantiate(enemy[selectEnemy], new Vector3(Random.Range(85.0f,100.0f),Random.Range(0, 20),
					                                            Random.Range(-50,50)),Quaternion.identity);


				spawnedEnemy.GetComponent<EnemyStats>().mSpeed = sMult*spawnedEnemy.GetComponent<EnemyStats>().mSpeed;
				spawnedEnemy.GetComponent<EnemyStats>().mHealth = hMult*spawnedEnemy.GetComponent<EnemyStats>().mHealth;

				yield return new WaitForSeconds(spawnWait);
				waveDur -= spawnWait;
				waveDur -= Time.deltaTime;
				if(waveDur < 0)
					break;
				if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
					break;
			}
		}
		//waveCompleted++;
		Debug.Log ("Wave Done");
	}


	IEnumerator SpawnOutsideWavesTop(float waveDur, int enemyCount, float hMult, float sMult)
	{
		Debug.Log ("OUTSIDE WAVE TOP");
		yield return new WaitForSeconds (startWait);
		waveDur -= startWait;
		waveDur -= Time.deltaTime;
		while(waveDur >= 0)
		{
			yield return new WaitForSeconds(waveWait);
			waveDur -= waveWait;
			if(waveDur < 0)
				break;
			if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
				break;
			for(int i = 0; i < enemyCount; i++)
			{
				numEnemiesRemaining++;
				int selectEnemy = Random.Range (0, outEnemy.Length);
				GameObject eO;
				if(selectEnemy < outEnemy.Length-1)
					eO = (GameObject)Instantiate(outEnemy[selectEnemy], new Vector3(95.0f,0.0f,15.0f),Quaternion.identity);
				else 
					eO = (GameObject)Instantiate(outEnemy[selectEnemy], new Vector3(Random.Range(85.0f,100.0f),Random.Range(0, 20),
					                                            Random.Range(-50,50)),Quaternion.identity);
				eO.GetComponent<EnemyStats>().mSpeed = sMult*eO.GetComponent<EnemyStats>().mSpeed;
				eO.GetComponent<EnemyStats>().mHealth = hMult*eO.GetComponent<EnemyStats>().mHealth;

				if(selectEnemy < outEnemy.Length-1)
				{
					Buy_Shoot_Modes bsm = theGC.GetComponent<Buy_Shoot_Modes>();
					MoveUsingDFS mud = eO.GetComponent<MoveUsingDFS>();
					mud.wayPoints = bsm.thePath;
				}
				yield return new WaitForSeconds(spawnWait);
				waveDur -= spawnWait;
				waveDur -= Time.deltaTime;
				if(waveDur < 0)
					break;
				if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
					break;
			}
		}
		//waveCompleted++;
	}

	IEnumerator SpawnOutsideWavesBottom(float waveDur, int enemyCount, float hMult, float sMult)
	{
		Debug.Log ("OUTSIDE WAVE BOTTOM");
		yield return new WaitForSeconds (startWait);
		waveDur -= startWait;
		waveDur -= Time.deltaTime;
		while(waveDur >= 0)
		{
			yield return new WaitForSeconds(waveWait);
			waveDur -= waveWait;
			if(waveDur < 0)
				break;
			if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
				break;
			for(int i = 0; i < enemyCount; i++)
			{
				numEnemiesRemaining++;
				int selectEnemy = Random.Range (0, outEnemy.Length);
				GameObject eO;
				if(selectEnemy < outEnemy.Length-1)
					eO = (GameObject)Instantiate(outEnemy[selectEnemy], new Vector3(95.0f,0.0f,-45.0f),Quaternion.identity);
				else 
					eO = (GameObject)Instantiate(outEnemy[selectEnemy], new Vector3(Random.Range(85.0f,100.0f),Random.Range(0, 20),
					                                                                Random.Range(-50,50)),Quaternion.identity);
				eO.GetComponent<EnemyStats>().mSpeed = sMult*eO.GetComponent<EnemyStats>().mSpeed;
				eO.GetComponent<EnemyStats>().mHealth = hMult*eO.GetComponent<EnemyStats>().mHealth;


				if(selectEnemy < outEnemy.Length-1)
				{
					Buy_Shoot_Modes bsm = theGC.GetComponent<Buy_Shoot_Modes>();
					MoveUsingDFS mud = eO.GetComponent<MoveUsingDFS>();
					mud.wayPoints = bsm.thePath2;
				}
				yield return new WaitForSeconds(spawnWait);
				waveDur -= spawnWait;
				waveDur -= Time.deltaTime;
				if(waveDur < 0)
					break;
				if(gameObject.GetComponent<Buy_Shoot_Modes>().gameover)
					break;
			}
		}
		//waveCompleted++;
	}

}
