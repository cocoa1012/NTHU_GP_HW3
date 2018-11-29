using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeloton : MonoBehaviour
{

    // Use this for initialization
    public GameObject player, exitFx;
    public float m_speed;
    bool facingRight;

    GenMonster GM;
    void Start()
    {
        facingRight = true;
        m_speed = 7.0f;
        player = GameObject.Find("player");
        GM = GameObject.Find("MonsterGen").GetComponent<GenMonster>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // if (player != null)
        // {
        //     if (player.transform.position.x < transform.position.x)
        //     {
        Move(-m_speed);
        // }
        //     else if (player.transform.position.x > transform.position.x)
        //     {
        //         Move(m_speed);
        //     }
        //     else
        //     {
        //         Move(0);
        //     }
        // }

    }

    public void Move(float movingSpeed)
    {
        //left / right moving actived only when the character is on the ground or air control is premitted


        //move the character
        //only change its velocity on x axis 
        GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed, GetComponent<Rigidbody2D>().velocity.y);

        //flip the character image if player input direction is different with character's facing direction
        if (movingSpeed > 0 && !facingRight || movingSpeed < 0 && facingRight) Flip();

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "left")
        {
            Instantiate(exitFx, new Vector3(transform.position.x + 0.05f, transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
            Destroy(this.gameObject);
            GM.currNum --;
        }
    }
}
