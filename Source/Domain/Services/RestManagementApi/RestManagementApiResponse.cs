using System;

namespace BigCommerceApi.Domain.Services.RestManagementApi
{
	public abstract class RestManagementApiResponse<TResult> where TResult : class
	{
        protected RestManagementApiResponse(TResult? result = default, Exception? raisedException = default)
		{
			Result = result;
			RaisedException = raisedException;
		}

		public TResult? Result { get; }
		public Exception? RaisedException { get; }
		public virtual bool IsValid => Result != null && RaisedException == null;
	}
}
