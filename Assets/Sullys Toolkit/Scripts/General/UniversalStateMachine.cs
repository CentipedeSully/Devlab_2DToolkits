using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    [System.Serializable]
    public class UniversalStateMachine : MonoBehaviour
    {
        private string _name;
        private Dictionary<string, State> _stateDictionary;

        private void Awake()
        {
            _stateDictionary = new Dictionary<string, State>();
        }

        public void LogDictionary()
        {
            Debug.Log(_stateDictionary);
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public int StateCount()
        {
            return _stateDictionary.Count;
        }

        public override string ToString()
        {
            return _name;
        }

        public UniversalStateMachine(string name)
        {
            _name = name;
        }

        public virtual void AddState(string stateName)
        {
            if (!DoesStateExist(stateName) && stateName != "" && stateName != null)
            {
                State newState = new State(stateName);
                _stateDictionary.Add(stateName, newState);
            }

        }

        public virtual void RemoveState(string stateName)
        {
            if (DoesStateExist(stateName))
            {
                _stateDictionary.Remove(stateName);
            }
        }

        public virtual bool DoesStateExist(string stateName)
        {
            return _stateDictionary.ContainsKey(stateName);
        }

        public virtual bool GetStateActivity(string stateName)
        {
            if (DoesStateExist(stateName))
                return _stateDictionary[stateName].IsStateActive();
            else
            {
                //Debug.LogError("Attempted to request activity of nonexistent State(" + stateName + ") on object: " + gameObject.name);
                return false;
            }

        }

        public virtual void UpdateStateActivity(string stateName, bool newValue)
        {
            if (DoesStateExist(stateName))
                _stateDictionary[stateName].SetStateActivity(newValue);
            //else Debug.LogError("Attempted to update activity of nonexistent State(" + stateName + ") on object: " + gameObject.name);
        }

        public virtual List<string> GetAllStateNames()
        {
            List<string> nameList = new List<string>();
            if (_stateDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, State> entry in _stateDictionary)
                {
                    nameList.Add(entry.Key);
                }
            }

            return nameList;
        }

        public virtual void LogAllStates()
        {
            foreach (KeyValuePair<string, State> entry in _stateDictionary)
            {
                Debug.Log(entry);
            }
        }
    }

    [System.Serializable]
    public class State
    {
        private string _name;

        private bool _isStateActive = false;


        public State(string name)
        {
            this._name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public bool IsStateActive()
        {
            return _isStateActive;
        }

        public void SetStateActivity(bool value)
        {
            _isStateActive = value;
        }

        public override string ToString()
        {
            return "(" + _name + ":" + _isStateActive + ")";

        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() * 2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;
            else return ((State)obj).GetName() == _name;
        }

        public static bool operator ==(State state1, State state2)
        {
            if (state1?.GetName() == null && state2?.GetName() == null)
                return true;
            else if (state1?.GetName() == null || state2?.GetName() == null)
                return false;
            else return state1.GetName() == state2.GetName();
        }

        public static bool operator !=(State state1, State state2)
        {
            return !(state1 == state2);
        }
    }

}
