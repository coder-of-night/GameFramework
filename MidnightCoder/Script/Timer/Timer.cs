using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MidnightCoder.Game
{
    public class Timer : TMonoSingleton<Timer>
    {
        public List<TimerTween> timerTweenList;
        private List<long> timerNodeListDelList;
        public long curId = 1;
        void Update()
        {
            if (this.timerTweenList.Count == 0)
            {
                //curId = 1;
                return;
            }

            for (int m = timerTweenList.Count - 1; m >= 0; m--)
            {
                if (this.timerNodeListDelList.Count == 0) break;
                TimerTween tn = timerTweenList[m];
                if (this.timerNodeListDelList.Contains(tn.id))
                {
                    timerTweenList.RemoveAt(m);
                    this.timerNodeListDelList.Remove(tn.id);
                }
            }
            timerNodeListDelList.Clear();

            for (int i = 0; i < timerTweenList.Count; i++)
            {
                TimerTween tn = timerTweenList[i];
                if (tn.target == null)
                {
                    this.timerNodeListDelList.Add(tn.id);
                    continue;
                }
                else
                {
                    tn.delayTime -= tn.realTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    if (tn.delayTime <= 0)
                    {
                        tn.onComplete?.Invoke();
                        this.timerNodeListDelList.Add(tn.id);
                    }
                    else
                    {
                        tn.onUpdate?.Invoke();
                    }
                }
            }
        }

        public override void OnSingletonInit()
        {
            this.timerTweenList = new List<TimerTween>();
            this.timerNodeListDelList = new List<long>();
            curId = 1;
        }
        public void Stop(TimerTween _tt)
        {
            if (_tt == null) return;
            //Debug.LogError($"停止了{_tt.id}");
            this.timerNodeListDelList.Add(_tt.id);
        }
        public void StopAll()
        {
            this.timerTweenList.Clear();
            this.timerNodeListDelList.Clear();
            curId = 1;
        }

        public TimerTween DoWait(float _time)
        {
            TimerTween tt = new TimerTween(curId++, _time);
            this.timerTweenList.Add(tt);
            return tt;
        }

    }

    [Serializable]
    public class TimerTween
    {
        public long id;
        public float delayTime;
        public GameObject target;
        public Action onUpdate;
        public Action onComplete;
        public bool realTime = false;
        public TimerTween(long id, float delayTime, GameObject target = null)
        {
            this.id = id;
            this.delayTime = delayTime;
            if (target == null)
            {
                this.target = Timer.S.gameObject;
            }
            else
            {
                this.target = target;
            }
        }
        /// <summary>
        /// 延时结束回调函数
        /// </summary>
        public TimerTween OnComplete(Action _call)
        {
            this.onComplete = _call;
            return this;
        }
        /// <summary>
        /// 延时周期更新函数
        /// </summary>
        public TimerTween OnUpdate(Action _call)
        {
            this.onUpdate = _call;
            return this;
        }
        /// <summary>
        /// 是否忽略TimeSclae缩放
        /// </summary>
        public TimerTween RealTime(bool flag)
        {
            this.realTime = flag;
            return this; ;
        }
        public TimerTween BindTarget(GameObject tar)
        {
            this.target = tar;
            return this;
        }


    }
}
