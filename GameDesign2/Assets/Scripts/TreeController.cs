using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : CombatController, IDamageable
{
    SpriteRenderer currentSprite;
    Item item;

    [SerializeField]
    float HP = 30;
    [SerializeField]
    List<Sprite> states = new List<Sprite>();
    [SerializeField]
    Item outputItem;
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

    }

    public void TakeDamage(float damage)
    {
        HP = HP - damage;
        audiosource.Play();
        if (HP <= 0)
        {
            Destroy(gameObject);
            Item instance = Instantiate(outputItem, new Vector3(this.transform.position.x, this.transform.position.y, -0.1f), Quaternion.identity);
        }
        else if (HP == 20)
        {
            currentSprite.sprite = states[1];
            Item instance = Instantiate(outputItem, new Vector3(this.transform.position.x, this.transform.position.y, -0.1f), Quaternion.identity);


        }
        else if (HP == 10)
        {
            currentSprite.sprite = states[2];
            Item instance = Instantiate(outputItem, new Vector3(this.transform.position.x, this.transform.position.y, -0.1f), Quaternion.identity);
        }

    }
}
