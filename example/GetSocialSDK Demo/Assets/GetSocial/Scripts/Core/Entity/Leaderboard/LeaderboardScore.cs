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

using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Describes a Leaderboard score.
    /// </summary>
    public class LeaderboardScore
    {
        /// <summary>
        /// User that has the score.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// The value of the score.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// The rank of the user.
        /// </summary>
        public int Rank { get; private set; }

        internal LeaderboardScore(JSONObject scoreJson)
        {
            Deserialize(scoreJson);
        }

        private void Deserialize(JSONObject json)
        {
            User = ParseUserIdentity(json);
            Value = (int)json["value"].n;
            Rank = (int)json["rank"].n;
        }

        private static User ParseUserIdentity(JSONObject myScoreData)
        {
            Debug.Log("ParseUserIdentity");

            Debug.Log(myScoreData.ToString(true));
            if(myScoreData["user"] == null || myScoreData["user"].IsNull)
            {
                return null;
            }

            var userJson = myScoreData["user"]["data"]["user"]["data"];
            var providerList = myScoreData["user"]["data"]["authProviders"]["data"];

            if(providerList != null && providerList.type != JSONObject.Type.NULL)
            {
                var providers = providerList.keys;
                var identitiesJson = new JSONObject();
                foreach(var authProvider in providers)
                {
                    identitiesJson.AddField(authProvider, providerList[authProvider]);
                }
                userJson.AddField("identities", identitiesJson);
            }

            return new User(userJson);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.LeaderboardScore"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.LeaderboardScore"/>.</returns>
        public override string ToString()
        {
            return string.Format("[LeaderboardScore: User={0}, Value={1}, Rank={2}]", User, Value, Rank);
        }
    }

}
