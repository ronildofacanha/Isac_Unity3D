using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int SizeX, SizeY;

    public GameObject[] objects;

    float minMax;

    void GetObjects()
    {
        objects = Resources.LoadAll<GameObject>("Fruit's");
    }

    void Start()
    {
        GetObjects();
        CreateGrid();
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(minMax/2,0,-10);
    }

    void InstantiateObjscts(int xPos, int yPos)
    {
        GameObject randomObjects = objects[Random.Range(0, objects.Length)];
        GridItem NewItem = (Instantiate(randomObjects, new Vector3(xPos, yPos), Quaternion.identity)).GetComponent<GridItem>();
        NewItem.OnItemPosChanged(xPos,yPos);
    }

    void CreateGrid()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                InstantiateObjscts(x, y);
            }
            minMax += 1;
        }
    }
}
