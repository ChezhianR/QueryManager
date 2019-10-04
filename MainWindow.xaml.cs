
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Query_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            btn_Query.IsEnabled = txt_qury.Text.Trim().Length > 0 ? true : false;
        }

        private async void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            if (cmd_companies.SelectedItems.Count > 0)
            {
                DatabaseConnections db = new DatabaseConnections();

                Query posted = new Query()
                {
                    Password = txt_password.Password,
                    UserName = txt_Uname.Text,
                    QueryText = txt_qury.Text.Trim(),
                    SelectedCompanies = (cmd_companies.SelectedItems).Cast<Company>().ToList()
                };
                // get selected companies 
                //var x = (cmd_companies.SelectedItems).Cast<Company>().ToList();
                //  get Query 
                //var selectedQuery = txt_qury.Text.Trim();
                // send to DB 
                var resultdata = Task<System.Data.DataTable>.Factory.StartNew(() => db.GetDataForSelectedCompanies(posted));
                ShowProgress("Querying Database....");

                await resultdata;
                Prg_bar.IsIndeterminate = false;

                // Load datagrid
                dg_result.ItemsSource = resultdata.Result.DefaultView;

                lbl_sta_text.Content = "Completed";

                //var t = Task<bool>.Factory.StartNew(() => db.GetDataForSelectedCompanies((cmd_companies.SelectedItems).Cast<Company>().ToList(), selectedQuery));

            }
        }
        bool _enableQueries;
        private async void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            // Try connection DB with Given Credentials
            var db = new DatabaseConnections
            {
                UserName = txt_Uname.Text.Trim(),
                Password = txt_password.Password.Trim()
            };
            var checkcred = Task<bool>.Factory.StartNew(() => db.CheckCredentials());
            ShowProgress("Conecting ....");

            await checkcred;
            Prg_bar.IsIndeterminate = false;
            _enableQueries = checkcred.Result;
            if (_enableQueries == false)
            {
                lbl_sta_text.Content = "User Name or password incorrect";
            }
            else
            {
                lbl_sta_text.Content = "Connected Successfully";
                await LoadCompanies(db);
            }
        }

        private async Task LoadCompanies(DatabaseConnections db)
        {
            var LoadCompanies = Task.Factory.StartNew(() => db.GetCompanies());
            ShowProgress("Loading Companies..");
            await LoadCompanies;
            cmd_companies.ItemsSource = LoadCompanies.Result;
            cmd_companies.DisplayMemberPath = "CompanyName";
            cmd_companies.IsEnabled = true;
            Prg_bar.IsIndeterminate = false;
        }

        private void ShowProgress(string statusText)
        {
            Prg_bar.IsIndeterminate = true;
            lbl_sta_text.Content = statusText;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            dg_result.SelectAllCells();
            dg_result.ClipboardCopyMode = System.Windows.Controls.DataGridClipboardCopyMode.IncludeHeader;
            System.Windows.Input.ApplicationCommands.Copy.Execute(null, dg_result);
            string results = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue); ;
            dg_result.UnselectAllCells();
            if (results != null)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\" + "result.csv");
                file.WriteLine(results);
                file.Close();
            }

            //test to see if the DataGridView has any rows


        }
    }
}
