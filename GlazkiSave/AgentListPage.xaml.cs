
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
    
        public partial class AgentListPage : Page
        {
            private int _currentPage = 1;
            private int _maxPage;
            public AgentListPage()
            {
                InitializeComponent();

                var context = GlazkiSaveEntities.GetContext();
                var agents = context.VW_AgentDetailsDB.ToList();

                _maxPage = agents.Count / 10;
                if (agents.Count % 10 > 0)
                    _maxPage++;
            var AgentTypes = agents.Select(x => x.Тип_агента).Distinct();
            foreach (var type in AgentTypes)
            {
                FilterComboBox.Items.Add(type);
            }
            UpdateContext(1);
            }

            private void UpdateContext(int page)
            {
                //костыыль
                if (FirstBtn is null)
                    return;

                var context = GlazkiSaveEntities.GetContext();

                //фильрация и поиск
                var SoretdAgents = context.VW_AgentDetailsDB.Where
                    (x => x.Наименование_агента.Contains(SearchQueryText.Text)).ToList();
                if (FilterComboBox.SelectionBoxItem.ToString() != "Все типы")
                    SoretdAgents = SoretdAgents.Where
                        (x => x.Тип_агента.Contains(FilterComboBox.SelectionBoxItem.ToString())).ToList();
            #region Sorting
            switch (SortComboBox.SelectionBoxItem.ToString())
            {
                case "Наименование (по возр)":
                    SoretdAgents = SoretdAgents.OrderBy(x => x.Наименование_агента).ToList();
                    break;
                case "Наименование (по убыв)":
                    SoretdAgents = SoretdAgents.OrderByDescending(x => x.Наименование_агента).ToList();
                    break;
                case "Размер скидки (по возр)":
                    SoretdAgents = SoretdAgents.OrderBy(x => x.Скидка_агента).ToList();
                    break;
                case "Размер скидки (по убыв)":
                    SoretdAgents = SoretdAgents.OrderByDescending(x => x.Скидка_агента).ToList();
                    break;
                case "Приоритет агента (по возр)":
                    SoretdAgents = SoretdAgents.OrderBy(x => x.Приоритет).ToList();
                    break;
                case "Приоритет агента (по убыв)":
                    SoretdAgents = SoretdAgents.OrderByDescending(x => x.Приоритет).ToList();
                    break;
            }
            #endregion

            _maxPage = SoretdAgents.Count / 10;
                if (SoretdAgents.Count % 10 > 0)
                    _maxPage++;

                if (AgentsList.SelectedItems.Count != 0)
                {
                    ChangePriorityBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    ChangePriorityBtn.Visibility = Visibility.Hidden;
                }

                AgentsList.ItemsSource = SoretdAgents.Skip((10 * page) - 10).Take(10);

            #region Pagging Buttons Content Change
            if (page == 1)
            {
                FirstBtn.Content = page.ToString();
                SecondBtn.Content = (page + 1).ToString();
                ThirdBtn.Content = (page + 2).ToString();
                FourBtn.Content = (page + 3).ToString();

                FirstBtn.FontWeight = FontWeights.Bold;
                SecondBtn.FontWeight = FontWeights.Normal;
                ThirdBtn.FontWeight = FontWeights.Normal;
                FourBtn.FontWeight = FontWeights.Normal;
            }
            else
            {
                switch (_maxPage - page)
                {
                    case 0:
                        FirstBtn.Content = (page - 3).ToString();
                        SecondBtn.Content = (page - 2).ToString();
                        ThirdBtn.Content = (page - 1).ToString();
                        FourBtn.Content = page.ToString();

                        FirstBtn.FontWeight = FontWeights.Normal;
                        SecondBtn.FontWeight = FontWeights.Normal;
                        ThirdBtn.FontWeight = FontWeights.Normal;
                        FourBtn.FontWeight = FontWeights.Bold;
                        break;
                    case 1:
                        FirstBtn.Content = (page - 2).ToString();
                        SecondBtn.Content = (page - 1).ToString();
                        ThirdBtn.Content = page.ToString();
                        FourBtn.Content = (page + 1).ToString();

                        FirstBtn.FontWeight = FontWeights.Normal;
                        SecondBtn.FontWeight = FontWeights.Normal;
                        ThirdBtn.FontWeight = FontWeights.Bold;
                        FourBtn.FontWeight = FontWeights.Normal;
                        break;
                    default:
                        FirstBtn.Content = (page - 1).ToString();
                        SecondBtn.Content = page.ToString();
                        ThirdBtn.Content = (page + 1).ToString();
                        FourBtn.Content = (page + 2).ToString();

                        FirstBtn.FontWeight = FontWeights.Normal;
                        SecondBtn.FontWeight = FontWeights.Bold;
                        ThirdBtn.FontWeight = FontWeights.Normal;
                        FourBtn.FontWeight = FontWeights.Normal;
                        break;
                }
            }
            #endregion
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var context = GlazkiSaveEntities.GetContext();
            VW_AgentDetailsDB agentsForEdit = AgentsList.SelectedItems[0] as VW_AgentDetailsDB;
            if (agentsForEdit == null)
                return;
            var agentForEdit = context.Agents.FirstOrDefault(x => x.IdAgent == agentsForEdit.IdAgent);
            Manager.MainFrame.Navigate(new AddEditPage(agentForEdit as Agents));
        }


        ///обработка нажатия на кнопку удалить
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var agentsForRemoving = AgentsList.SelectedItems.Cast<VW_AgentDetailsDB>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {agentsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = GlazkiSaveEntities.GetContext();
                    foreach (var item in agentsForRemoving)
                    {
                        var agent = context.Agents.FirstOrDefault(x => x.IdAgent == item.IdAgent);

                        var sale = context.ProductSale.FirstOrDefault(x => x.IdAgent == agent.IdAgent);
                        if (sale != null)
                        {
                            MessageBox.Show("Удаление запрещено");
                            return;
                        }

                        var log = context.AgentsLog.FirstOrDefault(x => x.IdAgent == agent.IdAgent);
                        if (log != null)
                        {
                            context = GlazkiSaveEntities.GetContext();
                            context.AgentsLog.Remove(log);
                            context.SaveChanges();
                        }

                        context = GlazkiSaveEntities.GetContext();
                        context.Agents.Remove(agent);
                    }

                    context = GlazkiSaveEntities.GetContext();
                    context.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    AgentsList.ItemsSource = GlazkiSaveEntities.GetContext().VW_AgentDetailsDB.ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
                _currentPage--;
            UpdateContext(_currentPage);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _maxPage)
                _currentPage++;
            UpdateContext(_currentPage);
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = Int32.Parse((sender as Button).Content.ToString());
            UpdateContext(_currentPage);
        }

        private void SearchQueryText_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            UpdateContext(_currentPage);
        }

        private void ChangePriorityBtn_Click(object sender, RoutedEventArgs e)
        {
            //берет только первый выделенный элемент для редактирования
            if (AgentsList.SelectedItems.Count > 0)
            {

                List<VW_AgentDetailsDB> selectedItems = new List<VW_AgentDetailsDB>();
                foreach (var item in AgentsList.SelectedItems)
                {
                    selectedItems.Add(item as VW_AgentDetailsDB);
                }
                var win = new AgentEditPriority(selectedItems);
                win.ShowDialog();
                UpdateContext(_currentPage);
            }
        }

        private void AgentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateContext(_currentPage);
        }

        private void FilterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            _currentPage = 1;
            UpdateContext(_currentPage);
        }

        private void SortComboBox_DropDownClosed(object sender, EventArgs e)
        {
            _currentPage = 1;
            UpdateContext(_currentPage);
        }
    }
}

