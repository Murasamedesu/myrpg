using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class CharacterDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }
        
        //基本属性
        public int Speed { get; set; }

        public float MaxHP { get; set; }
        public float MaxMP { get; set; }
        public float GrowthSTR { get; set; }
        public float GrowthINT { get; set; }
        public float GrowthDEX { get; set; }
        public float STR { get; set; } // 力量
        public float INT { get; set; } // 智力
        public float DEX { get; set; } // 敏捷
        public float AD { get; set; } //AD
        public float AP { get; set; } //AP
        public float DEF { get; set; } // 物理防御
        public float MDEF { get; set; } //法术防御
        public float SPD { get; set; } //攻速
        public float CRI { get; set; } //暴击概率


    }
}
