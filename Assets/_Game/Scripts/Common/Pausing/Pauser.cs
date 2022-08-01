using System.Collections.Generic;

namespace Common.Pausing
{
    public class Pauser
    {
        private readonly List<IPausable> _pausables = new();
        private bool _isPaused;


        public void Pause()
        {
            if (!_isPaused)
            {
                _isPaused = true;
                for (var i = 0; i < _pausables.Count; i++)
                    _pausables[i].Pause();
            }
        }

        public void Register(IPausable pausable)
        {
            _pausables.Add(pausable);
        }

        public void Resume()
        {
            if (_isPaused)
            {
                _isPaused = false;
                for (var i = 0; i < _pausables.Count; i++)
                    _pausables[i].Resume();
            }
        }

        public void Unregister(IPausable pausable)
        {
            _pausables.Remove(pausable);
        }
    }
}