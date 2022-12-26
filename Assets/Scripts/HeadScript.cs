using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadScript : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private float ignoreTime;
    [SerializeField] private TMP_Text text;

    private float baseIgnoreTime;
    private bool ignoreDamage;

    private void Start()
    {
        baseIgnoreTime = ignoreTime;
        text.GetComponent<TMP_Text>().text = hp.ToString();
    }

    private void Update()
    {
        if (ignoreDamage)
        {
            ignoreTime -= Time.deltaTime;
            ignoreDamage = ignoreTime <= 0 ? false : true;
            ignoreTime = ignoreTime <= 0 ? ignoreTime = baseIgnoreTime : ignoreTime = ignoreTime;
        }

        if (hp <= 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Box" && col.gameObject.GetComponent<BoxScript>().State == BoxState.dontHaveSpell && !ignoreDamage)
        {
            ignoreDamage = true;
            hp--;
            text.GetComponent<TMP_Text>().text = hp.ToString();
        }
    }
}
