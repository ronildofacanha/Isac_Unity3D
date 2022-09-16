using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject idle;
    public GameObject jump;
    public GameObject player;
    public int pontos = 0;
    public Text txtPontos;

    //Obstaculo
    public GameObject b1; // meio
    public GameObject b2; // direita
    public GameObject b3; // esquerda


    float escalaJogadorHorizontal;
    private List<GameObject> listObj;

    float posX, posY;
    bool direcaoPlayer = false;
    public bool comecou = false;
    public bool morreu = false;

    //Barra
    public Barra barraScript;

    void Start()
    {
        txtPontos.text = "Score: "+pontos.ToString();
        comecou = false;
        posX = b1.transform.position.x;
        posY = b1.transform.position.y;
        escalaJogadorHorizontal = player.transform.localScale.x;

        jump.SetActive(false);
        listObj = new List<GameObject>();
        criarBarriuStart();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && !morreu)
        {
            if (!comecou)
                comecou = true;

            if (Input.mousePosition.x > Screen.width / 2)
            {
                bDir();
                direcaoPlayer = true;
            }
            else
            {
                bEsq();
                direcaoPlayer = false;
            }

            listObj.RemoveAt(0);
            ReposBlocos();
            Jogada();

        }
    }

    void bDir()
    {
        jump.SetActive(true);
        idle.SetActive(false);
        player.transform.position = new Vector2(2.5f, player.transform.position.y);
        player.transform.localScale = new Vector2(-escalaJogadorHorizontal, player.transform.localScale.y);
        Invoke("volta", 0.25f);
        listObj[0].SendMessage("ActionDir");

    }
    void bEsq()
    {
        jump.SetActive(true);
        idle.SetActive(false);
        player.transform.position = new Vector2(-2.5f, player.transform.position.y);
        player.transform.localScale = new Vector2(escalaJogadorHorizontal, player.transform.localScale.y);
        Invoke("volta", 0.25f);
        listObj[0].SendMessage("ActionEsq");
    }

    void volta()
    {
        jump.SetActive(false);
        idle.SetActive(true);
    }

    GameObject newBarriu(Vector2 pos)
    {
        GameObject newB;

        if (Random.value > 0.5f || (listObj.Count < 2))
        {
            newB = Instantiate(b1);
        }
        else
        {
            if (Random.value > 0.5f)
            {
                newB = Instantiate(b2);
            }
            else
            {
                newB = Instantiate(b3);

            }
        }
        newB.transform.position = pos;

        return newB;

    }

    void criarBarriuStart()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = newBarriu(new Vector2(posX, posY + (i * 3.50f)));
            listObj.Add(obj);
        }

    }

    void ReposBlocos()
    {
        GameObject newobj = newBarriu(new Vector2(posX, posY + (3.50f) * 4));
        listObj.Add(newobj);

        for (int i = 0; i < 4; i++)
        {
            listObj[i].transform.position = new Vector2(listObj[i].transform.position.x, listObj[i].transform.position.y - 3.50f);
        }
    }

    void Jogada()
    {
        if (listObj[0].gameObject.CompareTag("gDir") && !direcaoPlayer)
        {
           // Debug.Log("ERROU DIR");
            FimDeJogo();
        }
        else if (listObj[0].gameObject.CompareTag("gEsc") && direcaoPlayer)
        {
           // Debug.Log("ERROU ESC");
            FimDeJogo();
        }
        else
        {
           // Debug.Log("PONTO");
            pontos += 1;
            txtPontos.text = "Score: " + pontos.ToString();
            barraScript.MaisTempo();
        }
    }
    public void FimDeJogo()
    {
        comecou = false;
        morreu = true;

        idle.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 1.0f);
        jump.GetComponent<SpriteRenderer>().color = new Color(1f, 0.25f, 0.25f, 1.0f);
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Rigidbody2D>().AddTorque(100f);

        if (direcaoPlayer)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 3);
        }
        else { player.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 3); }

        Invoke("RecarregaCena", 1);

    }

    void RecarregaCena()
    {

        SceneManager.LoadScene(0);
    }


}
