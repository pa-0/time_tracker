using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Ficksworkshop.TimeTrackerAPI
{
    /// <summary>
    /// Generates proxy objects for the <see cref="XmlDataSetProjectTimesData"/>. This class
    /// ensures that we generate (i.e. return) the same proxy object every time the same row
    /// is requested.
    /// 
    /// The benefits of this approach is we avoid lots of small memory allocations and the equality
    /// test is easy since they are the same object.
    /// </summary>
    /// <typeparam name="TRow">The type of row this proxy returns.</typeparam>
    /// <typeparam name="TProxy">The type of proxy this returns.</typeparam>
    abstract internal class XmlDataSetProxyFactory<TRow, TProxy>
    {
        private readonly Dictionary<TRow, TProxy> _allocatedItems = new Dictionary<TRow, TProxy>();

        internal XmlDataSetProxyFactory()
        {
            
        }

        /// <summary>
        /// Gets or creates the TProxy instance for the specified TRow.
        /// </summary>
        /// <param name="rowItem">The row to represent.</param>
        /// <returns>The proxy for the row.</returns>
        internal TProxy Create(TRow rowItem)
        {
            TProxy existingProxy;
            if(_allocatedItems.TryGetValue(rowItem, out existingProxy))
            {
                return existingProxy;
            }

            // It doesn't exist yet, so we need to create it, store it, and return it.
            existingProxy = CreateInstance(rowItem);
            _allocatedItems[rowItem] = existingProxy;

            return existingProxy;
        }

        /// <summary>
        /// Does the work of actually generating the 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected abstract TProxy CreateInstance(TRow row);
    }
}
