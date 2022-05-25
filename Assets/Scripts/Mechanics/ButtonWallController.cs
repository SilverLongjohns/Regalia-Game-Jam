using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWallController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openWall()
    {
        Debug.Log("Wall opened");
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void closeWall()
    {
        Debug.Log("Wall closed");
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
}
