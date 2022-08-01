using Gameplay.Weapons.Shooting;
using UnityEngine;
using Zenject;

namespace Configs.Weapons
{
    [CreateAssetMenu(
        fileName = "New Bullet Config Installer",
        menuName = "Configs/Weapons/Bullet Config Installer")]
    public class BulletConfig : ScriptableObjectInstaller
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] [Min(0.1f)] private float _bulletMovementSpeed = 10f;
        [SerializeField] [Min(0.1f)] private float _bulletExpirationTime = 5f;

        public Bullet BulletPrefab => _bulletPrefab;
        public float BulletMovementSpeed => _bulletMovementSpeed;
        public float BulletExpirationTime => _bulletExpirationTime;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}