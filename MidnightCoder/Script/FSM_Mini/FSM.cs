using System;
using System.Collections.Generic;
using UnityEngine;

namespace MidnightCoder.Game
{
    public class FSM
    {
        //
        // Fields
        //
        private Dictionary<string, FSMStateInfo> states = new Dictionary<string, FSMStateInfo>();

        private string prevName;

        private string currName;

        private string nextName;

        //
        // Constructors
        //
        public FSM()
        {
            this.prevName = string.Empty;
            this.currName = string.Empty;
            this.nextName = string.Empty;
        }

        //
        // Methods
        //
        public void AddState(string name, FSMStateEventDg enter, FSMStateEventDg update, FSMStateEventDg exit, bool isDefault = false)
        {
            FSMStateInfo fSMStateInfo;
            if (this.states.TryGetValue(name, out fSMStateInfo))
            {
                fSMStateInfo.enter = enter;
                fSMStateInfo.update = update;
                fSMStateInfo.exit = exit;
                return;
            }
            this.states.Add(name, new FSMStateInfo(enter, update, exit));
            if (isDefault)// || this.states.Count == 1  取消默认添加的第一个状态为初始状态
            {
                this.SetNextState(name, false);
            }
        }

        public string GetCurrState()
        {
            return this.currName;
        }

        public int GetCurrStateHash()
        {
            return Animator.StringToHash(this.currName.ToLower());
        }

        public string GetNextState()
        {
            return this.nextName;
        }

        public string GetPrevState()
        {
            return this.prevName;
        }

        public bool IsEmpty()
        {
            return this.states.Count <= 0;
        }

        public void SetNextState(string name, bool immediate = false, bool repeatState = false)
        {
            if (name == string.Empty)
            {
                return;
            }
            if (name == this.currName && !repeatState)
            {
                return;
            }
            FSMStateInfo fSMStateInfo;
            if (!this.states.TryGetValue(name, out fSMStateInfo))
            {
                Debug.LogWarning("FSM::SetNextState(" + name + ") state does not exist");
                return;
            }
            this.nextName = name;
            if (immediate)
            {
                this.Update();
            }
        }

        public void Update()
        {
            FSMStateInfo fSMStateInfo;
            bool flag = this.states.TryGetValue(this.currName, out fSMStateInfo);
            FSMStateInfo fSMStateInfo2;
            bool flag2 = this.states.TryGetValue(this.nextName, out fSMStateInfo2);
            if (flag && flag2)
            {
                if (fSMStateInfo.exit != null)
                {
                    //Debug.LogError(this.currName + "_Exit");
                    fSMStateInfo.exit();
                }
                this.prevName = this.currName;
                this.currName = this.nextName;
                this.nextName = string.Empty;
                if (fSMStateInfo2.enter != null)
                {
                    //Debug.LogError(this.currName + "_Enter");
                    fSMStateInfo2.enter();
                }
            }
            else
            {
                if (flag && !flag2)
                {
                    if (fSMStateInfo.update != null)
                    {
                       // Debug.Log(this.currName + "_Update");
                        fSMStateInfo.update();
                    }
                }
                else
                {
                    if (!flag && flag2)
                    {
                        this.prevName = this.currName;
                        this.currName = this.nextName;
                        this.nextName = string.Empty;
                        if (fSMStateInfo2.enter != null)
                        {
                           // Debug.LogError(this.currName + "_Enter");
                            fSMStateInfo2.enter();
                        }
                    }
                }
            }
        }
    }
}
