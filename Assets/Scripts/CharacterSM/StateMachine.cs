using System.Collections.Generic;
using System.Linq;

namespace CharacterSM
{
    public abstract class StateMachine : IStateSwitcher
    {
        public abstract void Start<T>() where T : BaseState;
        public abstract void SwitchState<T>() where T : BaseState;

        public virtual void FixedUpdate(float time) { }

        public virtual void Finish() { }
    }
}