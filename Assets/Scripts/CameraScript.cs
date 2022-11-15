using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    private bool concentrateOnPlayer = true;
    private Vector3 currentVelocity;
    private GameObject objectToConcentrate;


    private void Start()
    {
        objectToConcentrate = playerPos.gameObject;
    }

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
            transform.position = Vector3.SmoothDamp(transform.position, objectToConcentrate.gameObject.transform.position, ref currentVelocity, 0.3f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        else
        {
            concentrateOnPlayer = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D box = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(box && box.tag == "Box")
                objectToConcentrate = box.gameObject;
        }
        if (Input.GetMouseButtonUp(0))
        {
            objectToConcentrate = playerPos.gameObject;
        }
    }
}
