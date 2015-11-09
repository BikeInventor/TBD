using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using Railways.Model.Interfaces;

namespace Railways.Model.Context
{
    /// <summary>
    /// Класс, разделяющий контекст базы данных на локальные контексты для каждой из сущностей
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class ContextBase<TEntity, TContext> : IContext<TEntity>, IDisposable
        where TEntity : class
        where TContext : DbContext, new()
    {
        /// <summary>
        /// Место для хранения локального контекста
        /// </summary>
        private readonly TContext _entities = new TContext();

        /// <summary>
        /// Репозиторий с объектами
        /// </summary>
        public DbSet<TEntity> Repository { get; set; }


        /// <summary>
        /// Поиск свойства с именем "TEntity + Set" в исходном репозитории
        /// </summary>
        public ContextBase()
        {
            var property = typeof(TContext).GetProperty(typeof(TEntity).Name);
            if (property == null)
                throw new InvalidOperationException(typeof(TEntity).Name + "Set not found");

            Repository = (DbSet<TEntity>)property.GetMethod.Invoke(_entities, null);
            if (Repository == null)
                throw new InvalidOperationException(typeof(TEntity).Name + " could not be casted to DbSet<" + typeof(TEntity).Name + ">");
        }

        /// <summary>
        /// Поиск объекта по заданному условию
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>Список объектов, удовлетворяющих предикату</returns>
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Where(predicate);
        }

        /// <summary>
        /// Возвращает все объекты
        /// </summary>
        public virtual IQueryable<TEntity> GetAll()
        {
            return Repository.AsQueryable();
        }

        /// <summary>
        /// Добавление нового объекта
        /// </summary>
        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        /// <summary>
        /// Обновление данных объекта
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            Repository.Attach(entity);
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
        }

        /// <summary>
        /// Сохранение изменений в БД
        /// </summary>
        public virtual void SaveChanges()
        {
            _entities.SaveChanges();
        }

        /// <summary>
        /// метод интерфейса IDisposable
        /// </summary>
        public void Dispose()
        {
            _entities.Dispose();
        }

    }
}
