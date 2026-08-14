using Common;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Services
{
    public class GuildService : Singleton<GuildService>, IDisposable
    {
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;


        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        public void Init()
        {

        }

        public void SendGuildCreate(string guildName, string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildCreate = new GuildCreateRequest();
            msg.Request.guildCreate.GuildName = guildName;
            msg.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(msg);
        }

        void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse : {0}", response.Result);
            if(OnGuildCreateResult != null)
            {
                this.OnGuildCreateResult(response.Result == Result.Success);
            }
            if(response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(string.Format("{0} 公会创建成功", response.guildInfo.GuildName), "公会");
            }
            else
            {
                MessageBox.Show(string.Format("{0} 公会创建失败", response.guildInfo.GuildName), "公会");
            }

        }

        public void SendGuildJoinRequest(int guildId)
        {
            Debug.Log("SendGuildJoinRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildJoinReq = new GuildJoinRequest();
            msg.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            msg.Request.guildJoinReq.Apply.GuildId = guildId;
            NetClient.Instance.SendMessage(msg);
        }

        public void SendGuildJoinResponse(bool accept, GuildJoinRequest request)
        {
            Debug.Log("SendGuildJoinResponse");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildJoinRes = new GuildJoinResponse();
            msg.Request.guildJoinRes.Result = Result.Success;
            msg.Request.guildJoinRes.Apply = request.Apply;
            msg.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(msg);
        }


        //收到加入公会请求
        void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0} 申请加入公会", request.Apply.Name), "公会申请", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                SendGuildJoinResponse(false, request);
            };

        }


        // 收到加入公会响应
        void OnGuildJoinResponse(object sender, GuildJoinResponse response)
        {
            Debug.LogFormat("OnGuildJoinResponse : {0}", response.Result);
            if(response.Result == Result.Success)
            {
                MessageBox.Show("加入公会成功", "公会");
            }
            else
            {
                MessageBox.Show("加入公会失败" + MessageBoxType.Error, "公会");
            }
        }

        void OnGuild(object sender, GuildResponse response)
        {
            Debug.LogFormat("OnGuild : {0} {1} {2}", response.Result, response.guildInfo.Id, response.guildInfo.GuildName);
            GuildManager.Instance.Init(response.guildInfo);
            if(this.OnGuildUpdate != null)
            {
                this.OnGuildUpdate();
            }
        }


        public void SendGuildLeaveRequest()
        {
            Debug.Log("SendGuildLeaveRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(msg);
        }

        void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if(message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else
            {
                MessageBox.Show("离开公会失败", "公会");
            }
        }


        public void SendGuildListRequest()
        {
            Debug.Log("SendGuildListRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(msg);
        }

        void OnGuildList(object sender, GuildListResponse response)
        {
            if(OnGuildListResult != null)
            {
                OnGuildListResult(response.Guilds);
            }
        }

        public void SendGuildJoinApply(bool accept, NGuildApplyInfo apply)
        {
            Debug.Log("SendGuildJoinApply");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildJoinRes = new GuildJoinResponse();
            msg.Request.guildJoinRes.Result = Result.Success;
            msg.Request.guildJoinRes.Apply = apply;
            msg.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(msg);
        }


        public void SendAdminCommand(GuildAdminCommand command, int characterId)
        {
            Debug.Log("SendAdminCommand");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.guildAdmin = new GuildAdminRequest();
            msg.Request.guildAdmin.Command = command;
            msg.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(msg);
        }

        void OnGuildAdmin(object sender, GuildAdminResponse message)
        {
            Debug.LogFormat("OnGuildAdmin : {0} {1} ", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作:{0} 结果:{1} {2}", message.Command, message.Result, message.Errormsg));

            if (message.Result != Result.Success)
            {
                MessageBox.Show(string.Format("操作失败: {0}", message.Errormsg), "公会");
                return;
            }

            int myCharId = User.Instance.CurrentCharacter.Id; // 你自己的角色ID
            bool isAboutMe = (message.Command.Target == myCharId);

            switch (message.Command.Command)
            {
                case GuildAdminCommand.Kickout:
                    if (isAboutMe)
                    {
                        // 被踢的是自己
                        Log.Info("I was kicked out of the guild!");
                        GuildManager.Instance.Init(null);  // 清空本地公会数据

                        MessageBox.Show("你已被踢出公会", "公会");
                    }
                    else
                    {
                        // 被踢的是别人
                        MessageBox.Show(string.Format("已踢出成员 (ID:{0})", message.Command.Target), "公会");
                    }
                    break;

                case GuildAdminCommand.Transfer:
                    if (isAboutMe)
                    {
                        MessageBox.Show("你已成为新会长", "公会");
                    }
                    else
                    {
                        MessageBox.Show("会长已转让", "公会");
                    }
                    break;

                case GuildAdminCommand.Promote:
                    MessageBox.Show(isAboutMe ? "你已被提升为副会长" : "已提升成员", "公会");
                    break;

                case GuildAdminCommand.Depost:
                    MessageBox.Show(isAboutMe ? "你已被降为普通成员" : "已降级成员", "公会");
                    break;
            }

            if (this.OnGuildUpdate != null)
            {
                this.OnGuildUpdate();
            }
        }


    }
}