using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class GuildManager : Singleton<GuildManager>
    {

        public NGuildInfo guildInfo;
        public NGuildMemberInfo myMemberInfo;
        public bool HasGuild
        {
            get { return this.guildInfo != null; }
        }


        public void Init(NGuildInfo guild)
        {
            this.guildInfo = guild;
            if(guild == null)
            {
                myMemberInfo = null;
                return;
            }
            foreach(var mem in guild.Members)
            {
                if(mem.characterId == User.Instance.CurrentCharacter.Id)
                {
                    myMemberInfo = mem;
                    break;
                }
            }
        }


        public void ShowGuild()
        {
            if (this.HasGuild)
            {
                UIManager.Instance.ShoW<UIGuild>();
            }
            else
            {
                var window = UIManager.Instance.ShoW<UIGuildPopNoGuild>();
                window.OnClose += PopNoGuild_OnClose;
            }
        }

        void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
        {
            if(result == UIWindow.WindowResult.Yes) // 创建公会
            {
                UIManager.Instance.ShoW<UIGuildPopCreate>();
            }
            else if (result == UIWindow.WindowResult.No) // 加入公会
            {
                UIManager.Instance.ShoW<UIGuildList>();
            }
        }


    }
}