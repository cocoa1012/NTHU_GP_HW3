using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenMonster : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] Gen;
    public GameObject GenFx;
    public int MaxNum = 5, currNum;
    void Start()
    {
        currNum = 0;
        InvokeRepeating("GenItem", 2.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenItem()
    {
        if (currNum < MaxNum)
        {
            Instantiate(GenFx, new Vector3(transform.position.x + 0.05f, transform.position.y + 1.6f, 0), Quaternion.Euler(Vector3.zero));
            Instantiate(Gen[0], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(Vector3.zero));
            currNum++;
        }
    }
}
;