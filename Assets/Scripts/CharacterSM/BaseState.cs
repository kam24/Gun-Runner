namespace CharacterSM
{
    public abstract class BaseState
    {
        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void FixedUpdate(float time) { }
    }
}