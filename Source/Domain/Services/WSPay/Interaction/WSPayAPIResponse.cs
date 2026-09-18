using System;

namespace BigCommerceApi.Domain.Services.WSPay.Interaction
{
    public abstract class WSPayAPIResponse<TResult> where TResult : class
    {
        protected WSPayAPIResponse(TResult? result = default, Exception? raisedException = default)
        {
            Result = result;
            RaisedException = raisedException;
        }

        public TResult? Result { get; }
        public Exception? RaisedException { get; }
        public virtual bool IsValid => Result != null && RaisedException == null;
    }
}
