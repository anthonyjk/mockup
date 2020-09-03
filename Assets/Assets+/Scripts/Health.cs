using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int dmg;
    public Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            collider.enabled = true;
            StartCoroutine(coolDown());
        }
    }

    IEnumerator coolDown()
    {
        yield return new WaitForSeconds(.25f);
        collider.enabled = false;
        yield return new WaitForSeconds(.25f);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            health -= dmg;
        }
    }
}
