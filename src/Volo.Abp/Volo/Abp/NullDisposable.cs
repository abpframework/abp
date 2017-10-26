using System;

namespace Volo.Abp
{
    internal sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get; } = new NullDisposable();

        private NullDisposable()
        {
            
        }

        public void Dispose()
        {

        }
    }
}