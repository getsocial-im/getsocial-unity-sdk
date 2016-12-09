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

using System.Collections.Generic;
using System;
using GetSocialSdk.Core;
using UnityEngine;

public class LeaderboardSection : DemoMenuSection
{
    private const string LeaderboardId1 = "leaderboard_one";
    private const string LeaderboardId2 = "leaderboard_two";
    private const string LeaderboardId3 = "leaderboard_three";

    private Dictionary<string, Action> sectionButtons;

    private string scoreToSubmit = "Enter score to submit... (Integer)";

    #region implemented abstract members of DemoMenuSection
    protected override string GetTitle()
    {
        return "Leaderboards";
    }

    protected override void InitGuiElements()
    {
        sectionButtons = new Dictionary<string, Action> 
        {
            { "Get User Rank on leaderboard 1", GetUserRankOnLb1 },
            { "Get User Rank on l 1, 2, 3", GetUserRankOnLb123 },
            { "Get first 5 Leaderboards", GetFirst5Lb },
            { "Get first 5 scores from Leaderboard 1", GetFirst5ScoresFromLb1 },
            { "Submit Score to Leaderboard 1", SubmitScoreToLb1 },
            { "Submit Score to Leaderboard 2", SubmitScoreToLb2 }
        };
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButtons(sectionButtons, GSStyles.Button);
        scoreToSubmit = GUILayout.TextField(scoreToSubmit, GSStyles.TextField);
    }
    #endregion

    #region button_actions
    private void GetUserRankOnLb1()
    {
        getSocial.GetLeaderboard(LeaderboardId1, 
            leaderboard => console.LogD(FormatLbData(leaderboard)),
            () => console.LogE(string.Format("Failed to get user rank on " + LeaderboardId1)));
    }

    private void GetUserRankOnLb123()
    {
        var leaderboardIds = new HashSet<string> { LeaderboardId1, LeaderboardId2, LeaderboardId3 };
        getSocial.GetLeaderboards(leaderboardIds, leaderboards =>
        {
            foreach(Leaderboard leaderboard in leaderboards)
            {
                console.LogD(FormatLbData(leaderboard));
            }
        },
        () => console.LogE(string.Format("Failed to get leaderboards data")));
    }

    private void GetFirst5Lb()
    {
        console.LogD("Getting first 5 leaderboards: ");
        getSocial.GetLeaderboards(0, 5, leaderboards =>
        {
            foreach(Leaderboard leaderboard in leaderboards)
            {
                console.LogD(FormatLbData(leaderboard));
            }
        },
        () => console.LogE(string.Format("Failed to get leaderboards data")));
    }

    private void GetFirst5ScoresFromLb1()
    {
        console.LogD("GetFirst5ScoresFromLb1()");

        getSocial.GetLeaderboardScores(LeaderboardId1, 0, 5, LeaderboardScoreType.World, lbScores =>
        {
            foreach(LeaderboardScore score in lbScores)
            {
                console.LogD(score.ToString());
            }
        },
        () => console.LogE(string.Format("Failed to get leaderboards data")));
    }

    private void SubmitScoreToLb1()
    {
        SubmitScoreToLb(LeaderboardId1);
    }

    private void SubmitScoreToLb2()
    {
        SubmitScoreToLb("leaderboard_two");
    }

    private void SubmitScoreToLb(string leaderboardId)
    {
        int score = 0;
        if (int.TryParse(scoreToSubmit, out score)) 
        {
            console.LogD(String.Format("Submitting {0} to {1}", score, leaderboardId));
            getSocial.SubmitLeaderboardScore(leaderboardId, score, 
                                             rank => console.LogD("Now rank is: " + rank),
                                             () => console.LogE("Submitting score failed"));
        }
        else
        {
            console.LogW("Please enter an integer value to submit in the input field above");
        }
    }
    #endregion

    private static string FormatLbData(Leaderboard lb)
    {
        if (lb.CurrentScore == null)
        {
            return string.Format("You have no values on {0}", lb.MetaData.Name);
        }

        return string.Format("You are ranked {0} in {1} with {2}",
            lb.CurrentScore.Rank,
            lb.MetaData.Name,
            lb.CurrentScore.Value);
    }
}
