using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class CardZoomHandler : InteractionHandler
    {
        public float Margin;
        public float HoverTimer;
        public float ZoomFactor;
        public bool ShowOnRight;
        public SortingLayerPicker displayedLayer;

        private CardVisual<StandardCardData> CurrentVisual;
        private bool locked;
        private bool lockLocker; // the lock on the lock ... when true can't change the value of locked
        private float timer;

        private void Update()
        {
            if (Input.GetMouseButton(0))
                DestroyVisual();
        }

        public override void OnCardInteract(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (!data.isHovered() && !locked)
            {
                DestroyVisual();
                return;
            }

            if (timer > HoverTimer)
                CreateVisual(data);
            else if (data.Target != null)
                timer += Time.deltaTime;

            HandleLock(data);
        }

        private void CreateVisual(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (CurrentVisual)
                return;

            var instance = data.CardManager.GetCardInstance(data.Target.CardID).Value;
            var card = data.CardManager.CardPool.GetCard(instance.CardModelID);
            CurrentVisual = card.CreateVisual(instance);
            (CurrentVisual as CardVisual).canvas.sortingLayerID = displayedLayer.id;
            CurrentVisual.enabled = false;
            CurrentVisual.transform.parent = transform;
            CurrentVisual.transform.localScale *= ZoomFactor;
            CurrentVisual.transform.position = FindPos(data.Target);
        }

        private void HandleLock(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (data.RightClick)
            {
                if (!lockLocker)
                    locked = !locked && data.isHovered();
                lockLocker = true;
                CreateVisual(data);
            }
            else
            {
                lockLocker = false;
            }
        }

        private void DestroyVisual()
        {
            timer = 0;
            locked = false;
            if (CurrentVisual)
            {
                Destroy(CurrentVisual.gameObject);
                CurrentVisual = null;
            }
        }

        Vector3 FindPos(CardVisual<StandardCardData> original)
        {
            var camera = Camera.main;
            var screenHalfSize = new Vector3(camera.orthographicSize * camera.aspect, camera.orthographicSize);

            //The two corner of the screen in units
            var cameraLeftBottom = camera.transform.position - screenHalfSize;
            var cameraTopRight = camera.transform.position + screenHalfSize;

            var HalfSize = CurrentVisual.Bounds.size / 2;
            var halfBigSize = HalfSize * ZoomFactor;
            Vector3 dir = ShowOnRight ? Vector3.right : Vector3.left;
            var targetPos = original.transform.position + dir * (HalfSize.x + halfBigSize.x + Margin);

            //if no room on one side, try the other
            if(targetPos.x + halfBigSize.x > cameraTopRight.x || targetPos.x - halfBigSize.x < cameraLeftBottom.x)
                targetPos = original.transform.position - dir * (HalfSize.x + halfBigSize.x + Margin); 

            //if too low or too high, adjust position
            if (targetPos.y - halfBigSize.y < cameraLeftBottom.y)
                    targetPos.y = cameraLeftBottom.y + halfBigSize.y;
            if (targetPos.y + halfBigSize.y > cameraTopRight.y)
                targetPos.y = cameraTopRight.y - halfBigSize.y;

            return targetPos;
        }
    }
}
