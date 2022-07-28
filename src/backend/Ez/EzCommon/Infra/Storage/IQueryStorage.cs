using EzCommon.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EzCommon.Infra.Storage;
public interface IQueryStorage
{
    T UpinsertSnapShot<T>(T snapShot) where T : AggregateRoot;
    T GetSnapShot<T>(Expression<Func<T, bool>> query) where T : AggregateRoot;
    IQueryable<T> Query<T>(Expression<Func<T, bool>> query);
}
