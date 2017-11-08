using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryManager
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
            dt = new DataTable();
        }

        public bool CheckCredentials( )
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

        void InitializeDataTables(string db,string command)
        {
            _dbConnectionString = string.Format(db, UserName, Password);
            con = new SqlConnection { ConnectionString = _dbConnectionString };
            sqlcmd = new SqlCommand { CommandText = command, Connection = con, CommandType = CommandType.Text };
            da = new SqlDataAdapter { SelectCommand = sqlcmd } ;
            
        }

        // Get All Active Companies for Dropdown
        internal List<Company> GetCompanies()
        {
            InitializeDataTables(_dbdetails, _selectCompaniesCommand);
            da.Fill(dt);
            return dt.AsEnumerable().Select(r => new Company
            {
                CompanyNumber = r.Field<int>("CompanyNumber"),
                CompanyName = r.Field<string>("CompanyName"),
                SQLServer = r.Field<string>("SQLServer"),
                CompanyDatabase = r.Field<string>("CompanyDatabase")
            }
            ).ToList();
        }

        internal async Task<DataView> ExecuteQuery(Query _selectedCompanies)
        {

            dt.Reset();
            ////Server=CHEZHIAN-DEVCON;Database=Test;User Id={0};Password={1};
            foreach (Company cmp in _selectedCompanies.SelectedCompanies)
            {
                string _dbdetails = "Server={0};Database={1};User ID={2};Password={3};";
                _dbdetails = string.Format(_dbdetails, cmp.SQLServer, cmp.CompanyDatabase, UserName, Password);
                InitializeDataTables(_dbdetails, _selectedCompanies.QueryText);
                await Task.Run(
                        () =>
                            {
                                try
                                {
                                    da.Fill(dt);
                                }
                                catch (Exception)
                                {

                                }
                            }
                );
            }


            return new DataView(dt);
        }

        private async Task FillRecordsAsync(Query _selectedCompanies, Company cmp)
        {

            string _dbdetails = "Server={0};Database={1};User ID={2};Password={3};";
            _dbdetails = string.Format(_dbdetails, cmp.SQLServer, cmp.CompanyDatabase, UserName, Password);
            await Task.Run(() => InitializeDataTables(_dbdetails, _selectedCompanies.QueryText));
            await Task.Run(() => da.Fill(dt));
        }

        public void Dispose()
        {
            this.Dispose();
            //throw new NotImplementedException();
        }
    }

    


}
