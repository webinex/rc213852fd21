using System;
using System.Collections.Generic;

namespace Webinex.Receipts.Localization.Core.Lang
{
    internal static class EnumerableDisposerExtension
    {
        public static void Dispose(this IEnumerable<IDisposable> disposables)
        {
            
            var exceptions = new List<Exception>();
            foreach (var disposable in disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count == 1)
            {
                throw exceptions[0];
            }

            if (exceptions.Count > 1)
            {
                throw new AggregateException(exceptions);
            }
        }
        
        public static void DisposeAfterException(this IEnumerable<IDisposable> disposables, Exception exception)
        {
            try
            {
                disposables.Dispose();
            }
            catch (Exception ex)
            {
                throw new AggregateException(exception, ex);
            }

            throw exception;
        }
    }
}