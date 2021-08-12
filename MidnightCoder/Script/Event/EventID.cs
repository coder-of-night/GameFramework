using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EventID
{
    OnCloudTypeChange = 100, //云类型更换
    OnMoneyChange, //金币改变
    ChangeItemOperateState, //物体可交互状态改变
    OnIOMovieState, //是否在剧情状态
    OnGamePlayStart, //玩家开始完了
    OnGetInputState, //是否获取到玩家输入
    OnLevelComplete,
}
