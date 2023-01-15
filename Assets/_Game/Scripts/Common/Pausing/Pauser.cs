using System;

namespace Common.Pausing
{
    public class Pauser : IDisposable
    {
        private bool _isPaused;
        private Action _paused;
        private Action _resumed;


        public void Dispose()
        {
            _paused = null;
            _resumed = null;
        }

        public void Pause()
        {
            if (!_isPaused)
            {
                _isPaused = true;
                _paused?.Invoke();
            }
        }

        public void Register(IPausable pausable)
        {
            _paused += pausable.Pause;
            _resumed += pausable.Resume;
        }

        public void Resume()
        {
            if (_isPaused)
            {
                _isPaused = false;
                _resumed?.Invoke();
            }
        }
    }
}