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
    public class TeamService : Singleton<TeamService>, IDisposable
    {
        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void Init()
        {

        }

        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = User.Instance.CurrentCharacterInfo.Id;
            message.Request.teamInviteReq.FromName = User.Instance.CurrentCharacterInfo.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }


        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.teamInviteRes = new TeamInviteResponse();
            msg.Request.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            msg.Request.teamInviteRes.Errormsg = accept ? "组队成功" : "对方拒绝了你的请求";
            msg.Request.teamInviteRes.Request = request;
            NetClient.Instance.SendMessage(msg);
        }

        public void SendTeamLeaveRequest(int id)
        {
            Debug.Log("SendTeamLeaveRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.teamLeave = new TeamLeaveRequest();
            msg.Request.teamLeave.TeamId = id;
            msg.Request.teamLeave.characterId = User.Instance.CurrentCharacterInfo.Id;
            NetClient.Instance.SendMessage(msg);
        }


        /// <summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        //B收到组队请求
        void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0} 邀请你加入队伍", request.FromName), "组队请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, request);
            };
        }
        /// </summary>





        /// <summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        //收到组队邀请响应
        void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "接受了您的请求", "组队成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "邀请组队失败");
            }
        }
        /// </summary>


        void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }

        void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("退出成功", "退出队伍");
            }
            else
            {
                MessageBox.Show("退出失败", "退出队伍", MessageBoxType.Error);
            }
        }







    }
}