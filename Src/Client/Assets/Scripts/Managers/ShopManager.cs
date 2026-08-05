using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NpcDefine npc)
        {
            Debug.LogFormat("OnOpenShop:: NPC:[{0} {1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
            this.ShowShop(npc.Param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if(DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                UIShop uiShop = UIManager.Instance.ShoW<UIShop>();
                if(uiShop != null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        public bool BuyItem(int shopId, int shopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return true;
        }






        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeInsrance:: NPC:[{0} {1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
            MessageBox.Show("点击了NPC:"+ npc.Name + "的副本功能对话");
            return true;
        }



    }
}