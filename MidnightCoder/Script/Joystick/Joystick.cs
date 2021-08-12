using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidnightCoder.Game;
[RequireComponent(typeof(CanvasGroup))]
public class Joystick : MonoBehaviour
{
    [SerializeField, Title("杆子"), Tooltip("摇杆操纵点")]
    private RectTransform dot;

    [SerializeField, Title("底部圈子"), Tooltip("摇杆背景节点")]
    private RectTransform ring;

    [SerializeField, Title("操控角色"), Tooltip("操控角色")]
    private Transform player;
    public Transform Player { get { return player; } set { player = value; } }

    [SerializeField, Title("摇杆类型"), Tooltip("定死的还是随触摸位置变化的")]
    private JoystickType joystickType = JoystickType.FOLLOW;

  //  [SerializeField, Title("方向限制"), Tooltip("万向,四向,八向")]
  //  private DirectionType directionType = DirectionType.ALL;
    [SerializeField, Header("摇杆死角"), Range(0, 1)]
    private float deathArea = 0;

    //private RectTransform uiRoot;

    private Camera uiCamera;

    [SerializeField] private CanvasGroup cg;

    private Vector2 _stickPos; //摇杆所在位置
    private Vector2 _touchLocation; //触摸位置
    private float _radius; //

    private bool isStartFromUI = false; //是否一开始就点在UI上，如果是那么无论怎么滑都不应该再响应摇杆逻辑了
    void Awake()
    {
       // this.uiRoot = UIManager.S.GetComponent<RectTransform>();
        this.uiCamera = UIManager.S.GetComponent<Canvas>().worldCamera;
        this.cg = GetComponent<CanvasGroup>();
        this._radius = this.ring.rect.width / 2;
        // 当摇杆为跟随类型时隐藏显示
        if (this.joystickType == JoystickType.FOLLOW)
        {
            this.cg.alpha = 0;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TouchX.IsPointerOverUIObject(TouchX.curTouchPosition))
            {
                isStartFromUI = true;
                return;
            }
            EventSystem.S.Send(EventID.OnGetInputState, true);
            this._touchStartEvent();
        }
        if (!isStartFromUI && Input.GetMouseButton(0))
        {
            this._touchMoveEvent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isStartFromUI = false;
            EventSystem.S.Send(EventID.OnGetInputState, false);
            this._touchEndEvent();
        }

    }

    private void _touchStartEvent()
    {
        Vector2 touchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(),
            Input.mousePosition, uiCamera, out touchPos);
        if (this.joystickType == JoystickType.FIXED)
        {
            this._stickPos = this.ring.localPosition;

            // 触摸点与圆圈中心的距离
            float distance = Vector2.Distance(touchPos, this.ring.localPosition);
            //Debug.LogError(touchPos);
            //Debug.LogError(_stickPos);
            //Debug.LogError(distance);
            //Debug.LogError(_radius);
            // 手指在圆圈内触摸,控杆跟随触摸点
            if (this._radius > distance)
            {
                this.dot.position = touchPos;
            }

        }
        else if (this.joystickType == JoystickType.FOLLOW)
        {

            // 记录摇杆位置，给 touch move 使用
            this._stickPos = touchPos;
            this.cg.alpha = 1;
            this._touchLocation = touchPos;

            // 更改摇杆的位置
            this.ring.anchoredPosition = touchPos;
            this.dot.anchoredPosition = touchPos;
        }
    }

    private bool IsInDeathArea()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(),
            Input.mousePosition, uiCamera, out mousePos);
        return Vector2.Distance(_touchLocation, mousePos) <= deathArea * _radius;
    }
    private void _touchMoveEvent()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(),
            Input.mousePosition, uiCamera, out mousePos);
        if (this.joystickType == JoystickType.FOLLOW)
        {
            // 如果 touch start 位置和 touch move 在摇杆死角区域内，禁止移动
            if (_touchLocation.Equals(mousePos))
            {
                return;
            }
        }

        // 以圆圈为锚点获取触摸坐标
        Vector3 touchPos = mousePos - this.ring.anchoredPosition;
        float distance = touchPos.magnitude;
        // 由于摇杆的 postion 是以父节点为锚点，所以定位要加上 touch start 时的位置
        float posX = this._stickPos.x + touchPos.x;
        float posY = this._stickPos.y + touchPos.y;

        // 归一化
        Vector2 d = new Vector2(posX, posY) - this.ring.anchoredPosition;
        Vector2 p = d.normalized;

        if (this._radius > distance)
        {
            this.dot.anchoredPosition = new Vector2(posX, posY);
            // this.UpdatePlayerInput(SpeedType.NORMAL, p);
        }
        else
        {
            // 控杆永远保持在圈内，并在圈内跟随触摸更新角度
            float x = this._stickPos.x + p.x * this._radius;
            float y = this._stickPos.y + p.y * this._radius;
            this.dot.anchoredPosition = new Vector2(x, y);
            // this.UpdatePlayerInput(SpeedType.FAST, p);
        }

        if (this.player == null) return;
        //  if (IsInDeathArea()) return;

        this.UpdatePlayerInput(SpeedType.FAST, p * Mathf.Clamp(d.magnitude / _radius, 0, 1));
    }

    private void _touchEndEvent()
    {
        this.dot.anchoredPosition = this.ring.anchoredPosition;
        if (this.joystickType == JoystickType.FOLLOW)
        {
            this.cg.alpha = 0;
        }

        if (this.player == null) return;
        this.UpdatePlayerInput(SpeedType.STOP, Vector2.zero);
    }

    private void UpdatePlayerInput(SpeedType _type, Vector2 _dir)
    {
        this.player.SendMessage("SetSpeedType", _type, SendMessageOptions.DontRequireReceiver);
        this.player.SendMessage("SetDir", _dir, SendMessageOptions.DontRequireReceiver);


    }
}
