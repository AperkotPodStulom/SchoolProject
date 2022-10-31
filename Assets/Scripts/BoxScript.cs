using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalThings;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private int boxHealth;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(boxHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        boxHealth -= 1;

        if (collision.gameObject.tag == "Player" && rb.velocity.magnitude >= 10)
            boxHealth -= boxHealth;
    }

    private void OnMouseDown()
    {
        Debug.Log(boxHealth);
    }
}
