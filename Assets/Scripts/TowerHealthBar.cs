﻿using UnityEngine;
using System.Collections;

public class TowerHealthBar : MonoBehaviour {

	public int maxHealth = 20 ;
	public int curHealth = 20 ;
	public Texture2D HBImage ;
	public Texture2D HBbackground ;
	public Material mat ;
	public float health = 100 ;
	public float healthy = 1;
	public float x = 0;
	public float y = 0;
	public float w = 50;
	public float h = Screen.width/3;
	public float healthLength ;
	public Rect box ;
	public Rect boxback ;

	public void ChangeHealth( int n )
	{
		curHealth += n;
		if (curHealth < 0)
			curHealth = 0 ;
		if (curHealth > maxHealth)
			curHealth = maxHealth ;

		healthLength = (272) * (curHealth / (float)maxHealth);

	}
	// Use this for initialization
	void Start ()
	{
		health = 100;
		healthLength =300;
		//mat.SetFloat("_Cutoff", (float)0.1) ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		healthy = 1 - (GameObject.Find("Main Camera").GetComponent<TowerHealthBar>().health/100) ;
		if( healthy == 0 )
			healthy = (float)0.04 ;
		mat.SetFloat("_Cutoff", healthy) ;
		ChangeHealth (0);
	}

	void OnGUI()
	{
		if (Event.current.type.Equals (EventType.Repaint)) {
			boxback = new Rect(x,y*(float)1.15 +5,75*(float)1.15,Screen.height/2);
			Graphics.DrawTexture(boxback, HBbackground );
			box = new Rect(x+5,y,75,Screen.height/2);
			Graphics.DrawTexture(box, HBImage, mat );
		}
		UnityEngine.GUI.Box (new Rect(10,Screen.height/2 + 20, Screen.width/20, 20), curHealth + "/" + maxHealth);
		UnityEngine.GUI.Box (new Rect (550, 10, 150, 20), 
		                     "Resources: " + GameObject.FindGameObjectWithTag("TheTower").GetComponent<TowerStats> ().mResources);
	}
}
