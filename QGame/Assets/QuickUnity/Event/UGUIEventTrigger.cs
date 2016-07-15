using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace QuickUnity
{
    public class UGUIEventTrigger : EventTrigger, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
    {
        public bool interactable
        {
            get { return _interactable; }
            set
            {
                _interactable = value;
                if (selectables == null) return;
                for(int i=0; i<selectables.Length; ++i)
                {
                    var select = selectables[i];
                    if (select == null) continue;
                    select.interactable = _interactable;
                }
            }
        }
        [SerializeField]
        protected bool _interactable = true;

        [SerializeField]
        public Selectable[] selectables;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.BeginDrag, eventData);
        }

        public void OnCancel(BaseEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Cancel, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Drag, eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Drop, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.EndDrag, eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.InitializePotentialDrag, eventData);
        }

        public void OnMove(AxisEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Move, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.PointerClick, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.PointerDown, eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.PointerEnter, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.PointerExit, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.PointerUp, eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Scroll, eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Select, eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Submit, eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.UpdateSelected, eventData);
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            if (!interactable) return;
            ProccessEvent((int)EngineEvent.Deselect, eventData);
        }

//         public event UIEventHandler onDeselect;
//         public event UIEventHandler onBeginDrag;
//         public event UIEventHandler onDrag;
//         public event UIEventHandler onEndDrag; 
//         public event UIEventHandler onDrop;
//         public event UIEventHandler onMove;
//         public event UIEventHandler onClick;
//         public event UIEventHandler onDown;
//         public event UIEventHandler onEnter;
//         public event UIEventHandler onExit;
//         public event UIEventHandler onUp;
//         public event UIEventHandler onScroll;
//         public event UIEventHandler onSelect;
//         public event UIEventHandler onUpdateSelect;
// 
//         public delegate void UIEventHandler(GameObject go, BaseEventData data);
    }
}


