using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrastar : MonoBehaviour
{
    bool clickTime;
    public SpringJoint2D eslatico;
    Transform estilingue;
    public float esticadaMaxima = 3.0f;
    float esticadaMaximaQuadrada;
    Ray raioParaMouse;
    public Rigidbody2D myRigidbody2D;
    Vector2 velAnterior;
    Ray raioEstilingDown;

    public LineRenderer pointUp;
    public LineRenderer pointDown;
    public CapsuleCollider2D colliderPlayer;
    public GameObject point;

    public float posZ_1 = 0f;
    public float posZ_2 = 0f;

    private void Awake()
    {

    }
    void Start()
    {
        estilingue = eslatico.connectedBody.transform;
        esticadaMaximaQuadrada = esticadaMaxima * esticadaMaxima;
        raioParaMouse = new Ray(estilingue.position, Vector3.zero);
        raioEstilingDown = new Ray(pointDown.transform.position, Vector3.zero);
        LineConfig();
    }

    void LineConfig()
    {
        Vector3 pos1_0 = new Vector3 (pointUp.transform.position.x, pointUp.transform.position.y,posZ_1);
        Vector3 pos2_0 = new Vector3(pointDown.transform.position.x, pointDown.transform.position.y, posZ_2);

        pointUp.SetPosition(0, pos1_0);
        pointDown.SetPosition(0,pos2_0);

        pointUp.sortingOrder = 10;
        pointDown.sortingOrder = 0;
    }
    void RenderLine()
    {
        Vector2 estilingParaProjetil = point.transform.position - pointDown.transform.position;
        raioEstilingDown.direction = estilingParaProjetil;
        Vector3 pontoDeAmarra = raioEstilingDown.GetPoint(estilingParaProjetil.magnitude);

        Vector3 pos1_1 = new Vector3(pontoDeAmarra.x, pontoDeAmarra.y, posZ_1);
        pointUp.SetPosition(1, pos1_1);

        Vector3 pos2_1 = new Vector3(pontoDeAmarra.x, pontoDeAmarra.y, posZ_2);
        pointDown.SetPosition(1, pos2_1);

    }

    void Update()
    {
        if (clickTime == true)
            DragClick();

        if(eslatico != null)
        {
            if(!myRigidbody2D.isKinematic && velAnterior.sqrMagnitude > myRigidbody2D.velocity.sqrMagnitude)
            {
                Destroy(eslatico);
                myRigidbody2D.velocity = velAnterior;
            }

            if (!clickTime)
                velAnterior = myRigidbody2D.velocity;

            RenderLine();
        }
        else
        {

        }
    }

    private void OnMouseDown()
    {
        clickTime = true;
        eslatico.enabled = false;
    }
    private void OnMouseUp()
    {
        clickTime = false;
        eslatico.enabled = true;
        myRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
    void DragClick()
    {
        Vector3 posMouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 estilingueParaMouse = posMouseWorld - estilingue.position;

        if(estilingueParaMouse.sqrMagnitude > esticadaMaximaQuadrada)
        {
            raioParaMouse.direction = estilingueParaMouse;
            posMouseWorld = raioParaMouse.GetPoint(esticadaMaxima);
        }
        posMouseWorld.z = 0;
        transform.position = posMouseWorld;

    }
} 
