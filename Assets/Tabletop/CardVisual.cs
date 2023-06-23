using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private Vector3 Target;
        [SerializeField] private float speedLerp;

        public void UpdateData(Card<TCardData>.CardInstance NewCardInstance)
        {
            UpdateInternData(NewCardInstance.data);
        }

        abstract protected void UpdateInternData(TCardData cardData);

        abstract public void SetRenderingOrder(int order);

        public virtual void MoveTo(Vector3 Target)
        {
            this.Target = Target;
        }

        protected virtual void Update()
        {
            transform.position = Vector3.Lerp(transform.position, Target, speedLerp * Time.deltaTime);
        }
    }
}
