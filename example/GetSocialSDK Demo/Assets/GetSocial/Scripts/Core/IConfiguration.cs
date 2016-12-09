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
    /// Interface for GetSocial UI configuration.
    /// Learn more about SDK customizations from the <a href="http://docs.getsocial.im/#customizing-the-appearance">documentation</a>.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Sets the UI configuration file to use.
        /// </summary>
        /// <param name="configFileOrUrl">Relative path to the configuration file in <code>/StreamingAssets/</code>folder or URL.
        ///
        /// Note: Use loading configuration from url for development purposes, there is no callback for now to know if changing config succeeded.
        ///
        /// e.g. <code>mygame/myconfig.json</code> or url that contains json configuration file</param>
        /// 
        void SetConfiguration(string configFileOrUrl);

        /// <summary>
        /// Clear all GetSocial UI configurations and set them to default. Invoke outside of transaction.
        /// </summary>
        void Clear();

        /// <summary>
        /// All GetSocial UI configuration changes should start with call to <c><see cref="BeginTransaction"/></c>.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// All GetSocial UI configuration changes should end with call to <c><see cref="EndTransaction"/></c>.
        /// </summary>
        void EndTransaction();

        /// <summary>
        /// Base path relative to <c>/StreamingAssets/</c> folder,
        /// e.g. if all resources are in <c>/StreamingAssets/getsocial/</c>, call method
        /// <c>SetBasePath("getsocial/")</c>;
        /// </summary>
        /// <param name="basePath">Base path for assets relative to <c>/StreamingAssets/</c> folder.</param>
        void SetBasePath(string basePath);

        /// <summary>
        /// Sets the image path.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="path">Image path relative to <c>/StreamingAssets/</c> folder.</param>
        void SetImagePath(string id, string path);

        /// <summary>
        /// Sets the base design of the window.
        /// </summary>
        void SetBaseDesign(int width, int height, int ppi, GetSocial.ScaleMode scaleMode);

        /// <summary>
        /// Sets the dimension for a specific element.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="dimension">Dimension value.</param>
        void SetDimension(string id, float dimension);

        /// <summary>
        /// Sets the aspect ratio for a specified element id.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="aspectRatio">Aspect ratio value.</param>
        void SetAspectRatio(string id, float aspectRatio);

        /// <summary>
        /// Sets the color for specified element id.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="color">Color as hex integer, e.g. <c>0xAACCCCCC</c>.</param>
        void SetColor(string id, uint color);

        /// <summary>
        /// Sets the text style for a specific element id.
        /// <br/><b>NOTE</b>: typeface file name should be exactly the same as font postscript name, e.g. <c>Arial-BoldMT.ttf</c>
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="typefacePath">Typeface file name.
        /// Typeface file should be in folder specified as the base path in your configuration file.</param>
        /// <param name="textSize">Text size specified in pixels.</param>
        /// <param name="textColor">Text color as hex integer, e.g. <c>0xAACCCCCC</c>.</param>
        /// <param name="strokeColor">Stroke color as hex integer, e.g. <c>0xAACCCCCC</c>.</param>
        /// <param name="strokeSize">Stroke size specified in pixels.</param>
        /// <param name="strokeXOffset">Stroke X offset specified in pixels.</param>
        /// <param name="strokeYOffset">Stroke Y offset specified in pixels.</param>
        void SetTextStyle(string id, string typefacePath, float textSize, uint textColor, uint strokeColor = 0, float strokeSize = 0, float strokeXOffset = 0f, float strokeYOffset = 0f);

        /// <summary>
        /// Sets the animation style for a specific element id.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="animationStyle">Style of animation.</param>
        void SetAnimationStyle(string id, GetSocial.AnimationStyle animationStyle);

        /// <summary>
        /// Sets insets for specific element.
        /// </summary>
        /// <param name="id">Id of the element to customize.</param>
        /// <param name="top">Top inset.</param>
        /// <param name="right">Right inset.</param>
        /// <param name="bottom">Bottom inset.</param>
        /// <param name="left">Left inset.</param>
        void SetInsets(string id, int top, int right, int bottom, int left);
    }
}
