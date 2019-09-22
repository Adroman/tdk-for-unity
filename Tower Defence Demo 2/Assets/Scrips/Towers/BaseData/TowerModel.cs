using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.UI.UiTextDisplay;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    public class TowerModel : MonoBehaviour
    {
        public Transform RotationPoint;
        public Vector3 RotationDirection;
        public Vector3 InitialRotationOffset;
        private bool _hasRotatingPoint;

        public Transform ShootingPoint;
        private bool _hasShootingPoint;

        public BaseUiTextDisplay TextDisplay;
        private bool _hasTextDisplay;

        private void OnEnable()
        {
            _hasRotatingPoint = RotationPoint != null;
            _hasShootingPoint = ShootingPoint != null;
            _hasTextDisplay = TextDisplay != null;
        }

        public void RotateTurret(IEnumerable<Transform> targets)
        {
            if (!_hasRotatingPoint) return; // do not rotate

            var vectorsToTarget = targets.Select(t => t.position - transform.position).ToArray();

            var vectorToTarget = new Vector3(vectorsToTarget.Average(v => v.x), vectorsToTarget.Average(v => v.y));
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            var q = Quaternion.AngleAxis(-angle, RotationDirection);
            RotationPoint.localRotation = Quaternion.Slerp(RotationPoint.rotation, q, 10000 * Time.deltaTime);
        }

        public Transform GetShootingPoint()
        {
            return !_hasShootingPoint ? transform : ShootingPoint;
        }

        public void SetDisplay(string textToDisplay)
        {
            if (_hasTextDisplay)
            {
                TextDisplay.Display(textToDisplay);
            }
        }
    }
}