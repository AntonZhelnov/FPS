using UnityEngine;

namespace Gameplay.Weapons
{
    public class RandomWeaponProvider
    {
        private readonly Gun.Gun.Factory _gunFactory;
        private readonly Knife.Knife.Factory _knifeFactory;


        public RandomWeaponProvider(
            Knife.Knife.Factory knifeFactory,
            Gun.Gun.Factory gunFactory)
        {
            _knifeFactory = knifeFactory;
            _gunFactory = gunFactory;
        }

        public void GiveWeapon(IWeaponUser weaponUser)
        {
            if (Random.Range(0f, 1f) > .5f)
                weaponUser.EquipWeapon(_gunFactory.Create());
            else
                weaponUser.EquipWeapon(_knifeFactory.Create());
        }
    }
}