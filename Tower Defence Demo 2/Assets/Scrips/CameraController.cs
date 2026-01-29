using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scrips
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public float PanSpeed = 30;
        public float DragSpeed = 4;
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
        
        private bool _mouseDown;
        private Vector3? _lastMousePosition;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        private void Update ()
        {
            HandleKeyboardInputs();
            HandleMouseInputs();
            HandleTouchInputs();
        }

        void HandleKeyboardInputs()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.position.y < UpperBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.up);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (transform.position.y > LowerBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.down);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > LeftBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.left);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < RightBorder)
                    transform.Translate(PanSpeed * Time.deltaTime * Vector3.right);
            }
        }

        void HandleMouseInputs()
        {
            if (Input.GetMouseButton((int)MouseButton.RightMouse))
            {
                if (_lastMousePosition.HasValue)
                {
                    var diff = _lastMousePosition.Value - Input.mousePosition;
                    var translateVector = DragSpeed * Time.deltaTime * diff;
                    if ((transform.position.x < LeftBorder && translateVector.x < 0) 
                        || (transform.position.x > RightBorder && translateVector.x > 0))
                    {
                        translateVector.x = 0;
                    }

                    if ((transform.position.y < LowerBorder && translateVector.y < 0)
                        || (transform.position.y > UpperBorder && translateVector.y > 0))
                    {
                        translateVector.y = 0;
                    }
                    
                    transform.Translate(translateVector);
                }
                
                _lastMousePosition = Input.mousePosition;
            }
            else
            {
                _lastMousePosition = null;
            }
            
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            float newOrthographicSize = _camera.orthographicSize - scroll * ScrollSpeed;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, MinScrollDistance, MaxScrollDistance);

            _camera.orthographicSize = newOrthographicSize;
        }

        void HandleTouchInputs()
        {
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

                    float newOrthographicSize = _camera.orthographicSize + deltaMagnitudeDiff * MobileScrollSpeed;
                    newOrthographicSize = Mathf.Clamp(newOrthographicSize, MinScrollDistance, MaxScrollDistance);

                    _camera.orthographicSize = newOrthographicSize;
                    break;
                }
            }
        }
    }
}
