using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QueryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseConnections _dbConnections;
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

            // get selected companies 
            Query qm = new Query {
                UserName = txt_Uname.Text,
                Password = txt_password.Password,
                QueryText = txt_qury.Text.Trim(),
                SelectedCompanies = cmd_companies.SelectedItems.Cast<Company>().ToList()
            };
            //var get_results = Task<DataView>.Factory.StartNew(() => _dbConnections.ExecuteQuery(qm));
            ShowProgress("Executing .... ");



            //dg_result.ItemsSource = get_results;
            dg_result.ItemsSource = await _dbConnections.ExecuteQuery(qm);

            // Load datagrid

            ShowProgress(string.Empty);
            Prg_bar.IsIndeterminate = false;
        }
        bool _enableQueries;
        private async void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            // Try connection DB with Given Credentials
            _dbConnections = new DatabaseConnections
            {
                UserName = txt_Uname.Text.Trim(),
                Password = txt_password.Password.Trim()
            };
            var checkcred = Task<bool>.Factory.StartNew(() => _dbConnections.CheckCredentials());
            ShowProgress("Conecting ....");

            await checkcred;
            Prg_bar.IsIndeterminate = false;
            _enableQueries = checkcred.Result;
            if (_enableQueries == false)
            {
                MessageBox.Show("Wroung User Name or Password", "Wrong User Name/Password", MessageBoxButton.OK, MessageBoxImage.Error);
                //lbl_sta_text.Content = "User Name or password incorrect";
            }
            else
            {
                lbl_sta_text.Content = "Connected Successfully";
                await LoadCompanies(_dbConnections);
            }
        }

        private async Task LoadCompanies(DatabaseConnections db)
        {
            var LoadCompanies = await Task.Run(() => db.GetCompanies());
            ShowProgress("Loading Companies..");

            cmd_companies.IsEnabled = LoadCompanies.Count > 0 ? true : false;

            //cmd_companies.DisplayMemberPath = "CompanyName";
            //cmd_companies.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = LoadCompanies });
            cmd_companies.ItemsSource = LoadCompanies;

            ShowProgress(string.Empty);
            Prg_bar.IsIndeterminate = false;
        }

        private void ShowProgress(string statusText)
        {
            Prg_bar.IsIndeterminate = true;
            lbl_sta_text.Content = statusText;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //dg_result.SelectAllCells();
            //dg_result.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            //ApplicationCommands.Copy.Execute(null, dg_result);
            //string results = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue); ;

            //if (results != null)
            //{
            //    System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //    System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +"\\aa.txt" );
            //    //file.WriteLine(results.Replace(",", ""));
            //    file.Close();
            //    //MessageBox.Show("Exported Sucessfully");
            //}
            //dg_result.UnselectAllCells();

            dg_result.SelectAllCells();
            dg_result.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg_result);
            dg_result.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            try
            {
                if (result != null)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter("export.csv");
                    sw.WriteLine(result);
                    sw.Close();
                    MessageBox.Show("Exported Sucessfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
