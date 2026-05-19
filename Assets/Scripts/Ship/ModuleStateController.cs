using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleStateController : MonoBehaviour
{
    public IState CurrentState { get; private set; }

    public PlacingState PlacingState     { get; private set; }
    public SnappedState SnappedState     { get; private set; }
    public ConnectedState ConnectedState { get; private set; }

    private void Awake()
    {
        PlacingState   = new();
        SnappedState   = new();
        ConnectedState = new();
    }

    private void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.UpdateState(this);
        }
    }
    
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