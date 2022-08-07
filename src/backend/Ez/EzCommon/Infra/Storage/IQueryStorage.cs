using EzCommon.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EzCommon.Infra.Storage;
public interface IQueryStorage
{
    T UpinsertSnapShot<T>(T snapShot);
    T GetSnapShot<T>(Expression<Func<T, bool>> query);
    IQueryable<T> Query<T>(Expression<Func<T, bool>> query);
}
