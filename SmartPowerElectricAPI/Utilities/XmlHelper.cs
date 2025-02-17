using System.Xml.Linq;

namespace SmartPowerElectricAPI.Utilities
{
    /// <summary>
    /// Clase para funciones comunes en el trabajo con ficheros XML
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Dado un nodo determina si este existe o no y en función de esto devuelve vació o el valor del nodo
        /// </summary>
        /// <param name="node">Nodo del XML a comprobar</param>
        /// <returns>Cadena vacía si no hay nodo o el valor del nodo</returns>
        public static string GetValueNode(XAttribute node)
        {
            if (node == null)
            {
                return string.Empty;
            }
            else
            {
                return node.Value;
            }
        }

    }
}
