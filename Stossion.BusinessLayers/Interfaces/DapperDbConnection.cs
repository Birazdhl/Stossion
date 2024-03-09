using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Stossion.BusinessLayers.Interfaces
{
	public interface IDapperDbContext
	{
		IDbConnection CreateConnection();
	}
	public class DapperDbContext(IConfiguration configuration) : IDapperDbContext
	{
		private readonly string _connection = configuration.GetConnectionString("StossionConnectionString");

		public IDbConnection CreateConnection() => new SqlConnection(_connection);
	}
}
