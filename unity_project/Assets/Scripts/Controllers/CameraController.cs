using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    /// <summary>
    ///  Controller class for the camera rig in the game. Allowing for camera translation, rotation, and zoom through different means.
    ///
    /// It is based on the video by Game Dev Guide: https://youtu.be/rnqF6S7PfFA
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Translation")] [SerializeField]
        private float normalSpeed = 0.3f;

        [SerializeField] private float fastSpeed = 0.8f;
        [SerializeField] private float movementTime = 10f;

        [Header("Rotation")] [SerializeField] private float dragRotationSpeed = 1f;
        [SerializeField] private float rotationSpeed = 2.5f;

        [Header("Zoom")] [SerializeField] private Vector3 zoomAmount = Vector3.down;
        [SerializeField] private float minHeight = 2f;
        [SerializeField] private float maxHeight = 80f;

        private Vector3 targetPosition;
        private Vector3 targetZoom;
        private Transform cameraTransform;

        private ControlScheme.CameraActions controlScheme;
        private Camera rigCamera;
        private Vector3 dragTargetStart;
        private Vector3 dragTargetCurrent;

        private void Awake()
        {
            controlScheme = new ControlScheme().Camera;
            rigCamera = GetComponentInChildren<Camera>();
            cameraTransform = rigCamera.transform;
            targetPosition = transform.position;
            targetZoom = cameraTransform.localPosition;
        }

        private void OnEnable()
        {
            controlScheme.Enable();
            controlScheme.DragRotation.performed += OnDragRotate;
            controlScheme.Zoom.performed += OnCameraZoom;
            controlScheme.DragMovement.started += OnCameraDragStarted;
            controlScheme.DragMovement.performed += OnCameraDrag;
            cameraTransform.LookAt(transform);
        }

        private void OnDisable()
        {
            controlScheme.DragRotation.performed -= OnDragRotate;
            controlScheme.Zoom.performed -= OnCameraZoom;
            controlScheme.DragMovement.started -= OnCameraDragStarted;
            controlScheme.DragMovement.performed -= OnCameraDrag;
            controlScheme.Disable();
        }

        private void Update()
        {
            UpdateTargetPosition();
            UpdateCameraRotation();
            UpdateRigTransform();
        }

        private void UpdateTargetPosition()
        {
            Vector3 cameraRight = cameraTransform.right;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 input = controlScheme.Movement.ReadValue<Vector2>().x * cameraRight +
                            controlScheme.Movement.ReadValue<Vector2>().y * cameraForward;
            input.y = 0;

            float maxSpeed = controlScheme.FastMovement.IsPressed() ? fastSpeed : normalSpeed;
            targetPosition += input.normalized * maxSpeed;
        }

        private void UpdateCameraRotation()
        {
            float input = controlScheme.Rotation.ReadValue<float>();
            if (Mathf.Abs(input) > 0.1f)
            {
                float rotation = input * rotationSpeed + transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(Vector3.up * rotation);
            }
        }

        private void UpdateRigTransform()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movementTime);
            cameraTransform.localPosition =
                Vector3.Lerp(cameraTransform.localPosition, targetZoom, Time.deltaTime * movementTime);
            cameraTransform.LookAt(transform);
        }

        private void OnDragRotate(InputAction.CallbackContext inputEvent)
        {
            float input = inputEvent.ReadValue<Vector2>().x;
            float rotation = input * dragRotationSpeed + transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(Vector3.up * rotation);
        }

        private void OnCameraZoom(InputAction.CallbackContext inputEvent)
        {
            float input = -inputEvent.ReadValue<Vector2>().y / 100f;
            Vector3 target = targetZoom + input * zoomAmount;
            if (target.y > minHeight && target.y < maxHeight) targetZoom = target;
        }

        private void OnCameraDragStarted(InputAction.CallbackContext inputEvent)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = rigCamera.ScreenPointToRay(inputEvent.ReadValue<Vector2>());

            if (plane.Raycast(ray, out float distance)) dragTargetStart = ray.GetPoint(distance);
        }

        private void OnCameraDrag(InputAction.CallbackContext inputEvent)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = rigCamera.ScreenPointToRay(inputEvent.ReadValue<Vector2>());

            if (plane.Raycast(ray, out float distance))
            {
                dragTargetCurrent = ray.GetPoint(distance);
                targetPosition = transform.position + dragTargetStart - dragTargetCurrent;
            }
        }
    }
}