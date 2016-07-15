using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class PageRectHelper : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        protected void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        protected void LateUpdate()
        {
            if (!scrollingToChild) return;
            UpdateScrollVelocity();
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (scrollRect == null || data.button != PointerEventData.InputButton.Left) return;
            scrollingToChild = false;
        }

        public void OnDrag(PointerEventData data)
        {
            if (scrollRect == null || data.button != PointerEventData.InputButton.Left) return;
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (scrollRect == null || data.button != PointerEventData.InputButton.Left) return;

            Vector2 offset = Vector2.zero;
            int index = 0;
            if (!FindNearestChildToViewport(out index, out offset)) return;
            nearestChild = scrollRect.content.GetChild(index) as RectTransform;

            scrollRect.velocity = Vector2.zero;
            scrollingToChild = true; ;
        }


        protected bool FindNearestChildToViewport(out int index, out Vector2 offset)
        {
            bool find = false;
            offset = Vector2.zero ;
            float sqrMagnitude = float.MaxValue;
            index = 0;

            Bounds viewportBounds = GetBounds(scrollRect.viewport, null);
            Vector3 viewportCenter = viewportBounds.center;
            for(int i=0; i<scrollRect.content.childCount; ++i)
            {
                RectTransform child = scrollRect.content.GetChild(i) as RectTransform;
                if (child == null) continue;
                Bounds childBounds = GetBounds(child, scrollRect.viewport);
                Vector3 childCenter = childBounds.center;

                Vector2 childOffset = Vector2.zero;
                if (scrollRect.horizontal) childOffset.x = viewportCenter.x - childCenter.x;
                if (scrollRect.vertical) childOffset.y = viewportCenter.y - childCenter.y;

                if (childOffset.sqrMagnitude > sqrMagnitude) continue;
                offset = childOffset;
                sqrMagnitude = childOffset.sqrMagnitude;
                index = i;
                find = true;
            }

            return find;
        }

        protected void UpdateScrollVelocity()
        {
            float deltaTime = Time.unscaledDeltaTime;
            Bounds viewportBounds = GetBounds(scrollRect.viewport, null);
            Bounds childBounds = GetBounds(nearestChild, scrollRect.viewport);
            Vector2 offset = CalculateOffset(Vector2.zero, childBounds, viewportBounds);

            Vector2 position = scrollRect.content.anchoredPosition;
            Vector2 velocity = scrollRect.velocity;
            for (int axis = 0; axis < 2; axis++)
            {
                // Apply spring physics if movement is elastic and content has an offset from the view.
                if (scrollRect.movementType == UnityEngine.UI.ScrollRect.MovementType.Elastic && offset[axis] != 0)
                {
                    float speed = scrollRect.velocity[axis];
                    position[axis] = Mathf.SmoothDamp(scrollRect.content.anchoredPosition[axis], scrollRect.content.anchoredPosition[axis] + offset[axis], ref speed, scrollRect.elasticity, Mathf.Infinity, deltaTime);
                    velocity[axis] = speed;
                }
                // Else move content according to velocity with deceleration applied.
                else if (scrollRect.inertia)
                {
                    velocity[axis] *= Mathf.Pow(scrollRect.decelerationRate, deltaTime);
                    if (Mathf.Abs(scrollRect.velocity[axis]) < 1)
                        velocity[axis] = 0;
                    position[axis] += scrollRect.velocity[axis] * deltaTime;
                }
                // If we have neither elaticity or friction, there shouldn't be any velocity.
                else
                {
                    velocity[axis] = 0;
                }
            }
            scrollRect.velocity = velocity;
            if (offset.sqrMagnitude < 1)
            {
                scrollingToChild = false;
            }
        }

        protected Vector2 CalculateOffset(Vector2 delta, Bounds childBounds, Bounds viewportBounds)
        {
            Vector2 offset = Vector2.zero;
            if (scrollRect.movementType == UnityEngine.UI.ScrollRect.MovementType.Unrestricted)
                return offset;

            Vector3 viewportCenter = viewportBounds.center;
            Vector3 childCenter = childBounds.center;

            if (scrollRect.horizontal) offset.x = viewportCenter.x - childCenter.x;
            if (scrollRect.vertical) offset.y = viewportCenter.y - childCenter.y;

            return offset;
        }

        protected Bounds GetBounds(RectTransform target, RectTransform relativeTarget)
        {
            if (target == null) return new Bounds();
            if (relativeTarget == null) return new Bounds(target.rect.center, target.rect.size);

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = relativeTarget.worldToLocalMatrix;
            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        protected ScrollRect scrollRect;
        protected bool scrollingToChild = false;
        protected RectTransform nearestChild;
    }
}
