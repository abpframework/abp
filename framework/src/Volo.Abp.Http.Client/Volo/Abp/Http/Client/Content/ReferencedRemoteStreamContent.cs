using System.IO;
using Volo.Abp.Content;

namespace Volo.Abp.Http.Client.Content
{
    internal class ReferencedRemoteStreamContent : RemoteStreamContent
    {
        private readonly object[] _references;

        public ReferencedRemoteStreamContent(Stream stream, params object[] references)
             : base(stream)
        {
            this._references = references;
        }
    }
}
