using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace MidnightCoder.Game
{
    public class TouchStateX
    {
        public Vector3 pos;
        public GameObject target;

        public TouchStateX(Vector3 pos, GameObject target)
        {
            this.pos = pos;
            this.target = target;
        }
    }

    public static class TouchX
    {
        public static Vector3 curTouchPosition
        {
            get
            {
                if (Input.touchSupported && Input.touchCount > 0)
                {
                    return Input.GetTouch(0).position;
                }
                return Input.mousePosition;
            }
        }
        public static bool GetTouchStay()
        {
            if (Input.GetMouseButton(0))
            {
                return true;
            }
            if (!Input.touchSupported)
            {
                return false;
            }
            if (Input.touchCount > 0)
            {
                TouchPhase phase = Input.GetTouch(0).phase;
                if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
                {
                    return true;
                }
            }
            return false;
        }

        public static TouchStateX GetTouchBegan(bool ignoreUI = false)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchSupported && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
            {
                Vector3 pos = curTouchPosition;
                GameObject target = null;
                if (!ignoreUI)
                {
                    List<RaycastResult> result = GetObjectOverUI(pos);
                    if (result.Count > 0)
                    {
                        target = result[0].gameObject;
                    }
                }
                if (target == null)
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out RaycastHit hit, 1000, ~(1 << LayerMask.NameToLayer("UI")), QueryTriggerInteraction.Collide))
                    {
                        target = hit.transform.gameObject;
                    }
                }
                return new TouchStateX(pos, target);
            }
            else
            {
                return null;
            }
        }

        public static bool GetTouchEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            if (!Input.touchSupported)
            {
                return false;
            }
            if (Input.touchCount > 0)
            {
                TouchPhase phase = Input.GetTouch(0).phase;
                if (phase == TouchPhase.Ended)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsPointerOverUIObject(Vector2 screenPosition)
        {
            List<RaycastResult> results = GetObjectOverUI(screenPosition);
            return results.Count > 0;
        }

        private static List<RaycastResult> GetObjectOverUI(Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            //foreach (RaycastResult item in results)
            //{
            //    Debug.LogWarning("触摸点下有如下UI");
            //    Debug.LogWarning(item.gameObject.name);
            //}
            return results;
        }

    }
}
