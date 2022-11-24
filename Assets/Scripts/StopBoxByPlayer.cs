using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum SpellState
{
    active = 1,
    notActive = 2
}
public class StopBoxByPlayer : MonoBehaviour
{
    [SerializeField] private float time;

    private SpellState ss = (SpellState)1;

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            ss = SpellState.active;
        }

        StartCoroutine("StartTimer");
    }

    private IEnumerable StartTimer()
    {
        if(time > 0 && ss == SpellState.active)
        {
            time -= Time.deltaTime;
        }

        if(time < 0)
        {
            ss = SpellState.notActive;
        }

        yield return null;
    }
}
