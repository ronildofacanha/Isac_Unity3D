using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float valorImpacto = 1f;
    public float valorVida = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude >= valorImpacto)
        {
            valorVida -= 1;
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            valorVida -= 1;
        }

        if (valorVida <= 0)
        {
            Invoke("Morreu", 1f);
        }
    }
    void Morreu()
    {
        Destroy(this.gameObject);
    }
}
