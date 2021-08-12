# GameFramework
Custom Unity Framework

#AssetBundle#
AB加载卸载

#Audio#
音乐音效管理

#Await#
异步编程实现的语句级阻塞
Case: 
void Function()
{
...........
await CustomAwaiter.WaitForAction(DoXXX);
//3秒后才继续下面语句。
..........
}
void DoXXX(CustomAwaiter awaiter)
{
    TODO:
    等待3秒 => awaiter.Complete();
}

#Debug#
一般的Log和原生平台堆栈跟踪的日志记录

#Editor#
定制Transform面板，便捷拷贝\粘贴本地和世界变换信息


#Event#
事件系统

#Extension#
一些常用的扩展方法

#FSM_Mini#
对象级的状态机，迷你精简。

#FSM_Standard#
标准状态机框架

#GameObjectPool#
对象池

#Input#
兼容PC与原生的触摸输入，可区分UI与场景。

#Joystick#
通用摇杆

#Singleton#
单例,请继承

#Timer#
定时器，效仿Dotween，使用Linq语句方便实现延时回调。
Case: Timer.S.DoWait(3).OnUpdate(()=>{todo per frame}).OnComplete(()=>{todo while complete}).BindTarget(lifetimeObject);

#Tools#
工具类

#UIFramework#
MVC的UI框架


