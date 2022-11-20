using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private GameObject platformHelper;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            platformHelper.transform.position = new Vector3(transform.position.x, transform.position.y - 1f);
        }
    }

    private void OnMouseDown()
    {
        platformHelper.transform.position = new Vector3(1000, 1000);
    }
}
