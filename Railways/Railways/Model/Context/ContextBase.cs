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
        where TEntity : class, IEntity
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
            var property = typeof(TContext).GetProperty(typeof(TEntity).Name + "Set");
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
        /// Добавление нового объекта
        /// </summary>
        public virtual void Add(TEntity entity)
        {
            try
            {
                Repository.Add(entity);
                ContextKeeper.DataBase.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                throw; 
            }
        }

        /// <summary>
        /// Обновление данных объекта
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            var _entity = Repository.Find(entity.Id);
            if (_entity == null)
            {
                throw new NotImplementedException("Элемент с id " + entity.Id + " не найден в контексте");
            }
            _entity = entity;

            ContextKeeper.DataBase.Entry(_entity).State = EntityState.Modified;
            ContextKeeper.DataBase.SaveChanges();
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        public virtual void Remove(TEntity entity)
        {
            try
            {
                Repository.Remove(entity);
                ContextKeeper.DataBase.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                throw; 
            }
        }

        /// <summary>
        /// метод интерфейса IDisposable
        /// </summary>
        public void Dispose()
        {
            _entities.Dispose();
        }

        #region Query

        /// <summary>
        /// Селекция по заданному условию
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public virtual IQueryable<R> Select<R>(Expression<Func<TEntity, R>> selector)
        {
            return Repository.Select(selector);
        }

        /// <summary>
        /// Прокеция по заданному условию
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Where(predicate);
        }

        /// <summary>
        /// Получение всех элементов
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> All()
        {
            return Repository.AsQueryable();
        }

        /// <summary>
        /// Поиск первого элемента, соответствующего заданному предикату
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate = null)
        {
            return  predicate == null ? Repository.First() : Repository.First(predicate);
        }

        /// <summary>
        /// Поиск последнего элемента, соответствующего заданному предикату
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Last(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? Repository.Last() : Repository.Last(predicate);
        }


        #endregion

    }
}
