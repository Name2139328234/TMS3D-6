using System;
using System.Collections.Generic;



public class StateMachine
{
    private StateNode _current;
    private Dictionary<Type, StateNode> _states = new();



    public void Update()
    {
        _current.State.Update();

        ITransition transition = default;
        foreach (var currentTransition in _current.Transitions)
            if (currentTransition.Condition.Evaluate())
                transition = currentTransition;

        if (transition == null)
            return;

        _current.State.OnExit();
        _current = _states[transition.To.GetType()];
        _current.State.OnEnter();
    }
    public void AddState(IState state)
    {
        _states.Add(state.GetType(), new StateNode(state));

        if (_current == null)
        {
            _current = _states[state.GetType()];
            _current.State.OnEnter();
        }
    }
    public void AddTransition(IState from, ITransition transition)
    {
        _states[from.GetType()].AddTransition(transition);
    }



    private class StateNode
    {
        public IState State { get => _state; }
        public IReadOnlyCollection<ITransition> Transitions { get => _transitions; }

        private IState _state;
        private HashSet<ITransition> _transitions = new();



        public StateNode(IState state)
        {
            _state = state;
        }
        public void AddTransition(ITransition transition)
        {
            _transitions.Add(transition);
        }
    }
}
