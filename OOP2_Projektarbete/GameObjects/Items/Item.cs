﻿

namespace Skalm.GameObjects.Items
{
    internal class Item : IComparable<Item>
    {
        public string Name { get => _itemName; }
        public virtual string Description { get => _itemDescription == "" ? _itemName : _itemDescription; }

        private string _itemName;
        public string _itemDescription;

        // CONSTRUCTOR I
        public Item(string itemName, string itemDescription = "")
        {
            _itemName = itemName;
            _itemDescription = itemDescription;
        }

        // USE ITEM
        public virtual void Use(Player player)
        {
            throw new NotImplementedException();
        }

        // SORT ITEMS
        public virtual int CompareTo(Item? other)
        {
            if (other == null)
                return 1;
            else
                return 0;
        }
    }
}
