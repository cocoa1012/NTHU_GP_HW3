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
    public AudioClip slash, hit, jumpSFX, saveSFX, successSFX, deadSFX, crashSFX;

    public GenMonster GM;
    public Image[] Life;
    public Canvas GameOver;
    public Text Hint;
    Vector2 curVelocity;

    public bool airControl = true, facingRight = true, jump, inATK, inAirATK;
    public bool atkDone = true;
    public int atkCount = 0;
    int curLife = 5;

    public float lastAttackTime, attackPeriod = 0.5f;
    public List<GameObject> inRangedMonster;

    Vector2 savePos;
    bool isSaved = false, goNextLevel = false, isReviving = false;

    Animator m_anim;

    bool onGround;
    // Use this for initialization
    void Start()
    {
        // Life = new Image[curLife];
        if (Hint != null){
            Hint.enabled = false;
        }
        GameOver.enabled = false;
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
        if (!isReviving) Move(mX, jump);
        jump = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (goNextLevel)
            {
                m_audio.PlayOneShot(successSFX);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level_2");
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isReviving)
            {
                m_audio.PlayOneShot(jumpSFX);
                print("JUMP KEY");
                jump = true;
            }
            else{
                isReviving = false;
                m_rigid.gravityScale = 1;
                Hint.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onGround)
            {
                inATK = true;

                // foreach (var item in inRangedMonster)
                // {
                //     Destroy(item);
                // }
                m_anim.SetBool("atkDone", false);
                lastAttackTime = Time.time;
                if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("IDLE") || m_anim.GetCurrentAnimatorStateInfo(0).IsName("run"))
                {
                    if (atkCount == 0)
                    {
                        m_audio.PlayOneShot(slash);
                        hitMonster();
                        m_anim.SetBool("atk1", true);
                        atkCount++;
                    }
                }
                else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("atk1"))
                {
                    if (atkCount == 1)
                    {
                        m_audio.PlayOneShot(slash);
                        hitMonster();
                        m_anim.SetBool("atk1", false);
                        m_anim.SetBool("atk2", true);
                        atkCount++;
                    }
                }
                else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("atk2"))
                {
                    if (atkCount == 2)
                    {
                        m_audio.PlayOneShot(slash);
                        hitMonster();
                        m_anim.SetBool("atk2", false);
                        m_anim.SetBool("atk3", true);
                        atkCount++;
                    }
                }
                else if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("atk3"))
                {
                    if (atkCount == 3)
                    {
                        m_audio.PlayOneShot(slash);
                        hitMonster();
                        m_anim.SetBool("atk3", false);
                        m_anim.SetBool("atk1", true);
                        atkCount = 1;
                    }
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

    void hitMonster()
    {
        while (inRangedMonster.Count > 0)
        {
            GameObject t = inRangedMonster[0];
            inRangedMonster.RemoveAt(0);
            Instantiate(summonFx, new Vector3(t.transform.position.x + 0.05f, t.transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
            Destroy(t);
            m_audio.PlayOneShot(hit);
            GM.currNum--;
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
        if (other.gameObject.tag == "Skeloton")
        {

            curLife--;
            print(curLife);
            if (isSaved)
            {
                transform.position = savePos;
            }
            Life[curLife].GetComponent<Image>().color = new Color32(0, 0, 0, 0);

            if (curLife < 1)
            {
                m_audio.PlayOneShot(deadSFX);
                GameOver.enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                m_audio.PlayOneShot(crashSFX);
                isReviving = true;
                Hint.enabled = true;
                m_rigid.gravityScale = 0;
                m_rigid.velocity = new Vector2(0, 0);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.tag);

        if (other.gameObject.tag == "Save")
        {
            if (isSaved != true)
            {
                m_audio.PlayOneShot(saveSFX);
                isSaved = true;
                GameObject s = GameObject.Find("save-file-option");
                savePos = new Vector2(s.transform.position.x, s.transform.position.y + 1.0f);
            }
        }

        if (other.gameObject.tag == "Next")
        {
            goNextLevel = true;
        }

        if (other.gameObject.tag == "Skeloton")
        {
            // if (inAirATK)
            // {
            //     // playHit();
            //     Destroy(other.gameObject);
            //     Instantiate(summonFx, new Vector3(other.transform.position.x + 0.05f, other.transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
            //     GM.currNum--;
            // }
            // else
            inRangedMonster.Add(other.gameObject);
            if (inAirATK)
            {
                hitMonster();
            }
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
        if (other.gameObject.tag == "Next")
        {
            goNextLevel = false;
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
