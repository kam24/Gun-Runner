using System;

namespace PlayerMovement
{
    public class InputHandler
    {
        public Action OnUpKey { get; private set; }
        public Action OnDownKey { get; private set; }
        public Action OnLeftRightKey { get; private set; }

        public bool HasUpKeyHandler => OnUpKey != null;
        public bool HasDownKeyHandler => OnDownKey != null;
        public bool HasLeftRightKeyHandler => OnLeftRightKey != null;

        public void SetUpKeyHandler(Action handler)
        {
            OnUpKey = handler;
        }

        public void SetDownKeyHandler(Action handler)
        {
            OnDownKey = handler;
        }

        public void SetLeftRightKeyHandler(Action handler)
        {
            OnLeftRightKey = handler;
        }
    }
}