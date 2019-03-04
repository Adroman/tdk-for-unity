using UnityEngine;

namespace Scrips
{
    [RequireComponent(typeof(LineRenderer))]
    public class CircleRenderer : MonoBehaviour
    {
        public int Vertices;

        private LineRenderer _lineRenderer;

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void UpdateCircle(float radius)
        {
            if (gameObject.activeSelf)
            {
                float deltaTheta = 2 * Mathf.PI / Vertices;
                float theta = 0;

                _lineRenderer.positionCount = Vertices;
                _lineRenderer.loop = true;
                for (int i = 0; i < Vertices; i++)
                {
                    var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
                    _lineRenderer.SetPosition(i, pos);
                    theta += deltaTheta;
                }
            }
        }
    }
}