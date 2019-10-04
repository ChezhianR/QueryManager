using System.Collections.Generic;

namespace Query_Manager
{
    class Query
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueryText { get; set; }
        public List<Company> SelectedCompanies { get; set; }
    }
    class Company
    {
        public string CompanyName { get; set; }
        public int CompanyNumber { get; set; }
        public string SQLServer { get; set; }
        public string CompanyDatabase { get; set; }
    }
}
