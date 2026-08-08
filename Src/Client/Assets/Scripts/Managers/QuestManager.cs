using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace Managers
{

    public enum NpcQuestStatus
    {
        None = 0,     //无任务
        Complete = 1,   // 已完成
        Available = 2,  // 可接受
        Incomplete = 3, // 进行中
    }


    public class QuestManager : Singleton<QuestManager>
    {
        public delegate void OnQuestChangeHandler();

        public event OnQuestChangeHandler OnQuestChanged;

        public UnityAction<Quest> onQuestStatusChanged;

        public List<NQuestInfo> questInfos;

        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }


        void InitQuests()
        {
            //已有任务
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.QuestId] = quest;
            }

            this.CheckAvailableQuests();

            foreach (var kv in allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }

        }

        void CheckAvailableQuests()
        {


            //可用任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                // 职业是否符合
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                {
                    continue;
                }
                // 等级要求是否满足
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                {
                    continue;
                }
                // 任务是否已存在
                if (this.allQuests.ContainsKey(kv.Key))
                {
                    continue;
                }

                // 任务有前置
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        if (preQuest.Info == null)
                        {
                            continue; //前置未接取
                        }
                        if (preQuest.Info.Status != QuestStatus.Finished)
                        {
                            continue; //前置未完成
                        }
                    }
                    else
                    {
                        continue; // 没接前置
                    }
                }
                Quest quest = new Quest(kv.Value);
                this.allQuests[quest.Define.ID] = quest;
            }

        }


        void AddNpcQuest(int npcId, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
            {
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }

            List<Quest> availables;
            List<Quest> completes;
            List<Quest> incompletes;

            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out completes))
            {
                completes = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Complete] = completes;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }

            if (quest.Info == null)
            {
                if (npcId == quest.Define.AcceptNPC && !npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                    {
                        npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                    }
                }
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }



        }



        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            // 获取npc任务
            if (npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return NpcQuestStatus.Complete;
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return NpcQuestStatus.Incomplete;
                }
            }
            return NpcQuestStatus.None;
        }



        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
                }
            }
            return false;
        }

        bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.ShoW<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                {
                    MessageBox.Show(quest.Define.DialogIncomplete);
                }
            }
            return true;
        }


        void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                if(dlg.quest.Info == null)
                {
                    QuestService.Instance.SendQuestAccept(dlg.quest);
                }
                else if(dlg.quest.Info.Status == QuestStatus.Complated)
                {
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
                }
            }
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }

        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            npcQuests.Clear();
            Quest result;
            if (allQuests.ContainsKey(quest.QuestId))
            {
                allQuests[quest.QuestId].Info = quest;
                result = allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                allQuests[quest.QuestId] = result;
            }

            CheckAvailableQuests();

            foreach(var kv in allQuests)
            {
                AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }

            if(onQuestStatusChanged != null)
            {
                onQuestStatusChanged(result);
            }
            return result;

        }


        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }

        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }



    }
}