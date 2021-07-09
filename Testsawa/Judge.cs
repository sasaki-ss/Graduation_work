using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    private bool hit = false;
    public bool Hit
    {
        get { return this.hit; }
        set { this.hit = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        // •¨‘Ì‚ªƒgƒŠƒK[‚ÉÚG‚µ‚Æ‚«A‚P“x‚¾‚¯ŒÄ‚Î‚ê‚é


    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.name == "Ball")
        {
            hit = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.name == "Ball")
        {
            hit = false;
        }
    }
}
