using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    private bool concentrateOnPlayer = true;
    private Vector3 currentVelocity;

    private void Update()
    {
        if (concentrateOnPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, playerPos.position, ref currentVelocity, 0.3f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }

        if (Input.GetMouseButton(0))
        {
            concentrateOnPlayer = false;
            Collider2D box = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (box && box.tag == "Box")
            {
                transform.position = Vector3.SmoothDamp(transform.position, box.gameObject.transform.position, ref currentVelocity, 0.3f);
                transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }
        }
        else
        {
            concentrateOnPlayer = true;
        }
    }
}
