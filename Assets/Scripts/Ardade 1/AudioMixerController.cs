using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    float min = -80f;
    public Image barra;
    public Text textStyle;
    public float n1 = -80;
    public float n2 = -80;
    public float n3 = -80;
    public float n4 = -80;

    // Start is called before the first frame update
    void Awake()
    {
        audioMixer.SetFloat("lv1", n1);
        audioMixer.SetFloat("lv2", min);
        audioMixer.SetFloat("lv3", min);
        audioMixer.SetFloat("lv4", n2);
        audioMixer.SetFloat("lv5", n3);
        audioMixer.SetFloat("lv6", n4);
        textStyle.text = "";
        //audioSource.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        audioMixer.SetFloat("lv1", Mathf.Lerp(n1, 0, barra.fillAmount));
        audioMixer.SetFloat("lv4", Mathf.Lerp(n2, 0, barra.fillAmount));
        audioMixer.SetFloat("lv5", Mathf.Lerp(n3, 0, barra.fillAmount));
        audioMixer.SetFloat("lv6", Mathf.Lerp(n4, 0, barra.fillAmount));



        PontosDeEstilo();
    }

    void PontosDeEstilo()
    {

        //lv0
        if (barra.fillAmount <= 0.4)
        {
            //D
            textStyle.text = "D";
        }
        //lv1
        if (barra.fillAmount >= 0.4 && barra.fillAmount <= 0.54)
        {
            //C
            textStyle.text = "C";
        }
        //lv2
        if (barra.fillAmount >= 0.54 && barra.fillAmount <= 0.67)
        {
            //B
            textStyle.text = "B";
        }
        //lv3
        if (barra.fillAmount >= 0.67 && barra.fillAmount <= 0.76)
        {
            //A
            textStyle.text = "A";
        }
        //lv4
        if (barra.fillAmount >= 0.76 && barra.fillAmount <= 0.86)
        {
            //S
            textStyle.text = "S";
        }
        //lv5
        if (barra.fillAmount >= 0.86 && barra.fillAmount <= 0.91)
        {
            //Ss
            textStyle.text = "Ss";
        }
        //lv6
        if (barra.fillAmount > 0.91)
        {
            //Sss
            textStyle.text = "Sss";
        }
    }
}
