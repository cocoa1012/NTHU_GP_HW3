using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    Rigidbody2D m_rigid;
	Animator m_anim;
    public GameObject SnowBall, SummonFX;
	public AudioClip spellSFX, compeleteSFX, hitFX;

	AudioSource m_audio;

    GameObject player;

    bool facingRight = true;

    public float movingSpeed = 5.0f, changeSpeedTime = 3.0f, lastChangeTime;

    float mX = -1.5f;

    public int HP = 10;

    float attackX, attackY;
    Vector2 bulletDirection;

	public Slider hpBar;
	public Canvas Success;

    // Use this for initialization
    void Start()
    {
		Success.enabled = false;
		HP = 10;
		hpBar.value = HP;
        lastChangeTime = Time.time;
        player = GameObject.Find("player");
        m_rigid = gameObject.GetComponent<Rigidbody2D>();
		m_anim = gameObject.GetComponent<Animator>();
		m_audio = player.GetComponent<AudioSource>();
		InvokeRepeating("attack", 2.0f, 4.0f);
    }

    private void FixedUpdate()
    {
        if (Time.time > lastChangeTime + changeSpeedTime)
        {
            lastChangeTime = Time.time;
            mX = Random.Range(-movingSpeed, movingSpeed);
        }
        Move(mX);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < transform.position.x && facingRight || player.transform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    public void Move(float s)
    {
        //left / right moving actived only when the character is on the ground or air control is premitted


        //move the character
        //only change its velocity on x axis 
		if (s != 0){
			m_anim.SetTrigger("walk");
		}else{
			m_anim.SetTrigger("idle");
		}
        GetComponent<Rigidbody2D>().velocity = new Vector2(s, GetComponent<Rigidbody2D>().velocity.y);

        //flip the character image if player input direction is different with character's facing direction
        // if (s > 0 && !facingRight || s < 0 && facingRight) Flip();

    }

    void Flip()
    {
        //reverse the direction
        facingRight = !facingRight;

        //flip the character by multiplying x local scale with -1
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }

	public void GetDMG(){
		m_audio.PlayOneShot(hitFX);
		print ("DMG");
		HP -- ;
		hpBar.value = HP;
		if (HP < 1){
			m_audio.Stop();
			m_audio.PlayOneShot(compeleteSFX);
			Success.enabled = true;
			Time.timeScale = 0;
			Destroy(this.gameObject);
		}
	}
    void attack()
    {
		m_anim.SetTrigger("idle");
        m_anim.SetTrigger("attack");
        Invoke("lockOnAttack", 0.5f);
        // Invoke("lockOnAttack", 0.7f);
        Invoke("lockOnAttack", 0.9f);
        // Invoke("lockOnAttack", 1.1f);
        Invoke("lockOnAttack", 1.3f);

    }
    void lockOnAttack()
    {
        attackX = transform.position.x + Random.Range(-2f, 2f);
        attackY = transform.position.y + Random.Range(-2f, 2f);
        // attackX = Mathf.Clamp(attackX, genMinX, genMaxX);
        // attackY = Mathf.Clamp(attackY, genMinY, genMaxY);
        Instantiate(SummonFX, new Vector3(attackX + 0.05f, attackY + 1.4f, 0), Quaternion.Euler(Vector3.zero));
		m_audio.PlayOneShot(spellSFX,0.6f);
        GameObject atk = Instantiate(SnowBall, new Vector3(attackX, attackY, 0), Quaternion.Euler(Vector3.zero));
        bulletDirection = player.transform.position - atk.transform.position;
        atk.GetComponent<Rigidbody2D>().AddForce(bulletDirection.normalized * 400.0f);
    }
}
