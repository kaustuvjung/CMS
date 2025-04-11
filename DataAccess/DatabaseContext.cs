using Helper;
using ElmahCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
   public class DatabaseContext: IDisposable
    {
        private readonly AppConfig _config;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string connectionString;
        public SqlConnection cn;


        public DatabaseContext(string connectionString)
        {
            this.cn = new SqlConnection(connectionString);
        }

        public DatabaseContext(AppConfig config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            this.httpContextAccessor = httpContextAccessor;
            this.connectionString = _config.GetValue("DbConnection");
            this.cn = new SqlConnection(this.connectionString);
        }

        public List<SqlParameter> SqlParameters
        {
            get
            {
                return new List<SqlParameter>();
            }
        }

        public async Task<DataSet> ExucuteDataSetAsync(CommandType cmdType, string  cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdString;
            if(sqlParameters != null)
            {
                cmd.Parameters.AddRange(sqlParameters.ToArray());
            }
            cmd.Connection = cn;
            await Task.Run(() => da.Fill(ds));
            cmd.Parameters.Clear();

            return ds;
        }

        public async Task<DataTable> ExecuteDataTableAsync(CommandType cmdType, String cmdString , IEnumerable<SqlParameter> sqlParameters= null)
        {
            var ds = await this.ExucuteDataSetAsync(cmdType, cmdString, sqlParameters);
            return ds.Tables[0];
        }

        public async Task<DbResponse> ExecuteScalarAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = cmdType,
                    CommandText = cmdString
                };

                if (sqlParameters != null)
                {
                    cmd.Parameters.AddRange(sqlParameters.ToArray());
                }

                cmd.Connection = cn;

                if (cn.State != ConnectionState.Open)
                {
                    await cn.OpenAsync();
                }

                //object result = await cmd.ExecuteScalarAsync();
                object result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                //cn.Close();
                //cn.Dispose();
                return new DbResponse() { Response = result };
            }
            catch (Exception ex)
            {
                httpContextAccessor.HttpContext.RiseError(ex);

                return new DbResponse
                {
                    Message = ex.Message,
                    HasError = true
                };
            }
        }

        public async Task<DbResponse> ExecuteNonQueryAsync(CommandType cmdType, string cmdString, IEnumerable<SqlParameter> sqlParameters = null)
        {
            var cmd = new SqlCommand
            {
                CommandType = cmdType,
                CommandText = cmdString
            };
            if (sqlParameters != null)
            {
                cmd.Parameters.AddRange(sqlParameters.ToArray());
            }

            cmd.Connection = cn;
            int response = 0;
            await cn.OpenAsync();
            try
            {
                //response = await cmd.ExecuteNonQueryAsync();
                response = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return new DbResponse() { Message = "success" };
            }
            catch (SqlException ex)
            {
                httpContextAccessor.HttpContext.RiseError(ex);

                return new DbResponse
                {
                    HasError = true,
                    Message = ex.Message,
                    Response = response
                };
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    await cn.CloseAsync();
                }
            }
        }






        public void Dispose()
        {
            if (cn != null)
                cn.Dispose();
        }
    }
}
