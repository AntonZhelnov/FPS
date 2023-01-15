using UnityEngine.AI;
using Zenject;

namespace Common.Pausing
{
    public class NavMeshAgentPauser : IInitializable, IPausable
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Pauser _pauser;


        public NavMeshAgentPauser(
            NavMeshAgent navMeshAgent,
            Pauser pauser)
        {
            _navMeshAgent = navMeshAgent;
            _pauser = pauser;
        }

        public void Initialize()
        {
            _pauser.Register(this);
        }

        public void Pause()
        {
            _navMeshAgent.enabled = false;
        }

        public void Resume()
        {
            _navMeshAgent.enabled = true;
        }
    }
}