using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private Vector3 Target;
        [SerializeField] private float speedLerp;
        [SerializeField] private Collider2D CardCollider;

        public int CardID { get; internal set; }

        abstract public void UpdateData(TCardData cardData);

        abstract public void SetRenderingOrder(int order);

        public Bounds Bounds => CardCollider.bounds;

        public bool IsPointOnCard(Vector2 point)
        {
            return CardCollider.OverlapPoint(point);
        }

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
