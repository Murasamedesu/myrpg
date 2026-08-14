using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Guild
    {
        public TGuild Data;
        public int Id { get { return this.Data.Id; } }
        public string Name { get { return this.Data.Name; } }

       

        public double timestamp;

       


        public Guild(TGuild guild)
        {
            this.Data = guild;
        }


        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if(oldApply != null)
            {
                return false;
            }

            var dbApply = DBService.Instance.Entities.TGuildApplies.Create();
            dbApply.TGuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class = apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;

            DBService.Instance.Entities.TGuildApplies.Add(dbApply);
            Data.Applies.Add(dbApply);
            DBService.Instance.Save();

            timestamp = TimeUtil.timestamp;
            return true;

        }

        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
            {
                return false;
            }

            oldApply.Result = (int)apply.Result;

            if(apply.Result == ApplyResult.Accept)
            {
                AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }

            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }


        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now,
            };
            this.Data.Members.Add(dbMember);
            var character = CharacterManager.Instance.GetCharacter(characterId);
            if(character != null)
            {
                character.Data.GuildId = this.Id;
            }
            else
            {
                //DBService.Instance.Entities.Database.ExecuteSqlCommand("UPDATE Characters SET GuildId = @p0 WHERE CharacterId = @p1", this.Id, characterId);
                TCharacter dbChar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterId);
                dbChar.GuildId = this.Id;
            }
            timestamp = TimeUtil.timestamp;
        }

        public void Leave(Character character)
        {
            Log.InfoFormat("Leave Guild : {0} : {1}", character.Id, character.Info.Name);
            var Char = GetDBMember(character.Id);
            if (Char == null) return;
            this.Data.Members.Remove(Char);
            DBService.Instance.Entities.GuildMembers.Remove(Char);


            var playerApplies = this.Data.Applies.Where(v => v.CharacterId == character.Id).ToList();
            foreach (var apply in playerApplies)
            {
                this.Data.Applies.Remove(apply);
                DBService.Instance.Entities.TGuildApplies.Remove(apply);
            }

            if (Char.Title == (int)GuildTitle.President)
            {
                if (this.Data.Members.Count > 0)
                {
                    var nextpro = this.Data.Members.FirstOrDefault(v => v.Title == (int)GuildTitle.VicePresident);
                    if (nextpro == null)
                    {
                        nextpro = this.Data.Members.First();
                    }
                    nextpro.Title = (int)GuildTitle.President;
                    this.Data.LeaderID = nextpro.CharacterId;
                    this.Data.LeaderName = nextpro.Name;
                }
                else
                {
                    Log.InfoFormat("Guild {0} is now empty after member leave.", this.Id);
                    var allApplies = this.Data.Applies.ToList();
                    foreach (var apply in allApplies)
                    {
                        DBService.Instance.Entities.TGuildApplies.Remove(apply);
                    }

                    GuildManager.Instance.RemoveGuild(this.Id, this.Name);
                    DBService.Instance.Entities.Guilds.Remove(this.Data);
                    character.Data.GuildId = 0;
                    character.Guild = null;
                    timestamp = TimeUtil.timestamp;
                    return;
                }
            }
            
            character.Data.GuildId = 0;
            timestamp = TimeUtil.timestamp;
        }


        public void PostProcess(Character from, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);
            }
        }

        internal NGuildInfo GuildInfo(Character from)
        {
            NGuildInfo info = new NGuildInfo()
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderID,
                leaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count,
            };

            if(from != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if(from.Id == this.Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }

        List<NGuildMemberInfo> GetMemberInfos()
        {
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();

            foreach (var member in this.Data.Members)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId = member.CharacterId,
                    Title = (GuildTitle)member.Title,
                    joinTime = (long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime)
                };

                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if(character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Level = character.Data.Level;
                    member.Name = character.Data.Name;
                    member.LastTime = DateTime.Now;
                }
                else
                {
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;
                }
                members.Add(memberInfo);

            }
            return members;
        }

        NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }


        List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach (var apply in this.Data.Applies)
            {
                if(apply.Result != (int)ApplyResult.None) continue;
                applies.Add(new NGuildApplyInfo()
                {
                    characterId = apply.CharacterId,
                    GuildId = apply.TGuildId,
                    Class = apply.Class,
                    Level = apply.Level,
                    Name = apply.Name,
                    Result = (ApplyResult)apply.Result,
                });
            }
            return applies;
        }


        TGuildMember GetDBMember(int characterId)
        {
            foreach (var member in this.Data.Members)
            {
                if(member.CharacterId == characterId)
                {
                    return member;
                }
            }
            return null;
        }

        internal void ExecuteAdmin(GuildAdminCommand command, int targetId, int sourceId)
        {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceId);
            switch (command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    target.Title = (int)GuildTitle.President;
                    source.Title = (int)GuildTitle.None;
                    this.Data.LeaderID = targetId;
                    this.Data.LeaderName = target.Name;
                    break;
                case GuildAdminCommand.Kickout:
                    if (target == null) return; // 目标不在公会中
                    // 禁止踢出会长，会长只能通过Transfer或Leave处理
                    if (target.Title == (int)GuildTitle.President)
                    {
                        Log.Warning("Cannot kick the guild president directly.");
                        return;
                    }
                    this.Data.Members.Remove(target);
                    DBService.Instance.Entities.GuildMembers.Remove(target);
                    var kickedApplies = this.Data.Applies.Where(v => v.CharacterId == target.CharacterId).ToList();
                    foreach (var apply in kickedApplies)
                    {
                        this.Data.Applies.Remove(apply);
                        DBService.Instance.Entities.TGuildApplies.Remove(apply);
                    }

                    //处理被踢玩家的公会绑定状态
                    var targetChar = CharacterManager.Instance.GetCharacter(target.CharacterId);
                    if (targetChar != null)
                    {
                        // 玩家在线
                        targetChar.Data.GuildId = 0;
                    }
                    else
                    {
                        // 玩家离线
                        TCharacter dbChar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == target.CharacterId);
                        if (dbChar != null)
                        {
                            dbChar.GuildId = 0;
                        }
                    }

                    Log.InfoFormat("Guild Kickout: Character {0} kicked from Guild {1} by {2}", target.CharacterId, this.Id, sourceId);
                    break;

            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp;
        }



    }
}
