using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

// MY COD
public class GameManager : MonoBehaviour
{
    public int sizeX=2, sizeY=2;

    private GameObject[] _objects;

    private float minMax;

    private GridItem[,] _items;

    private GridItem _selectedItem;

    public int matchMinimun = 3;
    
    public float duration = 0.5f, spaceItems=0.2f, delayBetweenMatches = 0.2f;

    private bool _canPlay;

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
        Camera cam = Camera.main;

        _canPlay = true;
        GetObjects();
        CreateGrid();
        ClearGrid();
        GridItem.OnMouseOverItemEventHandler += OnMouseOverItem;

       //Camera
       cam.transform.position = new Vector3(minMax / 2, 0, -10);

    }

    void OnMouseOverItem(GridItem item)
    {
        if(_selectedItem == item || _canPlay == false)
        {
            return;
        }
        
        if(_selectedItem == null)
        {
            _selectedItem = item;

            _selectedItem.GetComponent<Animator>().SetTrigger("Select");

        }
        else 
        {
            int xResult = Mathf.Abs(item.x - _selectedItem.x);
            int yResult = Mathf.Abs(item.y - _selectedItem.y);

            if(xResult + yResult == 1)
            {
                StartCoroutine(TryMatch(_selectedItem, item, 0.1f));
            }

            _selectedItem.GetComponent<Animator>().SetTrigger("Deselect");

            _selectedItem = null;
        }
    }
    IEnumerator TryMatch(GridItem a, GridItem b, float duration)
    {
        _canPlay = false;
        yield return StartCoroutine(Swap(a, b, duration));

        MatchInfo matchInfoA = GetMatchInfo(a);
        MatchInfo matchInfoB = GetMatchInfo(b);

        if (!matchInfoA.IsMatchValid && !matchInfoB.IsMatchValid)
        {
            yield return StartCoroutine(Swap(a, b, duration));
            _canPlay = true;
            yield break;
        }

        if (matchInfoA.IsMatchValid)
        {
            yield return StartCoroutine(DestroyItems(matchInfoA.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGrid(matchInfoA));
        }

        if (matchInfoB.IsMatchValid)
        {
            yield return StartCoroutine(DestroyItems(matchInfoB.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGrid(matchInfoB));
        }


        if (!matchInfoA.IsMatchValid && !matchInfoB.IsMatchValid)
        {
            yield return StartCoroutine(Swap(a, b, duration));
            _canPlay = true;
            yield break;
        }

        _canPlay = true;
    }

    IEnumerator UpdateGrid(MatchInfo match)
    {
        if(match.verticalMatchStart == match.verticalMatchEnd) // [1,2][3,2] = Horizontalmacth 
        {
            for(int x = match.horizontalMatchStart; x <= match.horizontalMatchEnd; x++)
            {
                for (int y = match.verticalMatchStart; y < sizeY - 1; y++)
                {
                    GridItem aboveItem = _items[x, y + 1];
                    GridItem currentItem = _items[x, y];
                    _items[x, y] = aboveItem;
                    _items[x, y + 1] = currentItem;
                    _items[x, y].OnItemPosChanged(_items[x, y].x, _items[x, y].y - 1);
                }
                _items[x, sizeY - 1] = InstantiateItems(x, sizeY - 1);
            }
        }
        else if (match.horizontalMatchStart == match.horizontalMatchEnd)
        {
            int height = 1 + (match.verticalMatchEnd - match.verticalMatchStart);

            for (int y = match.verticalMatchStart + height; y <= sizeY - 1; y++)
            {
                GridItem belowItem = _items[match.horizontalMatchStart, y - height];
                GridItem current = _items[match.horizontalMatchStart, y];
                _items[match.horizontalMatchStart, y - height] = current;
                _items[match.horizontalMatchStart, y] = belowItem;
            }

            for (int y = 0; y < sizeY - height; y++)
            {
                _items[match.horizontalMatchStart, y].OnItemPosChanged(match.horizontalMatchStart, y);
            }

            for (int i = 0; i < match.match.Count; i++)
            {
                _items[match.horizontalMatchStart, (sizeY - 1) - i] = InstantiateItems(match.horizontalMatchStart, (sizeY - 1) - i);
            }
        }

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                MatchInfo matchInfo = GetMatchInfo(_items[x, y]);
                if (matchInfo.IsMatchValid)
                {
                    yield return StartCoroutine(DestroyItems(matchInfo.match));
                    yield return new WaitForSeconds(delayBetweenMatches);
                    yield return StartCoroutine(UpdateGrid(matchInfo));
                }
            }
        }
    }

    IEnumerator DestroyItems(List<GridItem> ListItems)
    {

        foreach(GridItem t_items in ListItems)
        {
            yield return StartCoroutine(t_items.transform.AninScale(Vector3.zero, duration));
            Destroy(t_items.gameObject);
        }
    }

    IEnumerator Swap(GridItem a, GridItem b, float duration)
    {
        ManagePhysics(false);

        Vector2 pos_A = a.transform.position;
        Vector2 pos_B = b.transform.position;

        StartCoroutine(a.transform.AninMove(pos_B, duration));
        StartCoroutine(b.transform.AninMove(pos_A, duration));
        
        SwapPosChanged(a, b);

        yield return new WaitForSeconds(duration*1.6f);

        ManagePhysics(true);
    }

    void SwapPosChanged(GridItem a, GridItem b)
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


    GridItem InstantiateItems(int xPos, int yPos)
    {
        GameObject randomObjects = _objects[Random.Range(0, _objects.Length)];
        GridItem NewItem = (Instantiate(randomObjects, new Vector3(xPos* spaceItems, yPos), Quaternion.identity)).GetComponent<GridItem>();
        NewItem.OnItemPosChanged(xPos,yPos);
        return NewItem;
    }

    void CreateGrid()
    {
        _items = new GridItem[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                _items[x,y] = InstantiateItems(x,y);
            }
            minMax += 1;
        }
    }
    void ClearGrid()
    {
        int bomb=0;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                MatchInfo matchInfo = GetMatchInfo(_items[x, y]);

              /*  if(_items[x,y].Id == 8)
                {
                    if(bomb < 2)
                    {
                        bomb++;
                    }
                    else
                    {
                        Destroy(_items[x, y].gameObject);
                        _items[x, y] = InstantiateItems(x, y);
                        y--;
                    }
                }
             */
              
                if (matchInfo.IsMatchValid)
                {
                    Destroy(_items[x, y].gameObject);
                    _items[x, y] = InstantiateItems(x, y);
                    y--;
                }
            }
        }
    }
    List<GridItem> CheckHorizontalMatches(GridItem item)
    {
        List<GridItem> horizontalMatches = new List<GridItem> { item };
        int left = item.x - 1, right = item.x + 1;

        while (left >= 0 && _items[left, item.y].Id == item.Id)
        {
            horizontalMatches.Add(_items[left, item.y]);
            left--;
        }

        while (right < sizeX && _items[right, item.y].Id == item.Id)
        {
            horizontalMatches.Add(_items[right, item.y]);
            right++;
        }

        return horizontalMatches;
    }
 
    List<GridItem> CheckVerticalMatches(GridItem item)
    {
        List<GridItem> verticalMatches = new List<GridItem> { item };
        int down = item.y - 1, up = item.y + 1;

        while (down >= 0 && _items[item.x,down].Id == item.Id)
        {
            verticalMatches.Add(_items[item.x, down]);
            down--;
        }

        while (up < sizeY && _items[item.x, up].Id == item.Id)
        {
            verticalMatches.Add(_items[item.x, up]);
            up++;
        }
        return verticalMatches;
    }
    // Ceck Info
    MatchInfo GetMatchInfo(GridItem item)
    {
        MatchInfo mInfo = new MatchInfo();
        mInfo.match = null;

        List<GridItem> horizontalMatch = CheckHorizontalMatches(item);
        List<GridItem> verticalMatch = CheckVerticalMatches(item);


        if (horizontalMatch.Count > verticalMatch.Count && horizontalMatch.Count >= matchMinimun)
        {
            mInfo.horizontalMatchStart = GetHorizontalStart(horizontalMatch);
            mInfo.horizontalMatchEnd = GetHorizontalEnd(horizontalMatch);

            mInfo.verticalMatchStart = mInfo.verticalMatchEnd = horizontalMatch[0].y;

            mInfo.match = horizontalMatch;
        }
        else if (verticalMatch.Count >= matchMinimun)
        {
            mInfo.verticalMatchStart = GetVerticalStart(verticalMatch);
            mInfo.verticalMatchEnd = GetVerticalEnd(verticalMatch);

            mInfo.horizontalMatchStart = mInfo.horizontalMatchEnd = verticalMatch[0].x;

            mInfo.match = verticalMatch;
        }
        return mInfo;
    }

    int GetHorizontalStart(List<GridItem> items)
    {
        float[] indexes = new float[items.Count];

        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = items[i].x;
        }
        return (int)Mathf.Min(indexes);
    }
    int GetHorizontalEnd(List<GridItem> items)
    {
        float[] indexes = new float[items.Count];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = items[i].x;
        }
        return (int)Mathf.Max(indexes);
    }
    int GetVerticalStart(List<GridItem> items)
    {
        float[] indexes = new float[items.Count];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = items[i].y;
        }
        return (int)Mathf.Min(indexes);
    }
    int GetVerticalEnd(List<GridItem> items)
    {
        float[] indexes = new float[items.Count];
        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = items[i].y;
        }
        return (int)Mathf.Max(indexes);
    }

}
