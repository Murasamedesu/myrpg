using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Chat
    {
        Character Owner;

        public int localIdx;
        public int worldIdx;
        public int systemIdx;
        public int teamIdx;
        public int guildIdx;

        public Chat(Character owner)
        {
            this.Owner = owner;
        }

        public void PostProcess(NetMessageResponse message)
        {
            if(message.Chat == null)
            {
                message.Chat = new ChatResponse();
                message.Chat.Result = Result.Success;
            }
            this.localIdx = ChatManager.Instance.GetLocalMessages(Owner.Info.mapId, localIdx, message.Chat.localMessages);
            this.worldIdx = ChatManager.Instance.GetWorldMessages(worldIdx, message.Chat.worldMessages);
            this.systemIdx = ChatManager.Instance.GetSystemMessages(systemIdx, message.Chat.systemMssages);
            if(Owner.Team != null)
            {
                this.teamIdx = ChatManager.Instance.GetTeamMessages(Owner.Team.Id, teamIdx, message.Chat.teamMessages);
            }
            if (Owner.Guild != null)
            {
                this.guildIdx = ChatManager.Instance.GetGuildMessages(Owner.Guild.Id, guildIdx, message.Chat.guildMessages);
            }
        }





    }
}
