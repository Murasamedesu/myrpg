using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common.Battle
{
    // 角色属性
    public class Attributes
    {
        AttributeData Initial = new AttributeData();
        AttributeData Growth = new AttributeData();
        AttributeData Equip = new AttributeData();
        AttributeData Basic = new AttributeData();
        AttributeData Buff = new AttributeData();
        public AttributeData Final = new AttributeData();

        int Level;
        private NAttributeDynamic dynamic;

        public float HP
        {
            get { return dynamic.Hp; }
            set { dynamic.Hp = (int)Math.Min(MaxHP, value); }
        }

        public float MP
        {
            get { return dynamic.Mp; }
            set { dynamic.Mp = (int)Math.Min(MaxMP, value); }
        }


        public float MaxHP { get { return this.Final.MaxHP; } }
        public float MaxMP { get { return this.Final.MaxMP; } }
        public float STR { get { return this.Final.STR; } }
        public float INT { get { return this.Final.INT; } }
        public float DEX { get { return this.Final.DEX; } }
        public float AD { get { return this.Final.AD; } }
        public float AP { get { return this.Final.AP; } }
        public float DEF { get { return this.Final.DEF; } }
        public float MDEF { get { return this.Final.MDEF; } }
        public float SPD { get { return this.Final.SPD; } }
        public float CRI { get { return this.Final.CRI; } }



        // 初始化角色属性
        public void Init(CharacterDefine define, int level, List<EquipDefine> equips, NAttributeDynamic dynamicAttr)
        {
            dynamic = dynamicAttr;
            LoadInitAttribute(this.Initial, define);
            LoadGrowthAttribute(this.Growth, define);
            LoadEquipAttribute(this.Equip, equips);
            this.Level = level;
            InitBasicAttributes();
            InitSecondaryAttributes();

            InitFinalAttributes();
            this.HP = dynamicAttr.Hp;
            this.MP = dynamicAttr.Mp;

        }


        // 计算基础属性
        public void InitBasicAttributes()
        {
            for(int i = (int)AttributeType.MaxHP; i < (int)AttributeType.MAX; i++)
            {
                Basic.Data[i] = Initial.Data[i];
            }

            for(int i = (int)AttributeType.STR; i <= (int)AttributeType.DEX; i++)
            {
                Basic.Data[i] = Initial.Data[i] + Growth.Data[i] * (Level - 1);     //一级属性成长
                Basic.Data[i] += Equip.Data[i];     //装备一级属性加成在计算属性前
            }
        }


        public void InitSecondaryAttributes()
        {
            // 二级属性成长,包括装备
            Basic.MaxHP = Basic.STR * 10 + Initial.MaxHP + Equip.MaxHP;
            Basic.MaxMP = Basic.INT * 10 + Initial.MaxMP + Equip.MaxMP;
            Basic.AD = Basic.STR * 5 + Initial.AD + Equip.AD;
            Basic.AP = Basic.INT * 5 + Initial.AP + Equip.AP;
            Basic.DEF = Basic.STR * 2 + Basic.DEX * 1 + Initial.DEF + Equip.DEF;
            Basic.MDEF = Basic.INT * 2 + Basic.DEX * 1 + Initial.MDEF + Equip.MDEF;
            Basic.SPD = Basic.DEX * 0.2f + Initial.SPD + Equip.SPD;
            Basic.CRI = Basic.DEX * 0.0002f + Initial.CRI + Equip.CRI;
        }



        public void InitFinalAttributes()
        {
            for (int i = (int)AttributeType.MaxHP; i < (int)AttributeType.MAX; i++)
            {
                Final.Data[i] = Basic.Data[i] + Buff.Data[i];
            }
        }



        void LoadInitAttribute(AttributeData attr, CharacterDefine define)
        {
            attr.MaxHP = define.MaxHP;
            attr.MaxMP = define.MaxMP;

            attr.STR = define.STR;
            attr.INT = define.INT;
            attr.DEX = define.DEX;
            attr.AD = define.AD;
            attr.AP = define.AP;
            attr.DEF = define.DEF;
            attr.MDEF = define.MDEF;
            attr.SPD = define.SPD;
            attr.CRI = define.CRI;
        }

        void LoadGrowthAttribute(AttributeData attr, CharacterDefine define)
        {
            attr.STR = define.GrowthSTR;
            attr.INT = define.GrowthINT;
            attr.DEX = define.GrowthDEX;
        }

        void LoadEquipAttribute(AttributeData attr, List<EquipDefine> equips)
        {
            attr.Reset();
            foreach(var define in equips)
            {
                attr.MaxHP += define.MaxHP;
                attr.MaxMP += define.MaxMP;
                attr.STR += define.STR;
                attr.INT += define.INT;
                attr.DEX += define.DEX;
                attr.AD += define.AD;
                attr.AP += define.AP;
                attr.DEF += define.DEF;
                attr.MDEF += define.MDEF;
                attr.SPD += define.SPD;
                attr.CRI += define.CRI;
            }
        }




    }
}
