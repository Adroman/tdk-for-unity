using UnityEngine;

namespace Scrips
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public float PanSpeed = 30;
        public float ScrollSpeed = 5;
        public float PanBorderThickness = 10;

        public float MobilePanSpeed = 1;
        public float MobileScrollSpeed = 1;


        public float MinScrollDistance = 5;
        public float MaxScrollDistance = 30;

        public float LeftBorder = -15;
        public float RightBorder = 15;
        public float UpperBorder = 10;
        public float LowerBorder = -10;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        private void Update ()
        {
            if (Input.GetKey("w"))
            {
                if (transform.position.y < UpperBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.up);
            }

            if (Input.GetKey("s"))
            {
                if (transform.position.y > LowerBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.down);
            }

            if (Input.GetKey("a"))
            {
                if (transform.position.x > LeftBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.left);
            }

            if (Input.GetKey("d"))
            {
                if (transform.position.x < RightBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.right);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            float newOrthographicSize = _camera.orthographicSize - scroll * ScrollSpeed;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, MinScrollDistance, MaxScrollDistance);

            _camera.orthographicSize = newOrthographicSize;

            switch (Input.touchCount)
            {
                /*&& Input.GetTouch(0).phase == TouchPhase.Moved*/
                case 1:
                {
                    var transformLocal = transform;

                    var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    transformLocal.Translate(-touchDeltaPosition.x * MobilePanSpeed * Time.deltaTime, -touchDeltaPosition.y * MobilePanSpeed * Time.deltaTime, 0);

                    var position = transformLocal.position;
                    position.x = Mathf.Clamp(position.x, LeftBorder, RightBorder);
                    position.y = Mathf.Clamp(position.y, LowerBorder , UpperBorder);

                    transformLocal.position = position;
                    break;
                }

                case 2:
                {
                    var touchZero = Input.GetTouch(0);
                    var touchOne = Input.GetTouch(1);

                    // Find the position in the previous frame of each touch.
                    var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    newOrthographicSize = _camera.orthographicSize + deltaMagnitudeDiff * MobileScrollSpeed;
                    newOrthographicSize = Mathf.Clamp(newOrthographicSize, MinScrollDistance, MaxScrollDistance);

                    _camera.orthographicSize = newOrthographicSize;
                    break;
                }
            }
        }
    }
}
