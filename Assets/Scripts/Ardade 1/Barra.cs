using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barra : MonoBehaviour
{
    public Main MainScript;
    float escalaX;
    float escalaY;
    float escalaXInicial;
    // Start is called before the first frame update
    void Start()
    {
        escalaX = this.transform.localScale.x;
        escalaY = this.transform.localScale.y;
        escalaXInicial = this.transform.localScale.x;
   }

    // Update is called once per frame
    void Update()
    {
        //print(escalaX);
        if (MainScript.comecou)
        {
            escalaX = (escalaX - 0.5f * Time.deltaTime);
            this.transform.localScale = new Vector2(escalaX, escalaY);

            if (escalaX <= 0)
            {
                MainScript.FimDeJogo();
            }
        }
    }

    public void MaisTempo()
    {
        if (escalaX >= escalaXInicial)
        {
            escalaX = escalaXInicial;
        }
        else
        {
            escalaX = (escalaX + 0.75f * Time.deltaTime);
            transform.localScale = new Vector2(escalaX, escalaY);


        }
    }
}
