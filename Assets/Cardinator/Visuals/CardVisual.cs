using UnityEngine;

namespace Cardinator
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private Vector3 Target;
        private Vector3? TargetVisualBody;
        [SerializeField] private float speedLerp;
        [SerializeField] private Collider2D CardCollider;
        [SerializeField] private GameObject visualBody;
        public bool isHidden { get; internal set; }

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
        
        public void MoveWithoutColliderTo(Vector3? Target)
        {
            if (!Target.HasValue)
            {
                TargetVisualBody = null;
                return;
            }

            TargetVisualBody = Target.Value - transform.position;
        }

        protected virtual void Update()
        {
            var lerp = speedLerp * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, Target, lerp);

            visualBody.transform.localPosition = Vector3.Lerp(visualBody.transform.localPosition, TargetVisualBody ?? Vector3.zero, lerp);
        }
    }
}
