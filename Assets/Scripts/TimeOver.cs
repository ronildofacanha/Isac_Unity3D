using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOver : MonoBehaviour
{
    public Image barra;
    public Main MainScript;
    public float reTime = 0.05f;
    public float addTime = 0.7f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //print(escalaX);
        if (MainScript.comecou)
        {
            barra.fillAmount -= reTime * Time.deltaTime;

            if (barra.fillAmount <= 0)
            {
                MainScript.FimDeJogo();
            }
        }
    }
    public void MaisTempo()
    {
        if (barra.fillAmount >= 1)
        {
            barra.fillAmount = 1;
        }
        else if(barra.fillAmount < 1)
        {
            barra.fillAmount += addTime * Time.deltaTime;

        }
    }
}
