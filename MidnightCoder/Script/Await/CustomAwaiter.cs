using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
public class CustomAwaiter : INotifyCompletion
{
    public CustomAwaiter GetAwaiter()
    {
        return this;
    }
    bool _isDone;
    Action _continuation;

    public bool IsCompleted
    {
        get { return _isDone; }
    }

    public void GetResult()
    {

    }

    public void Complete()
    {
        _isDone = true;
        if (_continuation != null)
        {
            _continuation();
        }
    }
    void INotifyCompletion.OnCompleted(Action continuation)
    {
        _continuation = continuation;
    }
    /// <summary>
    /// 可等待的方法
    /// </summary>
    /// <param name="act">传入参数1为CustomAwaiter的方法，用于主动结束等待</param>
    /// <returns></returns>
    public static CustomAwaiter WaitForAction(Action<CustomAwaiter> act)
    {
        var awaiter = new CustomAwaiter();
        act(awaiter);
        return awaiter;
    }
    /// <summary>
    /// 可等待的方法,带一个参数
    /// </summary>
    /// <returns></returns>
    public static CustomAwaiter WaitForAction<T>(Action<CustomAwaiter, T> act, T param)
    {
        var awaiter = new CustomAwaiter();
        act(awaiter, param);
        return awaiter;
    }
}