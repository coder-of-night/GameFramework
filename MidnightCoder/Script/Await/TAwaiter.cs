using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
public class TAwaiter<T> : INotifyCompletion
{
    public TAwaiter<T> GetAwaiter()
    {
        return this;
    }
    bool _isDone;
    Action _continuation;
    T _result;

    public bool IsCompleted
    {
        get { return _isDone; }
    }

    public T GetResult()
    {
        return _result;
    }

    public void Complete(T result = default(T))
    {
        _isDone = true;
        _result = result;
        if (_continuation != null)
        {
            _continuation();
        }
    }
    void INotifyCompletion.OnCompleted(Action continuation)
    {
        _continuation = continuation;
    }
}