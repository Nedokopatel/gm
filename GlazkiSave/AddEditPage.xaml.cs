using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agents _currentAgent = new Agents();
        public AddEditPage(Agents selectedAgents)
        {
            InitializeComponent();

            if (selectedAgents != null)
                _currentAgent = selectedAgents;

            DataContext = _currentAgent;
            var type = GlazkiSaveEntities.GetContext().Agents.Select(t => t.TypeAgent).Distinct().ToList();
            ComboType.ItemsSource = type;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentAgent.TypeAgent == null)
                errors.AppendLine("Выберите тип агента");
            if (string.IsNullOrEmpty(_currentAgent.NameAgent))
                errors.AppendLine("Укажите название агента");
            if (_currentAgent.Priority < 1)
                errors.AppendLine("Укажите приоритет");
           // if (_currentAgent.LogoAgent == null)
               // _currentAgent.LogoAgent = null;

            
            if (string.IsNullOrEmpty(_currentAgent.Director))
                errors.AppendLine("Укажите имя директора");
            if (string.IsNullOrEmpty(_currentAgent.Phone))
                errors.AppendLine("Укажите номер телефона");
            if (string.IsNullOrEmpty(_currentAgent.Email))
                errors.AppendLine("Укажите корректный Email");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentAgent.IdAgent == 0)
                GlazkiSaveEntities.GetContext().Agents.Add(_currentAgent);

            try
            {
                GlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.Navigate(new AgentListPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
