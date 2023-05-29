using System;

namespace Assets.Scripts
{
    public class Health
    {
        public event Action Changed;
        public event Action Dying;

        public float Value { get; private set; }
        public float MaxValue { get; private set; }

        public Health(float value)
        {
            Value = value;
            MaxValue = value;
        }

        public void RestoreToMax()
        {
            Value = MaxValue;
            Changed?.Invoke();
        }

        public void Decrease(float value)
        {
            if (value > Value)
                Value = 0;
            else
                Value -= value;
            Changed?.Invoke();

            if (Value == 0)
                Dying?.Invoke();
        }
    }
}
