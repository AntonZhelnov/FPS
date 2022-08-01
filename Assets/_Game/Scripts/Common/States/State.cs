using Zenject;

namespace Common.States
{
    public abstract class State : IInitializable
    {
        [Inject]
        public virtual void Initialize()
        {
        }

        public StateMachine StateMachine { get; set; }


        public abstract void Start();

        public abstract void Stop();

        public abstract void UpdateTime(float deltaTime);
    }
}