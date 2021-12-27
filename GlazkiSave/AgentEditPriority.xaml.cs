using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace GlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для AgentEditPriority.xaml
    /// </summary>
    public partial class AgentEditPriority : Window
    {
        private static Agents _editableAgent;
        private static List<int> IDAgents;
        public AgentEditPriority(List<VW_AgentDetailsDB> agents)
        {
            InitializeComponent();
            var context = GlazkiSaveEntities.GetContext();
            var selectedAgents = agents.OrderByDescending(x => x.Приоритет).ToList();
            IDAgents = selectedAgents.Select(x => x.IdAgent).ToList();
            var currentID = selectedAgents[0].IdAgent;
            var currentAgent = context.Agents.FirstOrDefault(x => x.IdAgent == currentID);
            _editableAgent = currentAgent;
            DataContext = _editableAgent;
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var context = GlazkiSaveEntities.GetContext();
            foreach (int id in IDAgents)
            {
                var agent = context.Agents.FirstOrDefault(x => x.IdAgent == id);
                agent.Priority = _editableAgent.Priority;
            }
            context.SaveChanges();
            Close();
        }
    }
}
