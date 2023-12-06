using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public Rigidbody2D player;
    SpringJoint2D mola;
    public float velocidadeParado = 0.5f;
    public float tempoParado = 10f;

    private void Awake()
    {
        mola = player.GetComponent<SpringJoint2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if(player.velocity.sqrMagnitude <= velocidadeParado && mola==null)
        {
            Invoke("Reset", tempoParado);
        }
    }

    void Reset()
    {
     //   SceneManager.LoadScene(SceneManager.GetActiveScene().name);
          SceneManager.LoadScene(2);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2D>() == player)
        {
            Reset();
        }
    }
}
