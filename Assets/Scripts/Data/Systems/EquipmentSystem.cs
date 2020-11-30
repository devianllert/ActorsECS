using System;
using ActorsECS.Data.Items;
using ActorsECS.Modules.Character;
using Pixeye.Actors;

namespace ActorsECS.Data.Systems
{
    /// <summary>
    ///     Handles the equipment stored inside an instance of CharacterData. Will take care of unequipping the previous
    ///     item when equipping a new one in the same slot.
    /// </summary>
    [Serializable]
    public class EquipmentSystem
    {
        public WeaponItem Weapon { get; private set; }

        public Action<EquipmentItem> OnEquiped { get; set; }
        public Action<EquipmentItem> OnUnequip { get; set; }

        private ent _owner;
        
        private EquipmentItem _headSlot;
        private EquipmentItem _torsoSlot;
        private EquipmentItem _legsSlot;
        private EquipmentItem _feetSlot;
        private EquipmentItem _accessorySlot;

        private WeaponItem _defaultWeapon;

        public void Init(ent owner)
        {
            _owner = owner;
        }
        
        public void InitWeapon(WeaponItem weapon, ent data)
        {
            _defaultWeapon = weapon;
        }
    
        public EquipmentItem GetItem(EquipmentItem.EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                    return _headSlot;
                case EquipmentItem.EquipmentSlot.Torso:
                    return _torsoSlot;
                case EquipmentItem.EquipmentSlot.Legs:
                    return _legsSlot;
                case EquipmentItem.EquipmentSlot.Feet:
                    return _feetSlot;
                case EquipmentItem.EquipmentSlot.Accessory:
                    return _accessorySlot;
                case EquipmentItem.EquipmentSlot.Weapon:
                    return Weapon;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Equip the given item for the given user. This won't check about requirement, this should be done by the
        /// inventory before calling equip!
        /// </summary>
        /// <param name="item">Which item to equip</param>
        public void Equip(EquipmentItem item)
        {
            OnEquiped?.Invoke(item);

            switch (item.slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                {
                    _headSlot = item;
                    _headSlot.EquippedBy(_owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Torso:
                {
                    _torsoSlot = item;
                    _torsoSlot.EquippedBy(_owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Legs:
                {
                    _legsSlot = item;
                    _legsSlot.EquippedBy(_owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Feet:
                {
                    _feetSlot = item;
                    _feetSlot.EquippedBy(_owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Accessory:
                {
                    _accessorySlot = item;
                    _accessorySlot.EquippedBy(_owner);
                }
                    break;
                case EquipmentItem.EquipmentSlot.Weapon:
                    var weaponController = _owner.GetMono<CharacterWeaponController>();
                    weaponController.SetupWeaponModel(item as WeaponItem);
                    
                    Weapon = item as WeaponItem;
                    Weapon.EquippedBy(_owner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Unequip the item in the given slot.
        /// </summary>
        /// <param name="slot"></param>
        public void Unequip(EquipmentItem.EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentItem.EquipmentSlot.Head:
                    if (_headSlot != null)
                    {
                        _headSlot.UnequippedBy(_owner);
                        // m_Owner.Inventory.AddItem(m_HeadSlot);
                        OnUnequip?.Invoke(_headSlot);
                        _headSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Torso:
                    if (_torsoSlot != null)
                    {
                        _torsoSlot.UnequippedBy(_owner);
                        // m_Owner.Inventory.AddItem(m_TorsoSlot);
                        OnUnequip?.Invoke(_torsoSlot);
                        _torsoSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Legs:
                    if (_legsSlot != null)
                    {
                        _legsSlot.UnequippedBy(_owner);
                        // m_Owner.Inventory.AddItem(m_LegsSlot);
                        OnUnequip?.Invoke(_legsSlot);
                        _legsSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Feet:
                    if (_feetSlot != null)
                    {
                        _feetSlot.UnequippedBy(_owner);
                        // m_Owner.Inventory.AddItem(m_FeetSlot);
                        OnUnequip?.Invoke(_feetSlot);
                        _feetSlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Accessory:
                    if (_accessorySlot != null)
                    {
                        _accessorySlot.UnequippedBy(_owner);
                        // m_Owner.Inventory.AddItem(m_AccessorySlot);
                        OnUnequip?.Invoke(_accessorySlot);
                        _accessorySlot = null;
                    }
                    break;
                case EquipmentItem.EquipmentSlot.Weapon:
                    if (Weapon != null)
                    {
                        Weapon.UnequippedBy(_owner);
                    
                        OnUnequip?.Invoke(Weapon);
                        Weapon = null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }
    }
}