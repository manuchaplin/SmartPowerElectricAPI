namespace SmartPowerElectricAPI.Utilities
{
    /// <summary>
    /// Clase que define la estructura para la ordenación: campo y tipo de ordenación
    /// </summary>
    /// <typeparam name="T">Modelo sobre el que se aplica</typeparam>
    public class OrderByHelper<T>
    {
        /// <summary>
        /// Campo que se va a ordenar
        /// </summary>
        public Func<T, object> OrderBy { get; set; }
        /// <summary>
        /// Tipo de ordenación que se va a realizar: asc->Ascendente; desc->Descendente
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// Constructor que recibe sólo el campo por el que se va a ordenar.
        /// En este caso el tipo de ordenación se establece como asc
        /// </summary>
        /// <param name="orderBy">Campo para la ordenación</param>
        public OrderByHelper(Func<T, object> orderBy)
        {
            OrderBy = orderBy;
            Type = "asc";
        }

        /// <summary>
        /// Constructor que recibe el campo y el tipo de ordenación
        /// </summary>
        /// <param name="orderBy">Campo para la ordenación</param>
        /// <param name="type">Tipo de ordenación. Si no es asc o desc se asigna la ordenación asc</param>
        public OrderByHelper(Func<T, object> orderBy, String type)
        {
            OrderBy = orderBy;
            Type = (type.ToLower() == "asc" || type.ToLower() == "desc") ? type : "asc";
        }

    }

    /// <summary>
    /// Clase para realizar ordenaciones múltiples con Linq
    /// </summary>
    public static class LinqMultipleOrderBy
    {
        /// <summary>
        /// Construye la sentencia de ordenación con las diferentes condiciones
        /// </summary>
        /// <typeparam name="T">Modelo con el que se trabaja</typeparam>
        /// <param name="data">Colección a la que se aplicará la ordenación</param>
        /// <param name="orderByProperties">Lista de condiciones para la ordenación</param>
        /// <returns>La colección con las condiciones de ordenación concatenadas</returns>
        public static IEnumerable<T> MultipleOrderBy<T>(this IEnumerable<T> data, List<OrderByHelper<T>> orderByProperties)
        {
            IEnumerable<T> query = from dbItem in data
                                   select dbItem;
            IOrderedEnumerable<T> orderedQuery = null;
            int numberOrderBy = 0;
            foreach (var orderByProperty in orderByProperties)
            {
                if (numberOrderBy == 0)
                {
                    orderedQuery = (orderByProperty.Type.ToLower() == "desc")
                        ? query.OrderByDescending(orderByProperty.OrderBy)
                        : query.OrderBy(orderByProperty.OrderBy);
                }
                else
                {
                    orderedQuery = (orderByProperty.Type.ToLower() == "desc")
                        ? orderedQuery.ThenByDescending(orderByProperty.OrderBy)
                        : orderedQuery.ThenBy(orderByProperty.OrderBy);
                }
                numberOrderBy++;
            }
            if (orderedQuery != null)
                query = orderedQuery;

            return query;
        }
    }
}
