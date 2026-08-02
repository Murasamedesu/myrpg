using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        TCharacterItem dbItem;

        public int ItemID;

        public int Count;
        public Item(TCharacterItem item)
        {
            dbItem = item;
            ItemID = (short)item.ItemID;
            Count = (short)item.ItemCount;
        }

        public void Add(int count)
        {
            Count += count;
            dbItem.ItemCount = Count;
        }

        public void Remove(int count)
        {
            Count -= count;
            dbItem.ItemCount = Count;
        }

        public bool Use(int count = 1)
        {
            //if (Count >= count)
            //{
            //    Count -= count;
            //    dbItem.ItemCount = Count;
            //    return true;
            //}
            return false;

        }

        public override string ToString()
        {
            return string.Format("ID: {0}, count: {1}", ItemID, Count);
        }


    }
}
