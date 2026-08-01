using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 仅用于测试NPC的功能性交互的管理器
namespace Managers
{
    public class TESTManager : Singleton<TESTManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop, OnNpcInvokeShop);
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNpcInvokeShop:: NPC:[{0} {1}] Type:{2} Func:{3}", npc.ID, npc.Name, npc.Type, npc.Function);
            UITest text = UIManager.Instance.ShoW<UITest>();
            text.SetTitle(npc.Name);
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