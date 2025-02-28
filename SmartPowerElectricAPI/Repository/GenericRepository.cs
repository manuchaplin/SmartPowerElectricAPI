using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Utilities;

using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq.Expressions;


namespace SmartPowerElectricAPI.Repository
{
    public class GenericRepository<TEntity> : IDisposable, IGenericRepository<TEntity> where TEntity : class
    {
        internal SmartPowerElectricContext context;
        protected DbSet<TEntity> dbSet;

        /// <summary>
        /// Constructor protegido. Sólo pueden crear instancias sus hijos.
        /// </summary>
        /// <param name="context">Contexto</param>
        protected GenericRepository(SmartPowerElectricContext context)
        {
            this.context = context;            
            dbSet = context.Set<TEntity>();
        }


        /**** Métodos públicos *******/

        /// <summary>
        /// Método que devuelve el conjunto de entidades.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get()
        {
            return dbSet;
        }

        /// <summary>
        /// Método que obtiene un listado de elementos que cumplen con el listado de filtros pasado
        /// por parámetro.
        /// </summary>
        /// <param name="filters">Listado de filtros.</param>
        /// <returns>Lista de elementos que cumplen con los filtros.</returns>
        public virtual IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters)
        {
            IQueryable<TEntity> data = dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    data = data.Where(filter);
                }
            }

            return data.ToList();
        }

        /// <summary>
        /// Permite devovler una entidad con filtros, incluyendo propiedades de navegación
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="includeProperties">String nombre propiedad navegación, separadas por , </param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters, string includeProperties = null)
        {
            IQueryable<TEntity> data = dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    data = data.Where(filter);
                }
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                             (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    data = data.Include(includeProperty);
                }
            }


            return data.ToList();
        }

        /// <summary>
        /// Método que obtiene un listado de elementos que cumplen con el listado de filtros pasados y ordenado según lo indicado por parámetro.
        /// </summary>
        /// <param name="filters">Listado de filtros.</param>
        /// <param name="orderByProperties">Lista de campos para ordenar</param>
        /// <returns>Lista de elementos que cumplen con los filtros.</returns>
        public virtual IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties)
        {
            IQueryable<TEntity> data = dbSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    data = data.Where(filter);
                }
            }

            // Si se reciben condiciones de ordenación se aplican
            if (orderByProperties != null)
                data = data.MultipleOrderBy(orderByProperties).AsQueryable();

            return data.ToList();
        }


        /// <summary>
        /// Método que obtiene los elementos.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Get(
                                            Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }



        /// <summary>
        /// Método que selecciona un número de elementos indicados por parámetro que se ajustan al listado
        /// de filtros.
        /// </summary>
        /// <param name="numberTake">Número de elementos a obtener.</param>
        /// <param name="filters">Filtros que deben cumplir los elementos seleccionados.</param>
        /// <returns>Listado con los elementos que cumplen los filtros.</returns>
        public virtual IEnumerable<TEntity> Take(List<Expression<Func<TEntity, bool>>> filters = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filters != null)
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));

            return query.ToList();
        }

        /// <summary>
        /// Método que selecciona un número de elementos indicados por parámetro que se ajustan al listado
        /// de filtros.
        /// </summary>
        /// <param name="numberTake">Número de elementos a obtener.</param>
        /// <param name="filters">Filtros que deben cumplir los elementos seleccionados.</param>
        /// <returns>Listado con los elementos que cumplen los filtros.</returns>
        public virtual IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters = null)
        {
            if (numberTake <= 0) return null;

            IQueryable<TEntity> query = dbSet;

            /*
            Esto es equivalente a añadir los filtros con Linq.
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
             */
            if (filters != null)
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));

            // Con Linq to Objects.
            // Generamos el listado para que se apliquen los filtros.
            // var items = query.ToList();

            // Con Linq to Objects.
            // Obtenemos los elementos.
            // return items.Take(numberTake).ToList();

            // Con Linq to Entities.
            return query.Take(numberTake).ToList();
        }

        /// <summary>
        /// Busca un número determinado de registros que cumplan unas condiciones y según el orden solicitado
        /// </summary>
        /// <param name="numberTake">Número de registros a devovler</param>
        /// <param name="filters">Condiciones para la búsqueda de registros</param>
        /// <param name="orderByProperties">Lista de campos para ordenar</param>
        /// <returns>Devuelve un número de registros según las condiciones y en el orden solicitado</returns>
        public virtual IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties)
        {
            if (numberTake <= 0) return null;

            IQueryable<TEntity> query = dbSet;
            // Si se reciben filtros se aplican
            if (filters != null && filters.Any()) { query = filters.Aggregate(query, (current, filter) => current.Where(filter)); }


            // Si se reciben condiciones de ordenación se aplican
            if (orderByProperties != null) { query = query.MultipleOrderBy(orderByProperties).AsQueryable(); }



            return query.Take(numberTake).ToList();
        }
        /// <summary>
        /// Busca un número determinado de registros que cumplan unas condiciones y según el orden solicitado
        /// </summary>
        /// <param name="numberTake">Número de registros a devovler</param>
        /// <param name="filters">Condiciones para la búsqueda de registros</param>
        /// <param name="orderByProperties">Lista de campos para ordenar</param>
        /// <returns>Devuelve un número de registros según las condiciones y en el orden solicitado</returns>
        public virtual IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties, string includeProperties)
        {
            if (numberTake <= 0) return null;

            IQueryable<TEntity> query = dbSet;
            if (!String.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            // Si se reciben filtros se aplican
            if (filters.Any()) { query = filters.Aggregate(query, (current, filter) => current.Where(filter)); }


            // Si se reciben condiciones de ordenación se aplican
            if (orderByProperties != null) { query = query.MultipleOrderBy(orderByProperties).AsQueryable(); }



            return query.Take(numberTake).ToList();
        }

        /// <summary>
        /// Método que realiza la cuenta del número de elementos existentes en la base de datos.
        /// que se corresponden con los filtros pasados por parámentro.
        /// </summary>
        /// <param name="groupBy">String del atributo de la entidad por la que se va a agrupar los elementos.</param>
        /// <param name="filters">Listado de filtros a aplicar a los elementos a contar.</param>
        /// <returns>El número de elementos.</returns>
        public virtual int Count(String groupBy, List<Expression<Func<TEntity, bool>>> filters)
        {
            if (String.IsNullOrEmpty(groupBy)) return 0;

            IQueryable<TEntity> query = dbSet;

            // Aplicamos el listado de filtros.
            if (filters != null)
            {
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            // Linq to Objects
            // Generamos el listado para que se apliquen los filtros.
            //var items = query.ToList();

            // Linq to Entities
            var items = query;

            // Al aplicar los filtros, como son independientes, se puede dar el caso
            // de que una misma entidad se repita, ya que cumple más de un filtro.
            // En este caso agrupamos todas las entidades iguales (p.e. groupBy = "id" --> clave primaria)
            // y contamos el número de veces que se repite la entidad.
            var itemsResult = from dbItem in items
                              group dbItem by groupBy into dbResult
                              select new
                              {
                                  MyKey = dbResult.Key,
                                  MyCount = dbResult.Count()
                              };

            // Como resulado tenemos un listado de objetos anónimos donde
            // en MyCount está la suma del número de elementos por grupo.
            // Sumamos dichas cantidades para obtener el resultado final.
            int totalCount = 0;
            foreach (var item in itemsResult)
            {
                totalCount += item.MyCount;
            }

            return totalCount;
        }

        /// <summary>
        /// Método que obtiene una entidad según su id.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <returns>La entidad seleccionada.</returns>
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetByID(object id, string includeProperties = null)
        {
            // Buscar la entidad por ID
            var entity = dbSet.Find(id);

            // Si no se encuentra, devolver null (o lanzar una excepción si prefieres)
            if (entity == null) return null;

            // Si se especifican propiedades a incluir (carga de relaciones)
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Se debe realizar el Include solo cuando se está trabajando con una consulta
                    entity = dbSet.Include(includeProperty).FirstOrDefault(e => e.Equals(entity));
                }
            }

            return entity;
        }



        /// <summary>
        /// Método que inserta una entidad en el contexto.
        /// </summary>
        /// <param name="entity">Entidad a insertar.</param>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
        }

        public virtual void InsertMassive(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                dbSet.Add(entity);

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        public virtual void RefreshDataContext(TEntity entity)
        {
            ((IObjectContextAdapter)this.context).ObjectContext.Refresh(RefreshMode.StoreWins, entity);

        }
        /// <summary>
        /// Método que guarda los cambios en el contexto.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Método que elimina una entidad del contexto indicada por su id.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = GetByID(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Método que elimina una entidad del contexto indicada por su id.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        public virtual void DeleteRange(IEnumerable<TEntity> entityToDelete, TEntity entityToDeleteAux)
        {
            if (context.Entry(entityToDeleteAux).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDeleteAux);
            }
            dbSet.RemoveRange(entityToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Método que elimina una entidad del contexto.
        /// </summary>
        /// <param name="entityToDelete">Entidad completa a eliminar</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Método que actualiza una entidad en el contexto.
        /// </summary>
        /// <param name="entityToUpdate">Entidad completa a actualizar.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
        }



        /****** Dipose ****************/
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
