using System;

namespace BigCommerceApi.Domain.Services.WSPayForm
{
	public abstract class FormAPIResponse<TResult> where TResult : class
	{
		public FormAPIResponse(TResult? result = default, Exception? raisedException = default)
		{
			Result = result;
			RaisedException = raisedException;
		}

		public TResult? Result { get; }
		public Exception? RaisedException { get; }
		public virtual bool IsValid => Result != null && RaisedException == null;
	}
}
