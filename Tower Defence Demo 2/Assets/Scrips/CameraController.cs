using UnityEngine;

namespace Scrips
{
    public class CameraController : MonoBehaviour
    {
        public float PanSpeed = 30;
        public float PanBorderThickness = 10;

        public float ScrollSpeed = 5;

        public float MinScrollDistance = 5;
        public float MaxScrollDistance = 30;

        public float LeftBorder = -15;
        public float RightBorder = 15;
        public float UpperBorder = 10;
        public float LowerBorder = -10;


        // Update is called once per frame
        private void Update ()
        {
            if (Input.GetKey("w"))
            {
                if (transform.position.y < UpperBorder)
                    transform.Translate(Vector3.up * PanSpeed * Time.deltaTime);
            }

            if (Input.GetKey("s"))
            {
                if (transform.position.y > LowerBorder)
                    transform.Translate(Vector3.down * PanSpeed * Time.deltaTime);
            }

            if (Input.GetKey("a"))
            {
                if (transform.position.x > LeftBorder)
                    transform.Translate(Vector3.left * PanSpeed * Time.deltaTime);
            }

            if (Input.GetKey("d"))
            {
                if (transform.position.x < RightBorder)
                    transform.Translate(Vector3.right * PanSpeed * Time.deltaTime);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            float newOrtographicSize = transform.GetComponent<Camera>().orthographicSize - scroll * ScrollSpeed;
            newOrtographicSize = Mathf.Clamp(newOrtographicSize, MinScrollDistance, MaxScrollDistance);

            transform.GetComponent<Camera>().orthographicSize = newOrtographicSize;
        }
    }
}
