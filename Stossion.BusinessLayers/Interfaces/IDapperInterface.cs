using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Interfaces
{
	public interface IDapperInterface
	{
		Task<List<T>> QueryExecuteAsync<T>(string query);
		Task<T> QueryExecuteSingleAsync<T>(string query);
		Task<T> QuerySingleAsync<T>(string query, object parameter);

    }
}
