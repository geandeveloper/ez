using System;
using System.Linq;
using System.Linq.Expressions;

namespace EzCommon.Infra.Storage;
public interface IQueryStorage
{
    T QueryOne<T>(Expression<Func<T, bool>> query);
    IQueryable<T> Where<T>(Expression<Func<T, bool>> query);
}
