﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperRockController : MonoBehaviour, IDamageable
{
    [SerializeField]
    float HP = 30;
    AudioSource audiosource;
    [SerializeField]
    Item outputItem;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
            Item instance = Instantiate(outputItem, new Vector3(this.transform.position.x, this.transform.position.y, -0.1f), Quaternion.identity);
        }
    }


    public void TakeDamage(float damage)
    {
        HP = HP - damage;
        audiosource.Play();
    }
}
