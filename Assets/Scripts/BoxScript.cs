using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxState
{
    haveSpell,
    dontHaveSpell
}

public class BoxScript : MonoBehaviour
{
    private BoxState bs = BoxState.dontHaveSpell;


    private void OnMouseDrag()
    {
        bs = BoxState.haveSpell;
    }

    private void OnMouseUp()
    {
        bs = BoxState.dontHaveSpell;
    }

    public BoxState State
    {
        get { return bs; }
    }

    private void FixedUpdate()
    {
        if(bs == BoxState.dontHaveSpell)
        {
            Physics2D.IgnoreCollision(GameObject.FindWithTag("StopBox").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }
        else
        {
            Physics2D.IgnoreCollision(GameObject.FindWithTag("StopBox").GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
        }
    }
}
