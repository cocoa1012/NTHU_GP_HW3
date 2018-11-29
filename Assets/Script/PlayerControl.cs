using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public float mX;
    public float speed, jumpForce, maxSpeed = 10.0f;
    Rigidbody2D m_rigid;
    public GameObject m_footPoint, summonFx;
    public LayerMask landLayer;

    public AudioSource m_audio;
    public AudioClip slash, hit;

    public GenMonster GM;
    public Slider HP;
    Vector2 curVelocity;

    public bool airControl = true, facingRight = true, jump, inATK, inAirATK;
    public bool atkDone = true;
    public int atkCount = 0;
    public int curHealth = 5;

    public float lastAttackTime, attackPeriod = 0.5f;
    public List<GameObject> inRangedMonster;

    Animator m_anim;

    bool onGround;
    // Use this for initialization
    void Start()
    {
        HP.value = curHealth;
        m_audio = GetComponent<AudioSource>();
        inATK = false;
        inAirATK = false;
        lastAttackTime = Time.time;
        jumpForce = 350.0f;
        onGround = true;
        jump = false;
        speed = 7.0f;
        m_rigid = GetComponent<Rigidbody2D>();
        // m_footPoint = GameObject.Find("FootPoint");
        m_anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Time.time > lastAttackTime + attackPeriod)
        {
            inATK = false;
            atkCount = 0;
            m_anim.SetBool("atk1", false);
            m_anim.SetBool("atk2", false);
            m_anim.SetBool("atk3", false);
            m_anim.SetBool("atkDone", true);
        }
        onGround = Physics2D.OverlapCircle(m_footPoint.transform.position, 0.1f, landLayer);
        print(onGround);
        mX = Input.GetAxis("Horizontal");
        Move(mX, jump);
        jump = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            print("JUMP KEY");
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onGround)
            {
                m_audio.PlayOneShot(slash);
                inATK = true;
                while (inRangedMonster.Count > 0)
                {
                    GameObject t = inRangedMonster[0];
                    inRangedMonster.RemoveAt(0);
                    Instantiate(summonFx, new Vector3(t.transform.position.x + 0.05f, t.transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
                    Destroy(t);
                    m_audio.PlayOneShot(hit);
                    GM.currNum--;
                }
                // foreach (var item in inRangedMonster)
                // {
                //     Destroy(item);
                // }
                m_anim.SetBool("atkDone", false);
                lastAttackTime = Time.time;
                if (atkCount == 0)
                {
                    m_anim.SetBool("atk1", true);
                    atkCount++;
                }
                else if (atkCount == 1)
                {
                    m_anim.SetBool("atk1", false);
                    m_anim.SetBool("atk2", true);
                    atkCount++;
                }
                else if (atkCount == 2)
                {
                    m_anim.SetBool("atk2", false);
                    m_anim.SetBool("atk3", true);
                    atkCount++;
                }
                else if (atkCount == 3)
                {
                    m_anim.SetBool("atk3", false);
                    m_anim.SetBool("atk1", true);
                    atkCount = 0;
                }
            }
            else
            {
                if (inAirATK == false)
                {
                    inATK = true;
                    inAirATK = true;
                    m_rigid.velocity = new Vector2(0, 0);
                    m_rigid.AddForce(new Vector2(0, -900.0f));
                    m_anim.SetBool("airATK", true);
                }
            }
        }
    }

    void playHit()
    {
        m_audio.PlayOneShot(hit);
    }

    public void setOnGround(bool b)
    {
        onGround = b;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Skeloton"){
            curHealth -- ;
            HP.value = curHealth;
            if(curHealth < 1){
                GameOver.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.tag);


        if (other.gameObject.tag == "Skeloton")
        {
            if (inAirATK)
            {
                // playHit();
                Destroy(other.gameObject);
                Instantiate(summonFx, new Vector3(other.transform.position.x + 0.05f, other.transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
                GM.currNum--;
            }
            else
                inRangedMonster.Add(other.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Skeloton")
        {
            if (inRangedMonster.Contains(other.gameObject))
            {
                inRangedMonster.Remove(other.gameObject);
            }
        }
    }

    public void Move(float movingSpeed, bool jump)
    {
        //left / right moving actived only when the character is on the ground or air control is premitted

        if (onGround || airControl && !inATK)
        {
            //change the character animation by moving speed
            m_anim.SetFloat("Speed", Mathf.Abs(movingSpeed));
            m_anim.SetFloat("SpeedY", GetComponent<Rigidbody2D>().velocity.y);

            //move the character
            //only change its velocity on x axis 
            GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

            //flip the character image if player input direction is different with character's facing direction
            if (movingSpeed > 0 && !facingRight || movingSpeed < 0 && facingRight) Flip();
        }

        if (onGround)
        {
            if (inAirATK) playHit();
            inAirATK = false;
            m_anim.SetBool("airATK", false);
            m_anim.SetBool("onGround", true);
            m_anim.SetBool("jump", false);
        }

        //let character jump when it's on the ground and player hits jump button
        if (onGround && jump && !inATK)
        {
            m_anim.SetBool("onGround", false);
            m_anim.SetBool("jump", true);
            // m_anim.SetTrigger("jump");
            print("JUMP");
            //make character jump by adding force
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, jumpForce));
        }
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

}
