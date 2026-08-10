using Managers;
using Models;
using Network;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class FriendService : Singleton<FriendService>, IDisposable
    {
        public UnityAction OnFriendUpdate;

        public void Init()
        {

        }

        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }


        public void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.Log("SendFriendAddRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.friendAddReq = new FriendAddRequest();
            msg.Request.friendAddReq.FromId = User.Instance.CurrentCharacter.Id;
            msg.Request.friendAddReq.FromName = User.Instance.CurrentCharacter.Name;
            msg.Request.friendAddReq.ToId = friendId;
            msg.Request.friendAddReq.ToName = friendName;
            NetClient.Instance.SendMessage(msg);
        }

        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            Debug.Log("SendFriendAddResponse");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.friendAddRes = new FriendAddResponse();
            msg.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            msg.Request.friendAddRes.Errormsg = accept ? "对方同意" : "对方拒绝了你的请求";
            msg.Request.friendAddRes.Request = request; 
            NetClient.Instance.SendMessage(msg);
        }

        public void SendFriendRemoveRequest(int id, int friendId)
        {
            Debug.Log("SendFriendRemoveRequest");
            NetMessage msg = new NetMessage();
            msg.Request = new NetMessageRequest();
            msg.Request.friendRemove = new FriendRemoveRequest();
            msg.Request.friendRemove.Id = id;
            msg.Request.friendRemove.friendId = friendId;
            NetClient.Instance.SendMessage(msg);
        }


        /// <summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        //B收到添加好友请求
        void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0} 请求添加你为好友", request.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                SendFriendAddResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                SendFriendAddResponse(false, request);
            };
        }
        /// </summary>





        /// <summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        //收到添加好友响应
        void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if(message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "接受了您的请求", "添加好友成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "添加好友失败");
            }
        }
        /// </summary>


        void OnFriendList(object sender, FriendListResponse message)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.allFriends = message.Friends;
            if(OnFriendUpdate != null)
            {
                OnFriendUpdate();
            }
        }

        void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if(message.Result == Result.Success)
            {
                MessageBox.Show("删除成功", "删除好友");
            }
            else
            {
                MessageBox.Show("删除失败", "删除好友", MessageBoxType.Error);
            }
        }



    }
}