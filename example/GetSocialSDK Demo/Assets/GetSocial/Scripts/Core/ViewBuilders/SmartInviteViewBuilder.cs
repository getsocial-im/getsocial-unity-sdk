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
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder to construct smart invites window.
    /// </summary>
    public class SmartInviteViewBuilder : ViewBuilder
    {
        private string text;
        private string subject;
        private byte[] image;
        private IDictionary<string, string> referralData;

        #region initialization
        private SmartInviteViewBuilder(IGetSocialNativeBridge getSocialImpl) : base(getSocialImpl) {}

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocial.CreateSmartInviteView"/></c> instead.
        /// </summary>
        public static SmartInviteViewBuilder Construct(IGetSocialNativeBridge getSocialImpl)
        {
            return new SmartInviteViewBuilder(getSocialImpl);
        }
        #endregion

        #region public methods implementation
        /// <summary>
        /// Sets the custom text for the smart invites.
        /// NOTE: you can use placeholder tags <see cref="AppInviteUrlPlaceHolder"/>, <see cref="AppNamePlaceHolder"/> etc. to customize the text.
        /// </summary>
        /// <returns><c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="text">Text.</param>
        public SmartInviteViewBuilder SetText(string text)
        {
            this.text = text;
            return this;
        }

        /// <summary>
        /// Sets the subject for the smart invites.
        /// </summary>
        /// <returns><c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="subject">Subject of smart invites.</param>
        public SmartInviteViewBuilder SetSubject(string subject)
        {
            this.subject = subject;
            return this;
        }


        /// <summary>
        /// Sets the image for the smart invites.
        /// </summary>
        /// <returns><c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="image">Subject of smart invites.</param>
        public SmartInviteViewBuilder SetImage(byte[] image)
        {
            this.image = image;
            return this;
        }

        /// <summary>
        /// Sets the image for the smart invites.
        /// </summary>
        /// <returns><c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="image">Subject of smart invites.</param>
        public SmartInviteViewBuilder SetImage(Texture2D image)
        {
            if(image != null)
            {
                this.image = image.EncodeToPNG();
            }
            return this;
        }

        /// <summary>
        /// Sets the referral data for the smart invites.
        /// </summary>
        /// <returns><c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="referralData">Subject of smart invites.</param>
        public SmartInviteViewBuilder SetReferralData(IDictionary<string, string> referralData)
        {
            this.referralData = referralData;
            return this;
        }
        #endregion

        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the smart invites view.
        /// </summary>
        /// <returns>The <c>SmartInviteViewBuilder</c> instance.</returns>
        /// <param name="title">New title.</param>
        public new SmartInviteViewBuilder SetTitle(string title)
        {
            return (SmartInviteViewBuilder)base.SetTitle(title);
        }

        protected override JSONObject ToJson()
        {
            var json = base.ToJson();

            json.SetFieldOptional(PropertyText, text);
            json.SetFieldOptional(PropertySubject, subject);

            if(image != null)
            {
#if UNITY_ANDROID
                    // On Android just save image and get URI from content provider
                    json.SetField(PropertyImage, AndroidUtils.GetBitmapUri(image));
#else
                    // On iOS we send the whole image
                    json.SetField(PropertyImage, Convert.ToBase64String(image));
#endif
            }

            if(referralData != null && referralData.Count > 0)
            {
                var serializedData = new JSONObject(referralData);
                json.SetField(PropertyReferralData, serializedData);
            }

            return json;
        }

        protected override string GetViewId()
        {
            return ViewSmartInvite;
        }
        #endregion
    }
}
