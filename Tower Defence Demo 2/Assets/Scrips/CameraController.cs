using UnityEngine;

namespace Scrips
{
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

            if (Input.touchCount == 1 /*&& Input.GetTouch(0).phase == TouchPhase.Moved*/)
            {
                var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * MobilePanSpeed * Time.deltaTime, -touchDeltaPosition.y * MobilePanSpeed * Time.deltaTime, 0);

                var tmpPosX = transform.position;
                tmpPosX.x = Mathf.Clamp(tmpPosX.x, LeftBorder, RightBorder);
                transform.position = tmpPosX;

                var tmpPosY = transform.position;
                tmpPosY.y = Mathf.Clamp(tmpPosY.y, LowerBorder , UpperBorder);
                transform.position = tmpPosY;
            }
            else if (Input.touchCount == 2)
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

                newOrtographicSize = transform.GetComponent<Camera>().orthographicSize + deltaMagnitudeDiff * MobileScrollSpeed;
                newOrtographicSize = Mathf.Clamp(newOrtographicSize, MinScrollDistance, MaxScrollDistance);
                
                transform.GetComponent<Camera>().orthographicSize = newOrtographicSize;
            }
        }
    }
}
