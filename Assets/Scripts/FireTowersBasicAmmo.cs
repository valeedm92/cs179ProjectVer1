﻿using UnityEngine;
using System.Collections;

public class FireTowersBasicAmmo : MonoBehaviour {
	private const float ACCEL_GRAVITY = -0.02f;

	public Vector3 dir;
	public float mAmmoSpeed;
	public float mAngle;
	public float radius = 0;
	public int typeOfAmmo = 0;
	private int enemyLayer = 0;
	private float mDamage;
	// Use this for initialization
	void Start () {
		mAmmoSpeed = 100;
		dir.y += Mathf.Sin(mAngle);
		dir = dir.normalized;
		enemyLayer = 1 << 10;
		if(typeOfAmmo > 0)
			mDamage = gameObject.GetComponent<DestroyOnContactGrenade> ().mDamage;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float xVelocity = dir.x * Time.deltaTime * mAmmoSpeed;
		float yVelocity = dir.y * Time.deltaTime * mAmmoSpeed;
		float zVelocity = dir.z * Time.deltaTime * mAmmoSpeed;

		Vector3 translateVector = new Vector3(xVelocity, yVelocity, zVelocity);
		transform.Translate (translateVector);
		dir.y += ACCEL_GRAVITY;
		if (gameObject) {
			float yPosition = transform.position.y;

			if(yPosition <= 0 && typeOfAmmo > 0)
			{
				Collider [] hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
				Debug.Log("fire: " + hitColliders.Length);
				for(int i = 0; i < hitColliders.Length; i++)
				{
					if(hitColliders[i].gameObject.tag == "Enemy")
					{
						float proximity = (transform.position - hitColliders[i].gameObject.transform.position).magnitude;

						float effect = (proximity/radius);
						Debug.Log ("Enemy at " + proximity + " away!" + " damage: " + effect);
						EnemyStats es = hitColliders[i].gameObject.GetComponent<EnemyStats>();
						if(es)
							es.mHealth -= mDamage * effect;
					}
				}
			}
			/** Remove when it passes the ground plane */
			if (yPosition < 0) {
				Destroy (gameObject);
			}
		}
	}
}
