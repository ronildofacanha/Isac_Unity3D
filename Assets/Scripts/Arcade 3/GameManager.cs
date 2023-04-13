using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public int SizeX, SizeY;

    private GameObject[] _objects;

    float minMax, boxSpace = 1.1f;

    private GridItem[,] _items;

    private GridItem _selectItem;

    private void Update()
    {
    }
    void GetObjects()
    {
        _objects = Resources.LoadAll<GameObject>("Fruit's");
    }

    private void OnDisable()
    {
        GridItem.OnMouseOverItemEventHandler -= OnMouseOverItem;
    }

    void Start()
    {
        GetObjects();
        CreateGrid();
        Camera cam = Camera.main;
        cam.transform.position = new Vector3(minMax/2,0,-10);
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;
    }
 
    void OnMouseOverItem(GridItem item)
    {
        if(_selectItem == item)
        {
            return;
        }
        
        if(_selectItem == null)
        {
            _selectItem = item;

            _selectItem.GetComponent<Animator>().SetTrigger("Select");

        }
        else 
        {
            int xResult = Mathf.Abs(item.x - _selectItem.x);
            int yResult = Mathf.Abs(item.y - _selectItem.y);

            if(xResult + yResult == 1)
            {
                StartCoroutine(Swap(_selectItem,item, 0.5f));
                itemsPosChanged(_selectItem, item); // apagar

                // noooo
                if (CheckHorizontalMatches(_selectItem).Count > 2)
                {
                    foreach(GridItem g in CheckHorizontalMatches(_selectItem))
                    {
                        Destroy(g.gameObject);
                    }
                }
                if(CheckHorizontalMatches(item).Count > 2)
                {
                    foreach (GridItem g in CheckHorizontalMatches(_selectItem))
                    {
                        Destroy(g.gameObject);
                    }
                }


            }
            _selectItem.GetComponent<Animator>().SetTrigger("Deselect");

            _selectItem = null;
        }
    }

    IEnumerator Swap(GridItem a, GridItem b, float duration)
    {
        ManagePhysics(false);

        Vector2 pos_A = a.transform.position;
        Vector2 pos_B = b.transform.position;

        StartCoroutine(a.transform.Move(pos_B, duration));
        StartCoroutine(b.transform.Move(pos_A, duration));

        itemsPosChanged(a, b);

        yield return new WaitForSeconds(duration);
        
        ManagePhysics(true);
    }

    void itemsPosChanged(GridItem a, GridItem b)
    {
        GridItem tempA = _items[a.x, a.y];
        _items[a.x, a.y] = b;
        _items[b.x, b.y] = tempA;

        int tempX = a.x, tempY = a.y;
        a.OnItemPosChanged(b.x, b.y);
        b.OnItemPosChanged(tempX, tempY);
    }

    void ManagePhysics(bool state)
    {
        foreach (GridItem i in _items)
        {
            i.GetComponent<Rigidbody2D>().isKinematic = !state;
        }
    }


    GridItem InstantiateObjscts(int xPos, int yPos)
    {
        GameObject randomObjects = _objects[Random.Range(0, _objects.Length)];
        GridItem NewItem = (Instantiate(randomObjects, new Vector3(xPos*boxSpace, yPos), Quaternion.identity)).GetComponent<GridItem>();
        NewItem.OnItemPosChanged(xPos,yPos);
        return NewItem;
    }

    void CreateGrid()
    {
        _items = new GridItem[SizeX, SizeY];

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                _items[x,y] = InstantiateObjscts(x,y);
            }
            minMax += 1;
        }
    }

    List<GridItem> CheckHorizontalMatches(GridItem item)
    {
        List<GridItem > horizontalMatches = new List<GridItem> {item};
        int left = item.x - 1, right = item.x + 1;

        while (left >= 0 && _items[left, item.y].Id == item.Id)
        {
            horizontalMatches.Add(_items[left, item.y]);
            left--;
        }

        while (right < SizeX && _items[right, item.y].Id == item.Id)
        {
            horizontalMatches.Add(_items[right, item.y]);
            right++;
        }
        return horizontalMatches;
    }
   
}
