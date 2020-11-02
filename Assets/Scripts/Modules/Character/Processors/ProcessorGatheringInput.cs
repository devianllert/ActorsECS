using ActorsECS.Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActorsECS.Modules.Character.Processors
{
  internal sealed class ProcessorGatheringInput : Processor, ITick, InputActions.ICharacterControllerActions
  {
    private readonly Group<ComponentInput> _characters = default;

    private readonly InputActions _inputActions;

    private Vector2 _movement;
    private Vector2 _look;
    private float _shoot;
    private bool _jump;
    private bool _reload;
    private bool _interact;

    public ProcessorGatheringInput()
    {
      _inputActions = new InputActions();
      _inputActions.CharacterController.SetCallbacks(this);
      _inputActions.Enable();
    }

    protected override void OnDispose()
    {
      _inputActions.Disable();
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

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cInput = ref character.ComponentInput();

        cInput.Movement = _movement;
        cInput.Interact = _interact;
        cInput.Shoot = _shoot;
        cInput.Reload = _reload;
      }

      _reload = false;
      _interact = false;
    }
  }
}