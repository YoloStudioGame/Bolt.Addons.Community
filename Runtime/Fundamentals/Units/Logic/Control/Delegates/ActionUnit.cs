﻿using Bolt.Addons.Community.Utility;
using Ludiq;
using System;
using System.Collections.Generic;

namespace Bolt.Addons.Community.Fundamentals.Units.logic
{
    [UnitCategory("Community/Control")]
    public class ActionUnit : Unit
    {
        [Serialize]
        public IAction _action;

        [DoNotSerialize]
        public ValueOutput action;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput invoke;

        public List<ValueOutput> parameters = new List<ValueOutput>();

        public GraphReference reference;

        public ActionUnit() : base() { }

        public ActionUnit(IAction action)
        {
            _action = action;
        }

        protected override void Definition()
        {
            parameters.Clear();

            invoke = ControlOutput("invoke");

            if (_action != null)
            {
                action = ValueOutput(_action.GetActionType(), "action", (flow) =>
                {
                    reference = flow.stack.ToReference();
                    _action.Initialize(flow, this, () => { flow.Invoke(invoke); });
                    return _action.GetAction();
                });

                for (int i = 0; i < _action.parameters.Length; i++)
                {
                    parameters.Add(ValueOutput(_action.parameters[i].type, _action.parameters[i].name));
                }
            }
        }

        public void AssignPorts(Flow flow, params object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                flow.SetValue(this.parameters[0], parameters[i]);
            }
        }
    }
}
