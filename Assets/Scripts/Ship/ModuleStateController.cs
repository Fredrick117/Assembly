using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleStateController : MonoBehaviour
{
    public IState CurrentState { get; private set; }

    public PlacingState PlacingState     { get; private set; }
    public SnappedState SnappedState     { get; private set; }
    public ConnectedState ConnectedState { get; private set; }

    private bool isMouseOver = false;

    private void Awake()
    {
        PlacingState   = new();
        SnappedState   = new();
        ConnectedState = new();
    }

    private void Update()
    {
        ModuleInput input = new ModuleInput()
        {
            isLeftMouseDown = Input.GetMouseButtonDown(0),
            isLeftMouseUp = Input.GetMouseButtonUp(0),
            isRightMouseDown = Input.GetMouseButtonDown(1),
            isRightMouseUp = Input.GetMouseButtonUp(1),
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition),
            isMouseOver = this.isMouseOver
        };

        if (CurrentState != null)
        {
            CurrentState.HandleInput(this, input);
            CurrentState.UpdateState(this);
        }
    }

    private void OnMouseEnter() => isMouseOver = true;
    private void OnMouseExit() => isMouseOver = false;

    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit(this);
        }

        CurrentState = newState;
        CurrentState.OnEnter(this);
    }
}

public interface IState
{
    public void OnEnter(ModuleStateController stateController);
    public void UpdateState(ModuleStateController stateController);
    public void OnExit(ModuleStateController stateController);
    public void HandleInput(ModuleStateController stateController, ModuleInput input);
}