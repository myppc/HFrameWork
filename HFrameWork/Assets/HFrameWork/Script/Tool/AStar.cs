using UnityEngine;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2Int;
public class PathNode
{
    public Vector2 mSelf;
    public PathNode mFrom;
    public Vector2 mDest;
    public float gValue;
    public float hValue;
    public float fValue;

    public PathNode(Vector2 start,Vector2 dest)
    {
        mDest = dest;
        mSelf = start;
        gValue = 0;
        hValue = 0;
        fValue = 0;
    }

    public PathNode(PathNode from,Vector2 self,Vector2 dest)
    {
        mSelf = self;
        mDest = dest;
        SetFrom(from);
    }

    public void SetFrom(PathNode from)
    {
        if (from == null)
        {
            return;
        }
        mFrom = from;
        var dis = mSelf - from.mSelf;
        var len = Mathf.Abs(dis.x) + Mathf.Abs(dis.y);
        switch (len)
        {
            case 0:
                {
                    gValue = 0 + from.gValue;
                    break;
                }
            case 1:
                {
                    gValue = 10 + from.gValue;
                    break;
                }
            case 2:
                {
                    gValue = 14 + from.gValue;
                    break;
                }
        }
        var hLen = mDest - mSelf;
        hValue = Mathf.Abs(hLen.x) *10 + Mathf.Abs(hLen.y) * 10;
        fValue = hValue + gValue;
    }
}

public  class AStar
{
    //public delegate bool CheckPassEnable(Vector2 vector,Vector2 dest);
    // public  CheckPassEnable mChecker;
    public List<PathNode> mOpenList;
    public List<Vector2> mCloseList;
    public Vector2 mStart;
    public Vector2 mDest;
    public int maxX;
    public int maxY;
    public delegate bool CheckDelegate(Vector2 point);
    public CheckDelegate checkFunc;


    public AStar(int x,int y,CheckDelegate check)
    {
        mOpenList = new List<PathNode>();
        mCloseList = new List<Vector2>();
        maxX = x;
        maxY = y;
        checkFunc = check;
    }

    public  bool GetCellState(Vector2 point)
    {
        return checkFunc(point);
        //var cellIndex = new Vector2(x, y);
        //var key = AdventureManager.GetInstance().IndexTokey(cellIndex);
        //var cellInfo = AdventureManager.GetInstance().GetCellInfo(AdventureManager.GetInstance().IndexTokey(cellIndex));
        //if (cellInfo.prefab != null)
        //{
        //    return false;
        //}

        //if (!AdventureManager.GetInstance().GetCellDiscovery(key))
        //{
        //    return false;
        //}
        //var index = cellInfo.props.FindIndex((item) => (AdvMapProp)item.propId == AdvMapProp.UN_PASS );
        //if (index != -1)
        //{
        //    return false;
        //}
        //return true;
    }


    public void Destroy()
    {
        mOpenList = null;
        mCloseList = null;
        mStart = Vector2.zero;
        mDest = Vector2.zero;
    }

    /// <summary>
    /// 查找最短路径
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="dest">终点</param>
    /// <returns>路径点</returns>
    public List<Vector2> SerchPath(Vector2 start,Vector2 dest)
    {
        this.mStart = start;
        this.mDest = dest;
        var pathNode = new PathNode(start, dest);
        this.mOpenList.Add(pathNode);
        while (true)
        {
            if (this.LoopPath())
            {
                break;
            }
        }
        if (this.GetOpenListIndex(this.mDest) == -1)
        {
            this.Clear();
            return null;
        }
        else
        {
            var ret = new List<Vector2>();

            var lastPathNode = this.GetPathNodeInOpenList(dest);
            while (true)
            {
                if (lastPathNode == null)
                {
                    this.Clear();
                    ret.Reverse();
                    return ret;
                }
                else
                {
                    ret.Add(lastPathNode.mSelf);
                    lastPathNode = lastPathNode.mFrom;
                }
            }
        }
    }

    public int GetOpenListIndex(Vector2 vec)
    {
        foreach (var item in this.mOpenList)
        {
            if (item.mSelf.Equals(vec))
            {
                return mOpenList.IndexOf(item);
            }
        }
        return -1;
    }


    public int GetCloseListIndex(Vector2 vec)
    {
        return mCloseList.IndexOf(vec);
    }

    public PathNode GetPathNodeInOpenList(Vector2 vec)
    {
        foreach (var item in mOpenList)
        {
            if (item.mSelf.Equals(vec))
            {
                return item;
            }
        }
        return null;
    }

    public bool LoopPath()
    {
        if (GetOpenListIndex(mDest) != -1 || mOpenList.Count == 0)
        {
            return true;
        }
        PathNode minPathNode = null;
        for(var i = 0;i< mOpenList.Count;i++)
        {
            var ptNode = mOpenList[i];
            if (minPathNode == null)
            {
                minPathNode = ptNode;
            }
            else if(ptNode.gValue + ptNode.hValue <= minPathNode.gValue + minPathNode.hValue)
            {
                minPathNode = ptNode;
            }
        }
        mOpenList.Remove(minPathNode);
        mCloseList.Add(minPathNode.mSelf);
        ErgodicAround(minPathNode);
        return false;
    }

    /// <summary>
    /// 清空当前数据
    /// </summary>
    public void Clear()
    {
        mOpenList.Clear();
        mCloseList.Clear();
        mDest = Vector2.zero;
        mStart = Vector2.zero;
    }

    /// <summary>
    /// 遍历指定格子周边格子
    /// </summary>
    /// <param name="pos">指定的cell</param>
    public void ErgodicAround(PathNode cell)
    {
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                #if DIR_4
                if (Mathf.Abs(x) + Mathf.Abs(y) != 1)
                {
                    continue;
                }
                #endif
                Vector2 serchCell = cell.mSelf + new Vector2(x, y);
                if (GetCloseListIndex(serchCell) != -1)
                {

                }
                else if (!CheckCellPass(serchCell, mDest) && !serchCell.Equals(mDest))
                {
                    mCloseList.Add(serchCell);
                }
                else
                {
                    var newNode = new PathNode(cell, serchCell, mDest);
                    if (GetOpenListIndex(serchCell) == -1)
                    {
                        mOpenList.Add(newNode);
                    }
                    else
                    {
                        var oldNode = GetPathNodeInOpenList(serchCell);
                        if (newNode.gValue <= oldNode.gValue)
                        {
                            oldNode.SetFrom(cell);
                        }
                    }
                }
            }
        }
    }

    public bool CheckCellPass(Vector2 cell, Vector2 dest)
    {
        if (cell.x <= 0 || cell.y <= 0)
        {
            return false;
        }
        if (cell.y > maxY)
        {
            return false;
        }
        if (cell.x > maxX)
        {
            return false;
        }

        return GetCellState(cell);
    }
}