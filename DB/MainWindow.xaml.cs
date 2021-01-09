using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace HW_DB_Boroday
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        DBClass dBClass = new DBClass();

        //регулярные выражения для корректировки ввода
        Regex numberRegex = new Regex(@"^[0-9]$");


        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, e) => { LabelCurrentDateTime.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); };
            timer.Start();
            
            //Подписываем поля для ввода на событие
            FirstTaskBiddingId.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            FirstTaskSecuritiesId.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            FirstTaskSecuritiesAmount.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            SecondTaskBrokerId.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            DBClass.closeConnection();
            timer.Stop();
            this.Close();
        }

        //обработчик события PreviewTextInput для элемента textBox
        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //проверяем или подходит введенный символ нашему правилу
            Match matchTwoSymbols = numberRegex.Match(e.Text);
            //и проверяем или выполняется условие
            //если количество символов в строке меньше
            //если введенный символ не подходит нашему правилу
            if (!matchTwoSymbols.Success)
            {
                //то обработка события прекращается и ввода неправильного символа не происходит
                e.Handled = true;
            }
        }


        private void ShowDbButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == FirstShowDbButton)
            {
                if (FirstComboBoxAllTables.Text == "Не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else 
                {
                    dBClass.ShowDB(FirstComboBoxAllTables.Text);
                }
            }
            else if (sender == SecondShowDbButton)
            {
                if (SecondComboBoxAllTables.Text == "Не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dBClass.ShowDB(SecondComboBoxAllTables.Text);
                }
            }
            else if (sender == ThirdShowDbButton)
            {
                if (ThirdComboBoxAllTables.Text == "Не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dBClass.ShowDB(ThirdComboBoxAllTables.Text);
                }
            }
            else if (sender == FourthShowDbButton)
            {
                if (FourthComboBoxAllTables.Text == "Не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dBClass.ShowDB(FourthComboBoxAllTables.Text);
                }
            }
            else if (sender == FifthShowDbButton)
            {
                if (FifthComboBoxAllTables.Text == "не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dBClass.ShowDB(FifthComboBoxAllTables.Text);
                }
            }
            else if (sender == SixthShowDbButton)
            {
                if (SixthComboBoxAllTables.Text == "Не выбрано")
                {
                    System.Windows.MessageBox.Show("Выберите БД.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dBClass.ShowDB(SixthComboBoxAllTables.Text);
                }
            }
        }

        private void FirstTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.FirstTask();
        }
        private void SecondTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.SecondTask();
        }

        private void ThirdTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.ThirdTask();
        }

        private void FourthTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.FourthTask();
        }

        private void FifthTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.FifthTask();
        }

        private void SixthTaskRunButton_Click(object sender, RoutedEventArgs e)
        {
            dBClass.SixthTask();
        }

        private void ThirdTaskTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            ThirdTaskTimeLabelFrom.Content = ThirdTaskTimePickerFrom.Value.ToString("HH:mm:ss");
        }

        private void ThirdTaskTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            ThirdTaskTimeLabelTo.Content = ThirdTaskTimePickerTo.Value.ToString("HH:mm:ss");
        }

        private void ThirdTaskDatePickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = ThirdTaskDatePickerFrom.SelectedDate;
            ThirdTaskDateLabelFrom.Content = selectedDate.Value.Date.ToString("yyyy-MM-dd");
        }

        private void ThirdTaskDatePickerTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = ThirdTaskDatePickerTo.SelectedDate;
            ThirdTaskDateLabelTo.Content = selectedDate.Value.Date.ToString("yyyy-MM-dd");
        }

        private void FifthTaskTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            FifthTaskTimeLabelFrom.Content = FifthTaskTimePickerFrom.Value.ToString("HH:mm:ss");
        }

        private void FifthTaskTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            ThirdTaskTimeLabelTo.Content = FifthTaskTimePickerTo.Value.ToString("HH:mm:ss");
        }

        private void FifthTaskDatePickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = FifthTaskDatePickerFrom.SelectedDate;
            FifthTaskDateLabelFrom.Content = selectedDate.Value.Date.ToString("yyyy-MM-dd");
        }

        private void FifthTaskDatePickerTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = FifthTaskDatePickerTo.SelectedDate;
            FifthTaskDateLabelTo.Content = selectedDate.Value.Date.ToString("yyyy-MM-dd");
        }


        private void SixthTaskTimePicker_ValueChanged(object sender, EventArgs e)
        {
            SixthTaskTimeLabel.Content = SixthTaskTimePicker.Value.ToString("HH:mm:ss");
        }

        private void SixthTaskDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = SixthTaskDatePicker.SelectedDate;
            SixthTaskDateLabel.Content = selectedDate.Value.Date.ToString("yyyy-MM-dd");
        }

        private void SetTaskInfo()
        {
            FirstTaskTBlock.Text = "Задание\nРеализовать транзакции покупка и продажа ценных бумаг (хранимая процедура).";
            SecondTaskTBlock.Text = "Задание\nНайти, какие ценные бумаги заданный брокер никогда не продавал бирже (функция).";
            ThirdTaskTBlock.Text = "Задание\nПодсчитать доход брокерской конторы (учитывая, что брокер отчисляет конторе 20 %" +
                " своего дохода) за определённый период времени (функция).";
            FourthTaskTBlock.Text = "Задание\nНайти самую прибыльную за все время работы биржи ценную бумагу (функция).";
            FifthTaskTBlock.Text = "Задание\nПодсчитать доход биржи за определенный период времени то каждому виду ценных бумаг, " +
                "учитывая, что доход бирже\n дают только сделки по продаже биржей ценной бумаги.Для подсчета дохода биржи" +
                " из суммы продаж данной ценной\n бумаги нужно вычесть сумму ее приобретений биржей и зарплату брокеров (функция).";
            SixthTaskTBlock.Text = "Задание\nОпределить, сколько ценных бумаг каждого вида находится на бирже на конец заданного рабочего дня (функция).";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThirdTaskTimeLabelTo.Content = ThirdTaskTimePickerTo.Value.ToString("HH:mm:ss");
            ThirdTaskTimeLabelFrom.Content = ThirdTaskTimePickerFrom.Value.ToString("HH:mm:ss");
            ThirdTaskDateLabelTo.Content = DateTime.Now.ToString("yyyy-MM-dd");
            ThirdTaskDateLabelFrom.Content = DateTime.Now.ToString("yyyy-MM-dd");

            FifthTaskTimeLabelTo.Content = FifthTaskTimePickerTo.Value.ToString("HH:mm:ss");
            FifthTaskTimeLabelFrom.Content = FifthTaskTimePickerFrom.Value.ToString("HH:mm:ss");
            FifthTaskDateLabelTo.Content = DateTime.Now.ToString("yyyy-MM-dd");
            FifthTaskDateLabelFrom.Content = DateTime.Now.ToString("yyyy-MM-dd");

            SixthTaskTimeLabel.Content = SixthTaskTimePicker.Value.ToString("HH:mm:ss");
            SixthTaskDateLabel.Content = DateTime.Now.ToString("yyyy-MM-dd");

            SetTaskInfo();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Программа дает возможность взаимодействовать с БД, отправлять и получать данные с помощью пользовательского интерфейса. " +
                "На каждой вкладке находится задание, поля ввода и кнопка вызова функции (процедуры). " +
                "После выполнения внизу формы будет выведен результат в виде таблцы. Для получения текущего состояния таблиц " +
                "нужно выбрать одну из выпадающего списка и нажать кнопку \"Показать таблицу\". При наведении на элемент интерфейса можно " +
                "получить дополнительну информацию.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
