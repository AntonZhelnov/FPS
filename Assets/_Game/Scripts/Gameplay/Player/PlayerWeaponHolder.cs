using Common.Extensions;
using Gameplay.Damaging;
using Gameplay.Weapons;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerWeaponHolder : WeaponHolder
    {
        private readonly int _weaponHolderLayer;


        public PlayerWeaponHolder(
            DamageGroup damageGroup,
            Transform weaponHolderTransform)
            : base(
                damageGroup,
                weaponHolderTransform)
        {
            _weaponHolderLayer = weaponHolderTransform.gameObject.layer;
        }

        public override void Equip(Weapon weapon)
        {
            weapon.transform.SetLayer(_weaponHolderLayer);
            base.Equip(weapon);
        }

        public void RecycleWeapon()
        {
            if (Weapon)
            {
                Weapon.transform.SetLayer(0);
                Weapon.Recycle();
            }
        }
    }
}