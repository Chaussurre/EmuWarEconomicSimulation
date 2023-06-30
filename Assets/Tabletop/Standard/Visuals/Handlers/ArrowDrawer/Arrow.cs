using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class Arrow : MonoBehaviour
    {
        public Vector3 destination;
        public Vector3 from;

        [SerializeField] private GameObject ArrowHead;
        [SerializeField] private GameObject ArrowBody;
        [SerializeField] private float BodyDistance;

        public void Update()
        {
            var delta = destination - from;
            float angle = Vector3.SignedAngle(Vector3.up, delta, Vector3.forward);

            ArrowHead.transform.eulerAngles = new(0, 0, angle);
            ArrowBody.transform.eulerAngles = new(0, 0, angle);

            ArrowHead.transform.position = destination;
            ArrowBody.transform.position = from;

            float sizeRatio = ArrowBody.transform.lossyScale.y / ArrowBody.transform.localScale.y;

            var scale = ArrowBody.transform.localScale;
            scale.y = (delta.magnitude - BodyDistance) * sizeRatio;
            if (scale.y > 0)
            {
                ArrowBody.SetActive(true);
                ArrowBody.transform.localScale = scale;
            }
            else
                ArrowBody.SetActive(false);
        }
    }
}
