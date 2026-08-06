using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

using Common;

namespace GameServer.Services
{
    class DBService : Singleton<DBService>
    {
        ExtremeWorldEntities entities;

        public ExtremeWorldEntities Entities
        {
            get { return this.entities; }
        }

        public void Init()
        {
            entities = new ExtremeWorldEntities();
        }

        public void Save(bool async = false)
        {
            try
            {
                if (async)
                {
                    entities.SaveChangesAsync();
                }
                else
                {
                    entities.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    // 哪个实体、当前处于什么状态（Added/Modified…）
                    Log.ErrorFormat("[EF验证失败] 实体={0}  状态={1}",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        // 哪个属性、违反什么规则、当前值是什么
                        object cur = null;
                        try { cur = eve.Entry.CurrentValues[ve.PropertyName]; } catch { }

                        Log.ErrorFormat("    属性={0}  错误={1}  当前值={2}",
                            ve.PropertyName, ve.ErrorMessage, cur ?? "<null>");
                    }
                }
            }
        }

    }
}
