using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
      [SerializeField] float turningSpeed = 5f;
      [SerializeField] float speed = 2f;
      [SerializeField] Animator animator;
      CharacterController controller;
      Vector2 initialTouchPos;
      Vector3 direction;

      private void Start()
      {
            controller = GetComponent<CharacterController>();
            direction = Vector3.zero;
            Application.targetFrameRate = 60;
      }


      void FixedUpdate()
      {
            HandleInput();
      }

      void HandleInput()
      {
            if (Touchscreen.current != null)
            {
                  var touch = Touchscreen.current.primaryTouch;
                  AnimatorClipInfo cInfo;
                  switch (touch.phase.ReadValue())
                  {
                        case UnityEngine.InputSystem.TouchPhase.Began:
                              this.initialTouchPos = touch.position.ReadValue();
                              break;
                        case UnityEngine.InputSystem.TouchPhase.Moved:
                              var currentTouchPosition = touch.position.ReadValue();

                              direction = (currentTouchPosition - initialTouchPos).normalized;
                              direction.z = direction.y;
                              direction.y = 0f;

                              cInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
                              if (cInfo.clip.name != "Running" && !animator.IsInTransition(0))
                              {
                                    animator.SetTrigger("Running");
                              }

                              Vector3 worldDirection = transform.position + (direction * turningSpeed);
                              transform.LookAt(worldDirection);

                              if (!animator.IsInTransition(0))
                              {
                                    controller.SimpleMove(direction * speed);
                              }

                              break;
                        case UnityEngine.InputSystem.TouchPhase.Ended:
                              cInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
                              if (cInfo.clip.name != "DynIdle" && !animator.IsInTransition(0))
                                    animator.SetTrigger("DynIdle");
                              break;
                  }

                  if (!controller.isGrounded)
                  {
                        controller.Move(Physics.gravity * Time.deltaTime);
                  }
            }
      }
}
