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
    /// Describes a Leaderboard meta data.
    /// </summary>
    public class LeaderboardMetaData
    {
        /// <summary>
        /// The unique deveoper Id of the Leaderboard.
        /// </summary>
        public string LeaderboardId { get; private set; }

        /// <summary>
        /// The name of the Leaderboard.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The units of the score (example: points, coins, etc).
        /// </summary>
        public string Units { get; private set; }

        /// <summary>
        /// The direction of the Leaderboard (Ascending or Descending).
        /// </summary>
        public LeaderboardDirection Direction { get; private set; }

        /// <summary>
        /// The format of the score (example: number).
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// Shows if the Leaderboard is published.
        /// </summary>
        public bool IsPublished { get; private set; }

        internal LeaderboardMetaData(JSONObject metadataJson)
        {
            Deserialize(metadataJson);
        }

        private void Deserialize(JSONObject json)
        {
            LeaderboardId = json["dev_id_string"].str;
            Name = json["name"].str;
            Units = json["units"].str;
            Direction = json["direction"].str.Equals("DESC") ? LeaderboardDirection.Descending : LeaderboardDirection.Ascending;
            Format = json["format"].str;
            IsPublished = "1".Equals(json["published"].str);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.LeaderboardMetaData"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.LeaderboardMetaData"/>.</returns>
        public override string ToString ()
        {
            return string.Format ("[LeaderboardMetaData: LeaderboardId={0}, name={1}, units={2}, direction={3}, format={4}, isPublished={5}]", LeaderboardId, Name, Units, Direction, Format, IsPublished);
        }
    }

}
