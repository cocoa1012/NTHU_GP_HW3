using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float destroyTime = 5;
	public ParticleSystem fx;
	ParticleSystem ps;
	public bool usingPS = false, usingFade = false;
	public float fadeTime = 0;
	float createTime;
	Color32 old;
	float a;
	// Use this for initialization
	void Start () {
		createTime = Time.time;
		if(usingPS){
			ps = GetComponent<ParticleSystem>();
		}
		if(usingFade){
			old = GetComponent<SpriteRenderer>().color;
			a = old.a;
		}
		if(fadeTime > destroyTime)
			fadeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// if(CompareTag("Spell"))
		// 	if(Time.time > createTime+0.1f)
		// 		GetComponent<BoxCollider2D>().tag = "Untagged";
		if(ps != null){
			if(!ps.IsAlive())
				Destroy(gameObject);
		}
		else{
			if(Time.time > createTime + destroyTime){
				if(usingPS){
					GameObject.Instantiate(fx,transform.position,transform.rotation);
					fx.Play();
				}
				Destroy(gameObject);
			}
			else if(Time.time > createTime + destroyTime - fadeTime) {
				if(usingFade){
					
					a -= (old.a/fadeTime)*Time.deltaTime;
					GetComponent<SpriteRenderer>().color = new Color32(old.r,old.g,old.b,(byte)a);
				}
			}
		}
	}
}
