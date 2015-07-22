using Newtonsoft.Json;

namespace Moen.KanColle.Dentan.Data.Raw
{
    public class RawMapBgm
    {

        [JsonProperty("api_id")]
        public int ID { get; set; }

        [JsonProperty("api_maparea_id")]
        public int MapAreaID { get; set; }

        [JsonProperty("api_no")]
        public int MapNo { get; set; }

        [JsonProperty("api_map_bgm")]
        public int[] NormalBGM { get; set; }

        [JsonProperty("api_boss_bgm")]
        public int[] BossBGM { get; set; }

        public int NormalDay
        {
            get { return NormalBGM[0]; }
            set { NormalBGM[0] = value; }
        }
        public int NormalNight
        {
            get { return NormalBGM[1]; }
            set { NormalBGM[1] = value; }
        }
        public int BossDay
        {
            get { return BossBGM[0]; }
            set { BossBGM[0] = value; }
        }
        public int BossNight
        {
            get { return BossBGM[1]; }
            set { BossBGM[1] = value; }
        }
    }
}
