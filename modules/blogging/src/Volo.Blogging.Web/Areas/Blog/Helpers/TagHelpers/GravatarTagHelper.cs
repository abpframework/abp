using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Blogging.Areas.Blog.Helpers.TagHelpers
{
    /// <inheritdoc />
    /// <summary>
    ///  Returns a Globally Recognised Avatar https://en.gravatar.com
    /// </summary>
    [HtmlTargetElement("img", Attributes = "gravatar-email")]
    public class GravatarTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public GravatarTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Email Address for the Gravatar
        /// </summary>
        [HtmlAttributeName("gravatar-email")]
        public string Email { get; set; }

        /// <summary>
        /// Gravatar content rating (note that Gravatars are self-rated)
        /// </summary>
        [HtmlAttributeName("gravatar-rating")]
        public GravatarRating Rating { get; set; } = GravatarRating.GeneralAudiences;

        /// <summary>
        /// Size in pixels (default: 80)
        /// </summary>
        [HtmlAttributeName("gravatar-size")]
        public int Size { get; set; } = 80;

        /// <summary>
        /// URL to a custom default image (e.g: 'Url.Content("~/images/no-grvatar.png")' )
        /// </summary>
        [HtmlAttributeName("default-image-url")]
        public string DefaultImageUrl { get; set; } = "";

        /// <summary>
        /// Prefer the default image over the users own Gravatar
        /// </summary>
        [HtmlAttributeName("force-default-image")]
        public bool ForceDefaultImage { get; set; }

        /// <summary>
        /// Default image if user hasn't created a Gravatar
        /// </summary>
        [HtmlAttributeName("default-image")]
        public GravatarDefaultImage DefaultImage { get; set; } = GravatarDefaultImage.Default;

        /// <summary>
        /// Always do secure (https) requests
        /// </summary>
        [HtmlAttributeName("force-secure-request")]
        public bool ForceSecureRequest { get; set; } = true;


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var emailAddress = Email == null ? string.Empty : Email.Trim().ToLower();

            var url = string.Format("{0}://{1}.gravatar.com/avatar/{2}?s={3}{4}{5}{6}",
                GetUrlScheme(),
                GetUrlPrefix(),
                GetMd5Hash(emailAddress),
                Size,
                GetDefaultImageParameter(),
                GetForceDefaultImageParameter(),
                GetRatingParameter()
            );

            output.Attributes.SetAttribute("src", url);
        }

        private string GetUrlScheme()
        {
            return ForceSecureRequest || _contextAccessor.HttpContext.Request.IsHttps
                ? "https" : "http";
        }

        private string GetUrlPrefix()
        {
            return ForceSecureRequest || _contextAccessor.HttpContext.Request.IsHttps ? "secure" : "www";
        }

        private string GetDefaultImageParameter()
        {
            return "&d=" + (!string.IsNullOrEmpty(DefaultImageUrl)
                       ? System.Net.WebUtility.UrlEncode(DefaultImageUrl)
                       : GetEnumDescription(DefaultImage));
        }

        private string GetForceDefaultImageParameter()
        {
            return ForceDefaultImage ? "&f=y" : "";
        }

        private string GetRatingParameter()
        {
            return "&r=" + GetEnumDescription(Rating);
        }

        /// <summary>
        /// Generates an MD5 hash of the given string
        /// </summary>
        /// <remarks>Source: http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx </remarks>
        private static string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Returns the value of a Description for a given Enum value
        /// </summary>
        /// <remarks>Source: http://blogs.msdn.com/b/abhinaba/archive/2005/10/21/483337.aspx </remarks>
        /// <param name="en"></param>
        /// <returns></returns>
        private static string GetEnumDescription(Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());

            if (memInfo == null || memInfo.Length <= 0)
            {
                return en.ToString();
            }

            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Any())
            {
                return ((DescriptionAttribute)attrs.First()).Description;
            }

            return en.ToString();
        }

        public enum GravatarDefaultImage
        {
            /// <summary>Default Gravatar logo</summary>
            [Description("")]
            Default,
            /// <summary>404 - do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found) response</summary>
            [Description("404")]
            Http404,
            /// <summary>Mystery-Man - a simple, cartoon-style silhouetted outline of a person (does not vary by email hash)</summary>
            [Description("mm")]
            MysteryMan,
            /// <summary>Identicon - a geometric pattern based on an email hash</summary>
            [Description("identicon")]
            Identicon,
            /// <summary>MonsterId - a generated 'monster' with different colors, faces, etc</summary>
            [Description("monsterid")]
            MonsterId,
            /// <summary>Wavatar - generated faces with differing features and backgrounds</summary>
            [Description("wavatar")]
            Wavatar,
            /// <summary>Retro - awesome generated, 8-bit arcade-style pixelated faces</summary>
            [Description("retro")]
            Retro
        }

        /// <summary>
        /// Gravatar allows users to self-rate their images so that they can indicate if an image is appropriate for a certain audience. By default, only 'G' rated images are displayed unless you indicate that you would like to see higher ratings
        /// </summary>
        public enum GravatarRating
        {
            /// <summary>Suitable for display on all websites with any audience type</summary>
            [Description("g")]
            GeneralAudiences,

            /// <summary>May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence</summary>
            [Description("pg")]
            ParentalGuidance,

            /// <summary>May contain such things as harsh profanity, intense violence, nudity, or hard drug use</summary>
            [Description("r")]
            Restricted,

            /// <summary>May contain hardcore sexual imagery or extremely disturbing violence</summary>
            [Description("x")]
            OnlyMature
        }
    }
}
