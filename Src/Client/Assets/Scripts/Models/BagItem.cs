using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BagItem
    {
        public ushort ItemId;
        public ushort Count;

        public static BagItem zero = new BagItem { ItemId = 0, Count = 0 };

        public BagItem(int itemId, int count)
        {
            this.ItemId = (ushort)itemId;
            this.Count = (ushort)count;
        }

        public static bool operator ==(BagItem a, BagItem b)
        {
            return a.ItemId == b.ItemId && a.Count == b.Count;
        }

        public static bool operator !=(BagItem a, BagItem b)
        {
            return !(a == b);
        }


        public override bool Equals(object obj)
        {
            if (obj is BagItem)
            {
                return Equals((BagItem)obj);
            }
            return false;
        }

        public bool Equals(BagItem other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return ItemId.GetHashCode() ^ (Count.GetHashCode() << 2);
        }


    }
}