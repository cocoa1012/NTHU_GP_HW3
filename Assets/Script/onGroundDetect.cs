using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onGroundDetect : MonoBehaviour {

	// Use this for initialization
	public PlayerControl player;
	public LayerMask landLayer;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == landLayer){
			print("Landing");
			player.setOnGround(true);
		}
	}
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == landLayer){
			print("Landing");
			player.setOnGround(true);
		}
	}
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == landLayer){
			print("Landing");
			player.setOnGround(true);
		}
	}
}
