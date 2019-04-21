using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopperRockController : MonoBehaviour, IDamageable
{
    [SerializeField]
    float HP = 30;
    AudioSource audiosource;
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
        }
    }

    public void TakeDamage(float damage)
    {
        HP = HP - damage;
        audiosource.Play();
    }
}
