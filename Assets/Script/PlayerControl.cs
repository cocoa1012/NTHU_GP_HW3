using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float mX;
    public float speed, jumpForce;
    Rigidbody2D m_rigid;
    GameObject m_footPoint;
    public LayerMask landLayer;

    Vector2 curVelocity;

    bool onGround;
    // Use this for initialization
    void Start()
    {
        jumpForce = 500.0f;
        onGround = true;
        speed = 7.0f;
        m_rigid = GetComponent<Rigidbody2D>();
        m_footPoint = GameObject.Find("FootPoint");
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(m_footPoint.transform.position, 0.1f, landLayer);
    }

    // Update is called once per frame
    void Update()
    {
        mX = Input.GetAxis("Horizontal");
        if (onGround)
        {
            curVelocity = m_rigid.velocity;
            curVelocity.x = Mathf.Clamp(curVelocity.x + mX, -speed, speed);
            // if (mX > 0)
            // {
            //     curVelocity.x = speed;
            // }
            // else if (mX == 0)
            // {
            //     curVelocity.x = curVelocity.x;
            // }
            // else
            // {
            //     curVelocity.x = -speed;
            // }
            // curVelocity.x = (mX > 0) ? speed : -speed;
            m_rigid.velocity = curVelocity;
        }
        // 	m_rigid.velocity += new Vector2(mX, 0);
        // m_rigid.velocity = Vector2.ClampMagnitude(m_rigid.velocity, speed);
        // transform.Translate(mX * speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(onGround);
            if (onGround)
            {
                m_rigid.AddForce(new Vector2(0.0f, jumpForce));
                // setOnGround(false);
            }
        }
    }

    public void setOnGround(bool b)
    {
        onGround = b;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Land"))
        // {
        //     setOnGround(true);
        // }
    }

}
