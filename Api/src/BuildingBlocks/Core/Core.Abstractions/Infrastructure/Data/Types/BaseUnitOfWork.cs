using System;

namespace Core.Abstractions.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }

    public class BaseUnitOfWork: IUnitOfWork, IDisposable
    {
        public IDatabaseContext DatabaseContext;
        private bool Disposed;

        public BaseUnitOfWork(IDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public void Dispose()
        {
            Disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            Disposed = false;
            DatabaseContext.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                DatabaseContext.Commit();
                Disposed = true;
                Dispose(true);
            }

            catch (System.Exception exc)
            {
                DatabaseContext.Rollback();
                throw exc;
            }
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    DatabaseContext.Dispose();
                }
            }
            Disposed = true;
        }

        public void Rollback()
        {
            try
            {
                DatabaseContext.Rollback();
            }

            catch (System.Exception exc)
            {                
                throw exc;
            }
        }
    }
}
