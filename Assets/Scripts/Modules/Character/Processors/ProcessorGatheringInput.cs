using Modules.Character.Components;
using Pixeye.Actors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Character.Processors
{
  internal sealed class ProcessorGatheringInput : Processor, ITick, InputActions.ICharacterControllerActions
  {
    private readonly Group<ComponentInput> _characters = default;

    private readonly InputActions _inputActions;

    private Vector2 _characterMovement;
    private Vector2 _characterLooking;
    private float _characterFiring;
    private bool _characterJumped;
    
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

    void InputActions.ICharacterControllerActions.OnMove(InputAction.CallbackContext context) => _characterMovement = context.ReadValue<Vector2>();
    void InputActions.ICharacterControllerActions.OnLook(InputAction.CallbackContext context) => _characterLooking = context.ReadValue<Vector2>();
    void InputActions.ICharacterControllerActions.OnFire(InputAction.CallbackContext context) => _characterFiring = context.ReadValue<float>();
    void InputActions.ICharacterControllerActions.OnJump(InputAction.CallbackContext context) { if (context.started) _characterJumped = true; }

    public void Tick(float delta)
    {
      foreach (var character in _characters)
      {
        ref var cinput = ref character.ComponentInput();
        
        cinput.movement = _characterMovement;
      }
    }
  }
}
