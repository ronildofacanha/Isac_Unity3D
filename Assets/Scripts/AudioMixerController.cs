using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    float off = -80f;
    float one = 0f;
    public Image barra;
    public Text textStyle;

   
    // Start is called before the first frame update
    void Awake()
    {
        audioMixer.SetFloat("lv1", off);
        audioMixer.SetFloat("lv2", off);
        audioMixer.SetFloat("lv3", off);
        audioMixer.SetFloat("lv4", off);
        audioMixer.SetFloat("lv5", off);
        audioMixer.SetFloat("lv6", off);
        textStyle.text = "";
        //audioSource.Stop();

    }

    // Update is called once per frame
    void Update()
    {

        //lv0
        if (barra.fillAmount <= 0.4)
        {
        //D
            textStyle.text = "D";
            audioMixer.SetFloat("lv1", off); // 0.2 * 300
            audioMixer.SetFloat("lv2", off);
            audioMixer.SetFloat("lv3", off);
            audioMixer.SetFloat("lv4", off);
            audioMixer.SetFloat("lv5", off);
            audioMixer.SetFloat("lv6", off);
        }
        //lv1
        if (barra.fillAmount >= 0.4 && barra.fillAmount <= 0.54)
        {
            //C
            textStyle.text = "C";
            audioMixer.SetFloat("lv1", one);
            audioMixer.SetFloat("lv2", off);
            audioMixer.SetFloat("lv3", off);
            audioMixer.SetFloat("lv4", off);
            audioMixer.SetFloat("lv5", off);
            audioMixer.SetFloat("lv6", off);
        }
        //lv2
        if (barra.fillAmount >= 0.54 && barra.fillAmount <= 0.67)
        {
            //B
            textStyle.text = "B";
            audioMixer.SetFloat("lv1", off);
            audioMixer.SetFloat("lv2", one);
            audioMixer.SetFloat("lv3", off); // d3 
            audioMixer.SetFloat("lv4", one);
            audioMixer.SetFloat("lv5", off);
            audioMixer.SetFloat("lv6", off);
        }
        //lv3
        if (barra.fillAmount >= 0.67 && barra.fillAmount <= 0.76)
        {
            //A
            textStyle.text = "A";
            audioMixer.SetFloat("lv1", off);
            audioMixer.SetFloat("lv2", one);
            audioMixer.SetFloat("lv3", off); // d3
            audioMixer.SetFloat("lv4", one);
            audioMixer.SetFloat("lv5", off);
            audioMixer.SetFloat("lv6", off);
        }
        //lv4
        if (barra.fillAmount >= 0.76 && barra.fillAmount <= 0.86)
        {
            //S
            textStyle.text = "S";
            audioMixer.SetFloat("lv1", off);
            audioMixer.SetFloat("lv2", one);
            audioMixer.SetFloat("lv3", off); // d3
            audioMixer.SetFloat("lv4", one);
            audioMixer.SetFloat("lv5", one);
            audioMixer.SetFloat("lv6", off);

        }
        //lv5
        if (barra.fillAmount >= 0.86 && barra.fillAmount <= 0.91)
        {
            //Ss
            textStyle.text = "Ss";
            audioMixer.SetFloat("lv1", off);
            audioMixer.SetFloat("lv2", off);
            audioMixer.SetFloat("lv3", one); // d3
            audioMixer.SetFloat("lv4", one);
            audioMixer.SetFloat("lv5", one);
            audioMixer.SetFloat("lv6", off);

        }
        //lv6
        if (barra.fillAmount > 0.91)
        {
            //Sss
            textStyle.text = "Sss";
            audioMixer.SetFloat("lv1", off);
            audioMixer.SetFloat("lv2", off);
            audioMixer.SetFloat("lv3", one);
            audioMixer.SetFloat("lv4", one);
            audioMixer.SetFloat("lv5", one);
            audioMixer.SetFloat("lv6", one);

        }
    }
}
