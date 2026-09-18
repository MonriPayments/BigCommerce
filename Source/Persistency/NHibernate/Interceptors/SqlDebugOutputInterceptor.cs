using System.Diagnostics;
using NHibernate;
using NHibernate.SqlCommand;

namespace BigCommerceApi.Persistency.NHibernate.Interceptors
{
    public class SqlDebugOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.Write("NHibernate: ");
            Debug.WriteLine(sql);

            return base.OnPrepareStatement(sql);
        }
    }
}
