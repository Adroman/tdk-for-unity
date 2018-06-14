using System.Collections;
using UnityEngine;

namespace Scrips.UI
{
    public class UiWave : MonoBehaviour
    {
        public RectTransform Layout;

        public RectTransform PartToMove;

        private Vector3 _originalPosition;

        public void Spawn()
        {
            _originalPosition = PartToMove.localPosition;
            PartToMove.Translate(PartToMove.rect.width, 0, 0);
            StartCoroutine(Move(-1, PartToMove.rect.width, 1));
        }

        public void Despawn()
        {
            StartCoroutine(Move(1, PartToMove.rect.width, 1, true));
        }

        private IEnumerator Move(float speed, float distance, float lerpTime, bool destroy = false)
        {
            float startTime = Time.time;

            while (true)
            {
                float timePassed = Time.time - startTime;
                float percentage = timePassed / lerpTime;

                if (!destroy && percentage >= 1) break;

                PartToMove.Translate(speed * distance * Time.deltaTime, 0, 0);

                if (percentage >= 1) break;

                yield return new WaitForEndOfFrame();
            }

            if (destroy) Destroy(gameObject);
            else
            {
                PartToMove.localPosition = _originalPosition;
            }
        }
    }
}