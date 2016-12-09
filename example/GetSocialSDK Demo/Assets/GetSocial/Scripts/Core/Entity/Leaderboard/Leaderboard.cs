/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Describes a Leaderboard.
    /// </summary>
    public class Leaderboard
    {
        /// <summary>
        /// Gets the leaderboard meta data.
        /// </summary>
        public LeaderboardMetaData MetaData { get; private set; }

        /// <summary>
        /// Gets the leaderboard current score.
        /// </summary>
        public LeaderboardScore CurrentScore { get; private set; }

        internal Leaderboard(JSONObject leaderboardJson)
        {
            Deserialize(leaderboardJson);
        }

        private void Deserialize(JSONObject json)
        {
            JSONObject data1 = json["data"];
            JSONObject leaderboard1 = data1["leaderboard"];
            JSONObject data2 = leaderboard1["data"];

            MetaData = new LeaderboardMetaData(data2);

            var myScoreData = json["data"]["myScore"]["data"];
            if(myScoreData != null && !myScoreData.IsNull)
            {
                CurrentScore = new LeaderboardScore(myScoreData);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.Leaderboard"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.Leaderboard"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Leaderboard: MetaData={0}, CurrentScore={1}]", MetaData, CurrentScore != null ? CurrentScore.ToString() : "null");
        }
    }
}
