﻿using UnityEngine;
using System.Collections;

public class MoveFlyingEnemy : MonoBehaviour {
	public Transform mTarget;
	public float mSpeed;
	private Vector3 mDirection;

	//public Transform mFlyingPan;
	// Use this for initialization
	void Start () {
		mSpeed = gameObject.GetComponent<EnemyStats> ().mSpeed;
		if(mTarget)
		{
			mDirection = mTarget.position - transform.position;
			mDirection = mDirection.normalized;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(mTarget)
		{
			Quaternion newRot = Quaternion.LookRotation (mTarget.position - transform.position, Vector3.up);
			newRot.y = 0;
			newRot.z = 0;
			mSpeed = gameObject.GetComponent<EnemyStats> ().mSpeed;
			transform.rotation = Quaternion.Lerp (transform.rotation, newRot, Time.deltaTime * mSpeed);
			transform.Translate(mDirection * mSpeed * Time.deltaTime);
		}
		if (gameObject.transform.position.x <= -100) {
			Destroy (gameObject);
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			if(gc)
			{
				SpawnWaves sw = gc.GetComponent<SpawnWaves>();
				sw.numEnemiesRemaining--;
			}
			Destroy (gameObject);
		}
	}
}
