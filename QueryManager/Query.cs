using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryManager
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
        public int CompanyNumber { get; set; }
        public string CompanyName { get; set; }

        public string SQLServer { get; set; }
        public string CompanyDatabase { get; set; }

        private IList<Company> _companies;
        public IList<Company> Companies
        {
            get
            {
                if (_companies == null)
                    _companies = new List<Company>();
                return _companies;
            }
            set { _companies = value; }
        }
    }
}