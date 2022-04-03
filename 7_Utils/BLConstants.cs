﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatLeader.Utils {
    internal static class BLConstants {

        #region Basic links

        public static readonly string REPLAY_UPLOAD_URL = "https://beatleader.azurewebsites.net/replay";

        public static readonly string BEATLEADER_API_URL = "https://api.beatleader.xyz";

        #endregion

        #region Leaderboard requests

        public static readonly string SCORES_BY_HASH_PAGED = // /v2/scores/{hash}/{diff}/{mode}/{context}/{scope}/page?player={playerId}&page={page}&count={count}
            BEATLEADER_API_URL + "/v3/scores/{0}/{1}/{2}/standard/{3}/page?{4}";

        public static readonly string SCORES_BY_HASH_SEEK = // /v2/scores/{hash}/{diff}/{mode}/{context}/{scope}/around?player={playerId}&count={count}
            BEATLEADER_API_URL + "/v3/scores/{0}/{1}/{2}/standard/{3}/around?{4}";

        public static readonly int SCORE_PAGE_SIZE = 10;

        #endregion

        public static readonly string PROFILE_BY_ID = // /player/{user_id}
            BEATLEADER_API_URL + "/player/{0}";

        internal class Param {
            public static readonly string PLAYER = "player";
            public static readonly string PAGE = "page";
            public static readonly string COUNT = "count";
        }
    }
}