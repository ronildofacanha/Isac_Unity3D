using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchInfo
{
    public List<GridItem> match;

    public int horizontalMatchStart;
    public int horizontalMatchEnd;

    public int verticalMatchStart;
    public int verticalMatchEnd;

    public bool IsMatchValid
    {
        get { return match != null; }
    }
}
