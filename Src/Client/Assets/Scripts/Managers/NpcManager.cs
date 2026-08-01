using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Common.Data.NpcDefine;


namespace Managers
{
    class NpcManager : Singleton<NpcManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc);

        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();

        public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
                eventMap[function] += action;
        }


        public NpcDefine GetNpcDefine(int npcID)
        {
            NpcDefine npc = null;
            DataManager.Instance.Npcs.TryGetValue(npcID, out npc);
            return npc;
        }

        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
            {
                var npc  = DataManager.Instance.Npcs[npcId];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NpcDefine npc)
        {
            if (npc.Type == NpcType.Task)
            {
                return DoTaskInteractive(npc);
            }
            else if (npc.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;

        }



        private bool DoTaskInteractive(NpcDefine npc)
        {
            MessageBox.Show("Task interaction with NPC: " + npc.Name);
            return true;
        }


        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if(npc.Type != NpcType.Functional)
            {
                Debug.LogErrorFormat("NpcManager > DoFunctionInteractive() > npcID: {0}, npcName: {1} is not a functional NPC", npc.ID, npc.Name);
                return false;
            }
            if(!eventMap.ContainsKey(npc.Function))
            {
                Debug.LogErrorFormat("NpcManager > DoFunctionInteractive() > npcID: {0}, npcName: {1} has no registered event for function: {2}", npc.ID, npc.Name, npc.Function);
                return false;
            }
            return eventMap[npc.Function](npc);
        }



    }
}
