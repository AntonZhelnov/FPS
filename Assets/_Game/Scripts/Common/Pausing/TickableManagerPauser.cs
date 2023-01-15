using Zenject;

namespace Common.Pausing
{
    public class TickableManagerPauser : IInitializable, IPausable
    {
        private readonly Pauser _pauser;
        private readonly TickableManager _tickableManager;


        public TickableManagerPauser(
            TickableManager tickableManager,
            Pauser pauser)
        {
            _tickableManager = tickableManager;
            _pauser = pauser;
        }

        public void Initialize()
        {
            _pauser.Register(this);
        }

        public void Pause()
        {
            _tickableManager.IsPaused = true;
        }

        public void Resume()
        {
            _tickableManager.IsPaused = false;
        }
    }
}