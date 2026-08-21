using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;



namespace Battle
{
    public class SkillManager
    {
        Creature Owner;
        public List<Skill> Skills { get; private set; }

        public SkillManager(Creature Owner)
        {
            this.Owner = Owner;
            this.Skills = new List<Skill>();
            this.InitSkills();
        }

        void InitSkills()
        {
            this.Skills.Clear();
            foreach (var skillInfo in this.Owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.Owner);
                this.AddSkill(skill);
            }
        }

        public void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }




    }
}