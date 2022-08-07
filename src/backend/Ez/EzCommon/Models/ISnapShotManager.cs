namespace EzCommon.Models
{
    public interface ISnapShotManager<TEntity, TSnapShot>
    {
        TEntity FromSnapShot(TSnapShot entityState);
        TSnapShot ToSnapShot();
    }
}
