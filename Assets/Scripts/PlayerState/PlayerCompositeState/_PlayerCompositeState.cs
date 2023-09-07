using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PlayerState.PlayerCompositeState
{
    public class _PlayerCompositeState : _PlayerState
    {
        public _PlayerCompositeState(PlayerMovement player) : base(player) { }

        public List<_PlayerState> _PlayerStates = new List<_PlayerState>();

        public void AddState(_PlayerState state)
        {
            _PlayerStates.Add(state);
            state.Enter();
        }

        public void RemoveState(_PlayerState state)
        {
            state.Exit();
            //_PlayerState temp =_PlayerStates.FirstOrDefault(t => t.GetType().Equals(state.GetType()));
            _PlayerStates.Remove(state);
            
        }



        //查找复合状态中是否存在某个状态
        public bool searchState(_PlayerState state)
        {
            _PlayerState temp = _PlayerStates.FirstOrDefault(t => t.GetType().Equals(state.GetType()));
            if(temp != null)
            {
                return true;
            }
            return false;
        }

        public bool searchState(Type state)
        {
            _PlayerState temp = _PlayerStates.FirstOrDefault(t => t.GetType().Equals(state));
            if (temp != null)
            {
                return true;
            }
            return false;
        }

        public _PlayerState GetState(Type state)
        {
            _PlayerState temp = _PlayerStates.FirstOrDefault(t => t.GetType().Equals(state));
            return temp;
        }

        public new void Enter()
        {
            foreach (var state in _PlayerStates)
            {
                state.Enter();
            }
        }

        public new void Update()
        {
            if( _PlayerStates.Count > 0 )
            {
                for (int i = this._PlayerStates.Count - 1; i >= 0; i--)
                {
                    _PlayerStates[i].Update();
                }
            }
        }

        public new void Exit()
        {
            foreach(var state in _PlayerStates)
            {
                state.Exit();
            }
        }

        public String _ToString()
        {
            String s = "";
            foreach(_PlayerState state in _PlayerStates)
            {
                s += state.ToString() + " ";
            }
            return s;
        }



    }
}