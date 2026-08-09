using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }
        private Map Map;

        //刷新时间
        private float spawnTime = 0;
        //消失时间
        private float unspawnTime = 0;

        private bool spawned = false;

        private SpawnPointDefine spawnPoint = null;


        public Spawner(SpawnRuleDefine define, Map map)
        {
            this.Define = define;
            this.Map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(Map.ID))
            {
                if (DataManager.Instance.SpawnPoints[Map.ID].ContainsKey(Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[Map.ID][Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not existed", Define.ID, Define.SpawnPoint);
                }
            }

        }

        public void Update()
        {
            if (this.CanSpawn())
            {
                this.Spawn();
            }
        }

        bool CanSpawn()
        {
            if (this.spawned)
            {
                return false;
            }
            if(this.unspawnTime + Define.SpawnPeriod > Time.time)
            {
                return false;
            }

            return true;
        }


        public void Spawn()
        {
            spawned = true;
            Log.InfoFormat("Map:[{0}] Spawn:[{1}:: Monster:{2} Lv:{3}] At Point:{4}", Define.MapID, Define.ID, Define.SpawnMonID, Define.SpawnLevel, Define.SpawnPoint);
            this.Map.MonsterManager.Create(Define.SpawnMonID, Define.SpawnLevel, spawnPoint.Position, spawnPoint.Direction);
        }





    }
}
