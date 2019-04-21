using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : CombatController, IDamageable
{
    SpriteRenderer currentSprite;

    [SerializeField]
    float HP = 30;
    [SerializeField]
    List<Sprite> states = new List<Sprite>();
    AudioSource audiosource;

    // Start is called before the first frame update
    void Awake()
    {
         currentSprite = GetComponent<SpriteRenderer>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else if(HP ==  20)
        {
            currentSprite.sprite = states[1];
        }
        else if(HP == 10)
        {
            currentSprite.sprite = states[2];
        }
    }

    public void TakeDamage(float damage)
    {
        HP = HP - damage;
        audiosource.Play();

    }
}
