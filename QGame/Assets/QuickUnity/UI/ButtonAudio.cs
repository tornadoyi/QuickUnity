using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ButtonAudio : AudioPlayer, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Play(EventTriggerType.PointerClick);
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Play(EventTriggerType.PointerDown);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Play(EventTriggerType.PointerUp);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Play(EventTriggerType.PointerEnter);
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Play(EventTriggerType.PointerExit);
        }
        

        protected void Play(EventTriggerType type)
        {
            if ((int)type != (int)triggerType) return;
            if (string.IsNullOrEmpty(symbolAudio)) return;
            var button = GetComponent<Button>();
            if(button == null)
            {
                Debug.LogError("Can not find button");
                return;
            }

            //PlaySymbolAsync(symbolAudio, symbolSample);
        }


        public string symbolAudio;
        public string symbolSample;
        public ButtonTriggerType triggerType = ButtonTriggerType.PointerClick;
        

        public enum ButtonTriggerType
        {
            PointerClick = EventTriggerType.PointerClick,
            PointerEnter = EventTriggerType.PointerEnter,
            PointerExit = EventTriggerType.PointerExit,
            PointerDown = EventTriggerType.PointerDown,
            PointerUp = EventTriggerType.PointerUp,
        }
    }
}

