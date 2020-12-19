using ActorsECS.Core.Modules.Character.Components;
using Pixeye.Actors;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActorsECS.Core.Modules.Character.Processors
{
  internal sealed class ProcessorGatheringInput : Processor, ITick, InputActions.ICharacterControllerActions
  {
    private readonly Group<ComponentInput> _characters = default;

    private readonly InputActions _inputActions;
    private bool _interact;
    private bool _jump;
    private Vector2 _look;

    private Vector2 _movement;
    private bool _pause;
    private bool _reload;
    private bool _roll;
    private float _shoot;

    public ProcessorGatheringInput()
    {
      _inputActions = new InputActions();
      _inputActions.CharacterController.SetCallbacks(this);
      _inputActions.Enable();
    }

    void InputActions.ICharacterControllerActions.OnMove(InputAction.CallbackContext context)
    {
      _movement = context.ReadValue<Vector2>();
    }

    void InputActions.ICharacterControllerActions.OnLook(InputAction.CallbackContext context)
    {
      _look = context.ReadValue<Vector2>();
    }

    void InputActions.ICharacterControllerActions.OnFire(InputAction.CallbackContext context)
    {
      _shoot = context.ReadValue<float>();
    }

    void InputActions.ICharacterControllerActions.OnJump(InputAction.CallbackContext context)
    {
      if (context.started) _jump = true;
      if (context.canceled) _jump = false;
    }

    void InputActions.ICharacterControllerActions.OnReload(InputAction.CallbackContext context)
    {
      if (context.started) _reload = true;
      if (context.canceled) _reload = false;
    }

    void InputActions.ICharacterControllerActions.OnInteract(InputAction.CallbackContext context)
    {
      if (context.started) _interact = true;
      if (context.canceled) _interact = false;
    }

    void InputActions.ICharacterControllerActions.OnRoll(InputAction.CallbackContext context)
    {
      if (context.started) _roll = true;
      if (context.canceled) _roll = false;
    }

    void InputActions.ICharacterControllerActions.OnPause(InputAction.CallbackContext context)
    {
      if (context.started) _pause = true;
      if (context.canceled) _pause = false;
    }

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();

        cInput.Movement.x = Accelerate(_movement.x, cInput.Movement.x, delta, 0.1f, 0.05f);
        cInput.Movement.y = Accelerate(_movement.y, cInput.Movement.y, delta, 0.1f, 0.05f);

        cInput.Look = _look;
        
        cInput.Interact = _interact;
        cInput.Shoot = _shoot;
        cInput.Reload = _reload;
        cInput.Roll = _roll;
        cInput.Pause = _pause;
      }

      _reload = false;
      _interact = false;
      _roll = false;
      _pause = false;
    }

    protected override void OnDispose()
    {
      _inputActions.Disable();
    }

    private float Accelerate(float actualInput, float acceleratedInput, float deltaTime, float acceleration,
      float deceleration)
    {
      // If the acceleration and deceleration values are negligible,
      // or a negative number, then no calculations need be done.
      if (acceleration < math.EPSILON && deceleration < math.EPSILON) return actualInput;

      if (Mathf.Abs(actualInput) < math.EPSILON
          || Mathf.Sign(acceleratedInput) == Mathf.Sign(actualInput)
          && Mathf.Abs(actualInput) < Mathf.Abs(acceleratedInput))
      {
        // Need to decelerate
        var a = Mathf.Abs(actualInput - acceleratedInput) / Mathf.Max(math.EPSILON, deceleration);
        var delta = Mathf.Min(a * deltaTime, Mathf.Abs(acceleratedInput));
        acceleratedInput -= Mathf.Sign(acceleratedInput) * delta;

        if (Mathf.Abs(acceleratedInput) < math.EPSILON) acceleratedInput = 0.0f;
      }
      else
      {
        // Need to accelerate
        var a = Mathf.Abs(actualInput - acceleratedInput) / Mathf.Max(math.EPSILON, acceleration);
        acceleratedInput += Mathf.Sign(actualInput) * a * deltaTime;
        if (Mathf.Sign(acceleratedInput) == Mathf.Sign(actualInput)
            && Mathf.Abs(acceleratedInput) > Mathf.Abs(actualInput))
          acceleratedInput = actualInput;
      }

      return acceleratedInput;
    }
  }
}