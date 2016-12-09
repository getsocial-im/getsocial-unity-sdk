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

using System;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// GetSocial API exception.
    /// </summary>
    internal class GetSocialAPIException : Exception
    {
        public enum Code : int
        {   
            SessionNotFound = 1001, /// Session was not found.
            SessionCorrupted = 1002, /// Session corrupted.
            UsernameInUse = 2001, /// Chosen username is already in use.
            EmailInUse = 2002, /// Chosen email is already in use.
            UserNotFound = 2003, /// User not found.
            UserNotLoggedIn = 2004, /// User not logged in.
            UnknownUser = 2005, /// Unknown user.
            UnknownFacebookUser = 2006, /// Unknown facebook user.
            InvalidUsername = 2007, /// That username is not valid.
            UnknownFacebookUserEmail = 2008, /// No email permissions on auth token.
            UnsuccessfulQueryException = 3001, /// Unsuccessfull query.
            DuplicateEntryException = 3002,/// Duplicate entry.
            GameNotFound = 4001, /// Game not found.
            ProblemSavingGame = 4002, /// There was a problem saving your game
            InvalidAuthorizationHeader = 5001, /// Invalid authorization header.
            InvalidEntityCount = 5002, /// Invalid authentication entity count.
            InvalidSignatureException = 5003, /// Invalid authentication signature
            MatchRequirementsFailed = 6001, /// Request authorization requirements don't match.
            UnspecifiedField = 7001, /// Unspecified field.
            InvalidFieldValue = 7002, /// Invalid field value.
            PossibleReplayAttack = 8001, /// Possible replay attack.
            GenericException = 12001/// Generic exception.
        }

        public Code ErrorCode { get; private set; }

        public GetSocialAPIException(String detailMessage, Code errorCode) : base(detailMessage)
        {
            this.ErrorCode = errorCode;
        }
    }
}

