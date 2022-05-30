using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject.transform.parent.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var kick = collision.gameObject;
        if (gameObject.transform.parent.tag == "IceBody" && collision.gameObject.transform.tag == "Kick")
        {
            Debug.Log("kicking Iceblock");
            if (collision.bounds.center.x < gameObject.GetComponent<BoxCollider2D>().bounds.center.x)
            {
                Debug.Log("Push Right");
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Push Left");
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 10, ForceMode2D.Impulse);
            }
        }
    }
}
