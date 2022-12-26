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
    private GameObject smth = null;


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

    private void Update()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        foreach (Collider2D coll in colls)
        {
            smth = coll.gameObject.tag == "StopBox" ? coll.gameObject : null;
        }

        if (smth)
        {
            if (bs == BoxState.dontHaveSpell)
            {
                Physics2D.IgnoreCollision(smth.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            }
            else
            {
                Physics2D.IgnoreCollision(smth.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>(), false);
            }
        }
    }
}
