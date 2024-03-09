using Dapper;
using Microsoft.Extensions.Configuration;
using Stossion.BusinessLayers.Interfaces;
using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Services
{
	public class DapperRepository(IConfiguration configuration, StossionDbContext dbContext, IDapperDbContext context) : IDapperInterface
	{
		public async Task<List<T>> QueryExecuteAsync<T>(string query)
		{
			using var connection = context.CreateConnection();
			var result = await connection.QueryAsync<T>(query);
			return result.ToList();
		}
	}
}
