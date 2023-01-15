using System.Collections.Generic;
using Common.Pausing;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Common.Physics
{
    public class RagdollActivator : MonoBehaviour, IPausable
    {
        [SerializeField] private List<Rigidbody> _rigidbodies;
        [SerializeField] private List<CharacterJoint> _characterJoints;

        private Animator _animator;
        private bool _isActive;
        private Pauser _pauser;


        [Inject]
        public void Construct(
            Animator animator,
            Pauser pauser)
        {
            _animator = animator;
            _pauser = pauser;
        }

        public void Start()
        {
            _pauser.Register(this);
        }

        public void Pause()
        {
            if (_isActive)
                DeactivateRagdollParts();
        }

        public void Resume()
        {
            if (_isActive)
                ActivateRagdollParts();
        }

        public void Activate()
        {
            if (_animator)
                _animator.enabled = false;
            ActivateRagdollParts();
            _isActive = true;
        }

        public void Deactivate()
        {
            DeactivateRagdollParts();
            if (_animator)
                _animator.enabled = true;
            _isActive = false;
        }

        private void ActivateRagdollParts()
        {
            foreach (var ragdollRigidbody in _rigidbodies)
            {
                ragdollRigidbody.isKinematic = false;
                ragdollRigidbody.useGravity = true;
            }

            if (!_isActive)
            {
                foreach (var characterJoint in _characterJoints)
                    characterJoint.autoConfigureConnectedAnchor = true;
            }
        }

        private void DeactivateRagdollParts()
        {
            foreach (var ragdollRigidbody in _rigidbodies)
            {
                ragdollRigidbody.useGravity = false;
                ragdollRigidbody.isKinematic = true;
            }

            foreach (var characterJoint in _characterJoints)
                characterJoint.autoConfigureConnectedAnchor = false;
        }

#if UNITY_EDITOR
        private void GatherRagdollParts()
        {
            _rigidbodies = new List<Rigidbody>();
            _characterJoints = new List<CharacterJoint>();

            var children = GetAllChildren(transform);

            foreach (var child in children)
            {
                if (child.TryGetComponent<Rigidbody>(out var rigidbody))
                    _rigidbodies.Add(rigidbody);

                if (child.TryGetComponent<CharacterJoint>(out var characterJoint))
                    _characterJoints.Add(characterJoint);
            }
        }

        private static List<Transform> GetAllChildren(Transform transform)
        {
            var children = new List<Transform>(transform.childCount);
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                children.Add(child);
                children.AddRange(GetAllChildren(child));
            }

            return children;
        }

        [CustomEditor(typeof(RagdollActivator))]
        public class RagdollActivatorEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                var script = (RagdollActivator)target;
                if (GUILayout.Button("Gather Ragdoll Parts"))
                    script.GatherRagdollParts();

                if (GUILayout.Button("Activate"))
                    script.Activate();

                if (GUILayout.Button("Deactivate"))
                    script.Deactivate();
            }
        }
#endif
    }
}