using System.Collections;
using System.Collections.Generic;
using System;
[Serializable]
public class GraphNode<T>
{
    public string Id;
   // protected List<GraphNode<T>> NextList;
    public T Value;

    public GraphNode()
    {

    }
    public GraphNode(string _id, T _v)
    {
        Id = _id;
        Value = _v;
     //   NextList = new List<GraphNode<T>>();
    }


    //public void AddNextNode(GraphNode<T> _n)
    //{
    //    if (NextList == null)
    //    {
    //        NextList = new List<GraphNode<T>>();
    //    }
    //    NextList.Add(_n);
    //}

    //public virtual bool HaveNextNode()
    //{
    //    return NextList.Count > 0;
    //}

    //public  List<GraphNode<T>> GetNextList()
    //{
    //    return NextList;
    //}
}
