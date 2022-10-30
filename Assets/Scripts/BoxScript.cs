using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalThings;

public class BoxScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        velocity = rb.velocity;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if(GlobalThings.GlobalSpells.GlobalSpellState == Spells.Dragging)
        {
            Vector2.SmoothDamp(transform.position, mousePos, ref velocity, 0.1f);
        }
    }
}
