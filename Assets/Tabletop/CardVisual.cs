using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private Vector3 Target;
        [SerializeField] private float speedLerp;

        abstract public void UpdateData(TCardData cardData);

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
