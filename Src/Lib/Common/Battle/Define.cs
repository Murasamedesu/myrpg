using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Battle
{
    // 属性定义 ,枚举
    public enum AttributeType
    {
        None = -1,
        MaxHP = 0,
        MaxMP = 1,
        STR = 2, 
        INT = 3,
        DEX = 4,
        AD = 5,
        AP = 6,     
        DEF = 7,    //护甲
        MDEF = 8,   //魔抗
        SPD = 9,    //攻速
        CRI = 10,   //暴击率

        MAX
    }

}
