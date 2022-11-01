using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonScript : MonoBehaviour
{
    [SerializeField] private string point;
    [SerializeField] private Transform objectCreatePos;
    [SerializeField] private GameObject objectPref;

    private BoxCollider2D coll;
    private bool active = false;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //checking collision with player
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, coll.size, 0);
        foreach(Collider2D collision in colliders)
        {
            if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box")
            {
                if (!active)
                {
                    switch (point)
                    {
                        case "CreateObject":
                            Debug.Log("Spawned");
                            Instantiate(objectPref, objectCreatePos.position, Quaternion.identity);
                            break;
                    }
                    transform.position += new Vector3(0, -0.10f);
                    active = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Box")
        {
            if (active)
            {
                active = false;
                Debug.Log("Despawned");
                transform.position += new Vector3(0, 0.10f);
            }
        }
    }
}
