using System;
using System.Linq;
using System.Linq.Expressions;

namespace EzCommon.Infra.Storage;
public interface IQueryStorage
{
    T UpInsert<T>(Expression<Func<T, bool>> query, T model);
    T QueryOne<T>(Expression<Func<T, bool>> query);
    IQueryable<T> Query<T>(Expression<Func<T, bool>> query);
}
