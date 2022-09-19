using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public abstract class PointerSelectable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected abstract void OnPointerClick();
        protected abstract void OnPointerEnter();
        protected abstract void OnPointerExit();

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnPointerClick();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnter();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            OnPointerExit();
        }
    }
}