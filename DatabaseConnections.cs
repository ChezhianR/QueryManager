using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Query_Manager
{
    class DatabaseConnections : IDisposable
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        string _dbConnectionString, _dbdetails, _selectCompaniesCommand;
        SqlConnection con;
        DataTable dt;
        SqlDataAdapter da;
        SqlCommand sqlcmd;


        public DatabaseConnections()
        {
            _dbdetails = System.Configuration.ConfigurationManager.ConnectionStrings["Prime"].ConnectionString;
            _selectCompaniesCommand = System.Configuration.ConfigurationManager.AppSettings["Select"];
        }

        public bool CheckCredentials()
        {
            try
            {
                _dbConnectionString = string.Format(_dbdetails, UserName, Password);
                con = new SqlConnection { ConnectionString = _dbConnectionString };
                var t = con.OpenAsync();
                Task.WaitAll(t);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // Get All Active Companies for Dropdown
        internal List<Company> GetCompanies()
        {
            List<Company> Companies = new List<Company>();
            try
            {
                _dbConnectionString = string.Format(_dbdetails, UserName, Password);
                dt = new DataTable();
                using (con = new SqlConnection { ConnectionString = _dbConnectionString })
                {
                    using (sqlcmd = new SqlCommand { CommandText = _selectCompaniesCommand, Connection = con, CommandType = CommandType.Text })
                    {

                        using (da = new SqlDataAdapter { SelectCommand = sqlcmd })
                        {
                            da.Fill(dt);
                        }
                    }
                }
                Companies = (from DataRow dr in dt.Rows
                             select new Company()
                             {
                                 CompanyNumber = Convert.ToInt32(dr["CompanyNumber"]),
                                 CompanyName = dr["CompanyName"].ToString(),
                                 SQLServer = dr["SQLServer"].ToString(),
                                 CompanyDatabase = dr["CompanyDatabase"].ToString()
                             }
                             ).ToList();
                return Companies;
            }
            catch (Exception e)
            {
                return Companies;
            }
        }

        internal DataTable GetDataForSelectedCompanies(Query posted)
        {
            var list = new List<DataRow>();
            string _connection = "Server={3};Database={2};User Id={0};Password={1};";
            try
            {
                foreach (Company item in posted.SelectedCompanies)
                {
                    dt = new DataTable();
                    _dbConnectionString = string.Format(_connection, posted.UserName, posted.Password, item.CompanyDatabase, item.SQLServer);
                    con = new SqlConnection { ConnectionString = _dbConnectionString };
                    using (con = new SqlConnection { ConnectionString = _dbConnectionString })
                    {
                        using (sqlcmd = new SqlCommand { CommandText = posted.QueryText, Connection = con, CommandType = CommandType.Text })
                        {

                            using (da = new SqlDataAdapter { SelectCommand = sqlcmd })
                            {
                                da.Fill(dt);
                            }
                        }
                    }
                    list.AddRange(dt.AsEnumerable().ToList());

                }

                return list.CopyToDataTable();
            }
            catch (Exception e)
            {
                return list.CopyToDataTable();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
