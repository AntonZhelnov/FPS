using System;
using Common;

namespace Gameplay.Weapons.Factories
{
    public class WeaponFactory
    {
        private readonly Gun.Factory _gunFactory;
        private readonly Knife.Factory _knifeFactory;


        public WeaponFactory(
            Knife.Factory knifeFactory,
            Gun.Factory gunFactory)
        {
            _knifeFactory = knifeFactory;
            _gunFactory = gunFactory;
        }

        public Weapon Create(
            Type weaponType,
            DamageGroup damageGroup)
        {
            if (weaponType == typeof(Knife))
                return _knifeFactory.Create(damageGroup);

            if (weaponType == typeof(Gun))
                return _gunFactory.Create(damageGroup);

            return null;
        }
    }
}