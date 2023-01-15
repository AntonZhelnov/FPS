using Gameplay.Damaging;
using UnityEngine;

namespace Gameplay.Weapons
{
    public class WeaponHolder
    {
        private readonly DamageGroup _damageGroup;
        private readonly Transform _weaponHolderTransform;

        protected Weapon Weapon;


        protected WeaponHolder(
            DamageGroup damageGroup,
            Transform weaponHolderTransform)
        {
            _damageGroup = damageGroup;
            _weaponHolderTransform = weaponHolderTransform;
        }

        public virtual void Equip(Weapon weapon)
        {
            if (Weapon)
                Weapon.Recycle();

            Weapon = weapon;
            Weapon.Equip(
                _damageGroup,
                _weaponHolderTransform);
        }

        public void Use(Vector3 targetPosition)
        {
            Weapon.Attack(targetPosition);
        }
    }
}