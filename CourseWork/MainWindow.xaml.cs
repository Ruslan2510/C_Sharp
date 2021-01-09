using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        List<Person> firstRegionInhabitant = new List<Person>();
        List<Person> secondRegionInhabitant = new List<Person>();
        List<Person> thirdRegionInhabitant = new List<Person>();
        List<Person> fourthRegionInhabitant = new List<Person>();
        List<Person> fifthRegionInhabitant = new List<Person>();
        List<Person> sixthRegionInhabitant = new List<Person>();
        List<Person> seventhRegionInhabitant = new List<Person>();
        List<Person> eighthRegionInhabitant = new List<Person>();
        List<Person> ninethRegionInhabitant = new List<Person>();
        List<Ellipse> ellipses = new List<Ellipse>();
        Random random;
        private bool flag;
        private byte[] checkArr = null;
        private bool stopFlag = false;
        private bool previousStep;
        private bool isWork;
        private int countOfLaunch = 0;
        private int speedValue;
        Window1 taskWindow;


        public MainWindow()
        {
            InitializeComponent();

            ShowObjectToolTip();

            btnStop.IsEnabled = false;

            firstInfectedAreaTbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInputWithOneSymbol);

            dotsAmountTbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInputWithThreeSymbols);
            diseaseDurationTbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInputWithThreeSymbols);
            diseaseProbabilityTbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInputWithThreeSymbols);
            immuneProbabilityTbox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInputWithThreeSymbols);
        }


        Regex inputOneSymbolRegex = new Regex(@"^[1-9]$");
        Regex inputTwoSymbolsRegex = new Regex(@"^\d{1,2}|100$");

        //обработчик события PreviewTextInput для элемента textBox
        void textBox_PreviewTextInputWithThreeSymbols(object sender, TextCompositionEventArgs e)
        {
            //проверяем или подходит введенный символ нашему правилу
            Match matchTwoSymbols = inputTwoSymbolsRegex.Match(e.Text);
            //и проверяем или выполняется условие
            //если количество символов в строке больше или равно одному либо
            //если введенный символ не подходит нашему правилу
            if ((sender as TextBox).Text.Length >= 3 || !matchTwoSymbols.Success) 
            {
                //то обработка события прекращается и ввода неправильного символа не происходит
                e.Handled = true; 
            }
        }

        void textBox_PreviewTextInputWithOneSymbol(object sender, TextCompositionEventArgs e)
        {
            //проверяем или подходит введенный символ нашему правилу
            Match matchOneSymbol = inputOneSymbolRegex.Match(e.Text);
            //и проверяем или выполняется условие
            //если количество символов в строке больше или равно одному либо
            //если введенный символ не подходит нашему правилу
            if ((sender as TextBox).Text.Length >= 1 || !matchOneSymbol.Success)
            {
                //то обработка события прекращается и ввода неправильного символа не происходит
                e.Handled = true;
            }
        }

        private void ShowInfo()
        {
            string info = "Данная программа моделирует развитие эпидемии в определенном регионе.\n" +
                "Текущая область делится на 9 областей и в зависимости от выбора области, в которой" +
                "появляется первый зараженный, проверяется текущая и соседние с ней области.\n" +
                "В каждой области задается кол-во жителей, изначально все они являются здоровыми - " +
                "зеленая метка (круг), в последствии, если житель заражается его метка становится краснов," +
                "по истечении заданного периода, житель переходит из состояния болезни в состояние человека с иммунитетом " +
                "или погибает.\nДля ввода количества жителей в каждой области, в первом поле для ввода нужно задать занчение" +
                "от 1 до 100. Далее нужно выбрать область в которой появится первый зараженный, в следующем поле - продолжительность " +
                "заболевания. В двух последних поля нужно ввести вероятность заражения значение от 1 до 100 и вероятность" +
                "выоздоровления от 1 до 100.\nС помощью слайдера можно выбрать скорость анимации, где 1 это очень быстро, а 10 " +
                "очень медленно.\nВнизу находится кнопка справки для повторного вывода данной информации.";
            MessageBox.Show(info, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowObjectToolTip()
        {
            dotsAmountTbox.ToolTip = "Количества точек, которое будет сгенерировано для каждой области.";
            firstInfectedAreaTbox.ToolTip = "Номер области с которой начинается заражение (всего 9 областей).";
            diseaseDurationTbox.ToolTip = "Длительность заболевания у одного человека, после определенного периода\nсостояние жителя изменится.";
            diseaseProbabilityTbox.ToolTip = "Вероятность заражения человека при контакте с больным (задается в процентах, значение от 0 до 100).";
            immuneProbabilityTbox.ToolTip = "Вероятность выздоровления человека по истечении заданного периода после заражения\n" +
                "(задается в процентах, значение от 0 до 100). " +
                "Если вероятность меньше ста, то оставшиеся проценты будут приходиться на вероятность гибели.";
            SliderValue.ToolTip = "Слайдер для изменения задержки отображения. При 10 скорость будет наименьшей, при 1 наибольшей.";

            FirstDataGrid.ToolTip = "Область №1";
            SecondDataGrid.ToolTip = "Область №2";
            ThirdDataGrid.ToolTip = "Область №3";
            FourthDataGrid.ToolTip = "Область №4";
            FifthDataGrid.ToolTip = "Область №5";
            SixthDataGrid.ToolTip = "Область №6";
            SeventhDataGrid.ToolTip = "Область №7";
            EighthDataGrid.ToolTip = "Область №8";
            NinethDataGrid.ToolTip = "Область №9";
        }

        //Метод, реализующий разметку области передвижения объектов
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            taskWindow = new Window1();
            taskWindow.Owner = this;
            taskWindow.Show();
            taskWindow.Owner.IsHitTestVisible = false;

            int cellSize = 10;
            for (int i = 0; i < 10; i++)
            {
                FirstDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(FirstDataGrid.Height / 100 * cellSize) });
                SecondDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SecondDataGrid.Height / 100 * cellSize) });
                ThirdDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ThirdDataGrid.Height / 100 * cellSize) });
                FourthDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(FourthDataGrid.Height / 100 * cellSize) });
                FifthDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(FifthDataGrid.Height / 100 * cellSize) });
                SixthDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SixthDataGrid.Height / 100 * cellSize) });
                SeventhDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SeventhDataGrid.Height / 100 * cellSize) });
                EighthDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(EighthDataGrid.Height / 100 * cellSize) });
                NinethDataGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(NinethDataGrid.Height / 100 * cellSize) });

                FirstDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(FirstDataGrid.Width / 100 * cellSize) });
                SecondDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SecondDataGrid.Width / 100 * cellSize) });
                ThirdDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ThirdDataGrid.Height / 100 * cellSize) });
                FourthDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(FourthDataGrid.Height / 100 * cellSize) });
                FifthDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(FifthDataGrid.Height / 100 * cellSize) });
                SixthDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SixthDataGrid.Height / 100 * cellSize) });
                SeventhDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(SeventhDataGrid.Height / 100 * cellSize) });
                EighthDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(EighthDataGrid.Height / 100 * cellSize) });
                NinethDataGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(NinethDataGrid.Height / 100 * cellSize) });
            }
            //ShowInfo();
        }

        //Метод заполнения областей населением
        private void PopulationAreasInitialization(int dotsPerArea, Person person)
        {
            int ellipseCount = 0;
            //Инициализцаия списка 1
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(0, 10) * 26;
                person.YСoordinate = random.Next(0, 10) * 26;
                person.AreaId = 1;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                firstRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 2
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(10, 20) * 26;
                person.YСoordinate = random.Next(0, 10) * 26;
                person.AreaId = 2;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                secondRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 3
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(20, 30) * 26;
                person.YСoordinate = random.Next(0, 10) * 26;
                person.AreaId = 3;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                thirdRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 4
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(0, 10) * 26;
                person.YСoordinate = random.Next(10, 20) * 26;
                person.AreaId = 4;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                fourthRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 5
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(10, 20) * 26;
                person.YСoordinate = random.Next(10, 20) * 26;
                person.AreaId = 5;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                fifthRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 6
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(20, 30) * 26;
                person.YСoordinate = random.Next(10, 20) * 26;
                person.AreaId = 6;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                sixthRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 7
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(0, 10) * 26;
                person.YСoordinate = random.Next(20, 30) * 26;
                person.AreaId = 7;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                seventhRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 8
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(10, 20) * 26;
                person.YСoordinate = random.Next(20, 30) * 26;
                person.AreaId = 8;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                eighthRegionInhabitant.Add(person);
                ellipseCount++;
            }

            //Инициализцаия списка 9
            for (int i = 0; i < dotsPerArea; i++)
            {
                person = new Person();
                person.Status = (Status)1;                              //Житель изначально здоров
                person.EllipseNumber = ellipseCount;                    //Номер фигуры для каждого жителя 
                person.XСoordinate = random.Next(20, 30) * 26;
                person.YСoordinate = random.Next(20, 30) * 26;
                person.AreaId = 9;
                person.IllnessDays = 0;
                ellipses[ellipseCount].SetValue(Canvas.LeftProperty, person.XСoordinate);
                ellipses[ellipseCount].SetValue(Canvas.TopProperty, person.YСoordinate);
                ellipses[ellipseCount].ToolTip = person.AreaId;
                ninethRegionInhabitant.Add(person);
                ellipseCount++;
            }
        }

        private void EllipsesInitialization(Ellipse ellipse, int dotsPerArea)
        {
            for (int i = 0; i < dotsPerArea * 9; i++)
            {
                ellipse = new Ellipse();
                ellipse.Fill = Brushes.Green;
                ellipse.Width = 26;
                ellipse.Height = 26;
                ellipses.Add(ellipse);
                canvasArea.Children.Add(ellipse);
            }
        }

        //Очистка
        private void ClearAreas()
        {
            firstRegionInhabitant.Clear();
            secondRegionInhabitant.Clear();
            thirdRegionInhabitant.Clear();
            fourthRegionInhabitant.Clear();
            fifthRegionInhabitant.Clear();
            sixthRegionInhabitant.Clear();
            seventhRegionInhabitant.Clear();
            eighthRegionInhabitant.Clear();
            ninethRegionInhabitant.Clear();

            for (int i = 0; i < ellipses.Count; i++)
            {
                canvasArea.Children.Remove(ellipses[i]);
            }

            ellipses.Clear();
        }

        private void SliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speedValue = Convert.ToInt32(e.NewValue);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;

            countOfLaunch++;

            StringBuilder errorMessage = new StringBuilder("", 100);
            int dotsPerArea = 0;
            try
            {
                Int32.TryParse(dotsAmountTbox.Text, out dotsPerArea);
                if (dotsPerArea < 1 || dotsPerArea > 100)
                {
                    throw new Exception("Ошибка ввода кол-ва точек (должно быть от 1 до 100);");
                }
            }
            catch(Exception ex)
            {
                errorMessage.Append($"{ex.Message}");
            }

            int infectedAreaNumber = 0;

            try
            {
                Int32.TryParse(firstInfectedAreaTbox.Text, out infectedAreaNumber);
                if (infectedAreaNumber < 1)
                {
                    throw new Exception("\nОшибка ввода номера области (должно быть от 1 до 9);");
                }
            }
            catch(Exception ex)
            {
                errorMessage.Append($"{ex.Message}");
            }

            //Период, пока человек находится в состоянии 'больной', далее будет или 'с иммунитетом' или 'мертв'
            int daysAmount = 0;
            try
            {
                Int32.TryParse(diseaseDurationTbox.Text, out daysAmount);
                if (daysAmount < 1)
                {
                    throw new Exception("\nОшибка ввода длительности заболевания (должно быть не менее 1);");
                }
            }
            catch(Exception ex)
            {
                errorMessage.Append($"{ex.Message}");
            }

            int infectProbability = 0;
            try
            {
                Int32.TryParse(diseaseProbabilityTbox.Text, out infectProbability);
                if (infectProbability < 0 || infectProbability > 100)
                {
                    throw new Exception("\nОшибка ввода вероятности заболевания (должно быть от 0 до 100);");
                }
            }
            catch(Exception ex)
            {
                errorMessage.Append($"{ex.Message}");
            }

            int immuneProbability = 0;
            try
            {
                Int32.TryParse(immuneProbabilityTbox.Text, out immuneProbability);
                if (immuneProbability < 0 || immuneProbability > 100)
                {
                    throw new Exception("\nОшибка ввода вероятности выздоровления (должно быть от 0 до 100);");
                }
            }
            catch(Exception ex)
            {
                errorMessage.Append($"{ex.Message}");
            }

            if (errorMessage.Length == 0)
            {
                CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
                CancellationToken token = cancelTokenSource.Token;

                Start(token, dotsPerArea, infectedAreaNumber, daysAmount, infectProbability, immuneProbability, speedValue);

                if (isWork && countOfLaunch > 1 && previousStep)
                {
                    daysAmount++;
                    cancelTokenSource.Cancel();
                }
            }
            else
            {
                MessageBox.Show(errorMessage.ToString(), "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                errorMessage.Clear();
            }
        }

        async private void Start(CancellationToken token, int dotsPerArea, int infectedAreaNumber,
            int daysAmount, double infectProbability, double immuneProbability, int displaySpeed)
        {
            btnStop.IsEnabled = true;

            ClearAreas();



            Ellipse ellipse = null;

            EllipsesInitialization(ellipse, dotsPerArea);

            Person person = null;
            random = new Random();
            PopulationAreasInitialization(dotsPerArea, person);


            // 0 - все здоровы
            // 1 - проверяется
            // 2 - есть зараженные
            if (checkArr == null)
            {
                checkArr = new byte[9];
            }
            for (int i = 0; i < checkArr.Length; i++)
            {
                checkArr[i] = 0;
            }

            SetFirstInfected(infectedAreaNumber, ref checkArr);

            previousStep = false;

            if(isWork)
            {
                previousStep = true;
            }

            flag = false;
            isWork = true;

            int amountOfHealth = 0;
            int amountOfImmune = 0;
            int amountOfDead = 0;


            int passedDaysNumber = 0;

            while (!flag)
            {
                if (token.IsCancellationRequested)
                {
                    isWork = false;
                    return;
                }

                if (stopFlag)
                {
                    var result = MessageBox.Show("Программа приостановлена.\nНажмите \"OK\" для продолжения.", "Пауза", MessageBoxButton.OK);
                    while (stopFlag)
                    {
                        if (result == MessageBoxResult.OK)
                        {
                            stopFlag = false;
                        }
                    }
                }


                await Task.Delay(displaySpeed * 100);
                CheckAreasCondition(ref checkArr);
                PersonMovement();
                ChangeAreaForPerson();
                SetPersonIllness(infectProbability / 100);
                CheckPersonCondition(daysAmount, immuneProbability / 100);
                passedDaysNumber++;
            }
            InhabitantCondition(ref amountOfHealth, ref amountOfImmune, ref amountOfDead);
            MessageBox.Show($"За период {passedDaysNumber - 2} дней из {9 * dotsPerArea} жителей\n" +
                $"{amountOfHealth} остались здоровы;\n" +
                $"{amountOfImmune} получили иммунитет;\n" +
                $"{amountOfDead} погибли.", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            isWork = false;
        }

        private void InhabitantCondition(ref int amountOfHealth, ref int amountOfImmune, ref int amountOfDead)
        {
            for (int i = 0; i < firstRegionInhabitant.Count; i++)
            {
                if(firstRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (firstRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (firstRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < secondRegionInhabitant.Count; i++)
            {
                if (secondRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (secondRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (secondRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < thirdRegionInhabitant.Count; i++)
            {
                if (thirdRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (thirdRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (thirdRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < fourthRegionInhabitant.Count; i++)
            {
                if (fourthRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (fourthRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (fourthRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < fifthRegionInhabitant.Count; i++)
            {
                if (fifthRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (fifthRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (fifthRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < sixthRegionInhabitant.Count; i++)
            {
                if (sixthRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (sixthRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (sixthRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < seventhRegionInhabitant.Count; i++)
            {
                if (seventhRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (seventhRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (seventhRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < eighthRegionInhabitant.Count; i++)
            {
                if (eighthRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (eighthRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (eighthRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }

            for (int i = 0; i < ninethRegionInhabitant.Count; i++)
            {
                if (ninethRegionInhabitant[i].Status == (Status)1)
                {
                    amountOfHealth++;
                }
                else if (ninethRegionInhabitant[i].Status == (Status)2)
                {
                    amountOfImmune++;
                }
                else if (ninethRegionInhabitant[i].Status == (Status)4)
                {
                    amountOfDead++;
                }
            }
        }

        private void SetFirstInfected(int number, ref byte[] checkArr)
        {
            int personNumber = random.Next(0, firstRegionInhabitant.Count);

            switch (number)
            {
                case 1:
                    firstRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[firstRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[0] = 2;
                    break;

                case 2:
                    secondRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[secondRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[1] = 2;
                    break;

                case 3:
                    thirdRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[thirdRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[2] = 2;
                    break;

                case 4:
                    fourthRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[fourthRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[3] = 2;
                    break;

                case 5:
                    fifthRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[fifthRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[4] = 2;
                    break;

                case 6:
                    sixthRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[sixthRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[5] = 2;
                    break;

                case 7:
                    seventhRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[seventhRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[6] = 2;
                    break;

                case 8:
                    eighthRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[eighthRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[7] = 2;
                    break;

                case 9:
                    ninethRegionInhabitant[personNumber].Status = (Status)3;
                    ellipses[ninethRegionInhabitant[personNumber].EllipseNumber].Fill = Brushes.Red;
                    checkArr[8] = 2;
                    break;
            }

        }

        private void CheckAreasCondition(ref byte[] checkArr)
        {
            //Первая область
            if (checkArr[0] == 1 || checkArr[0] == 2 || checkArr[1] == 2 || checkArr[3] == 2 || checkArr[5] == 2)
            {
                checkArr[0] = 1;
                for (int i = 0; i < firstRegionInhabitant.Count; i++)
                {
                    if (firstRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[0] = 2;
                    }
                }
            }
            else
            {
                checkArr[0] = 0;
            }

            //Вторая область
            if (checkArr[1] == 1 || checkArr[1] == 2 || checkArr[0] == 2 || checkArr[2] == 2 ||
                checkArr[3] == 2 || checkArr[4] == 2 || checkArr[5] == 2)
            {
                checkArr[1] = 1;
                for (int i = 0; i < secondRegionInhabitant.Count; i++)
                {
                    if (secondRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[1] = 2;
                    }
                }
            }
            else
            {
                checkArr[1] = 0;
            }

            //Третяя область
            if (checkArr[2] == 1 || checkArr[2] == 2 || checkArr[1] == 2 || checkArr[4] == 2 || checkArr[5] == 2)
            {
                checkArr[2] = 1;
                for (int i = 0; i < thirdRegionInhabitant.Count; i++)
                {
                    if (thirdRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[2] = 2;
                    }
                }
            }
            else
            {
                checkArr[2] = 0;
            }

            //Четвертая область
            if (checkArr[3] == 1 || checkArr[3] == 2 || checkArr[0] == 2 || checkArr[1] == 2 ||
                checkArr[4] == 2 || checkArr[6] == 2 || checkArr[7] == 2)
            {
                checkArr[3] = 1;
                for (int i = 0; i < fourthRegionInhabitant.Count; i++)
                {
                    if (fourthRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[3] = 2;
                    }
                }
            }
            else
            {
                checkArr[3] = 0;
            }

            //Пятая область
            if (checkArr[4] == 1 || checkArr[4] == 2 || checkArr[0] == 2 || checkArr[1] == 2 ||
                checkArr[2] == 2 || checkArr[3] == 2 || checkArr[4] == 2 || checkArr[5] == 2 ||
                checkArr[6] == 2 || checkArr[7] == 2 || checkArr[8] == 2)
            {
                checkArr[4] = 1;
                for (int i = 0; i < fifthRegionInhabitant.Count; i++)
                {
                    if (fifthRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[4] = 2;
                    }
                }
            }
            else
            {
                checkArr[4] = 0;
            }

            //Шестая область
            if (checkArr[5] == 1 || checkArr[5] == 2 || checkArr[1] == 2 || checkArr[2] == 2 ||
                checkArr[4] == 2 || checkArr[7] == 2 || checkArr[8] == 2)
            {
                checkArr[5] = 1;
                for (int i = 0; i < sixthRegionInhabitant.Count; i++)
                {
                    if (sixthRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[5] = 2;
                    }
                }
            }
            else
            {
                checkArr[5] = 0;
            }

            //Седьмая область
            if (checkArr[6] == 1 || checkArr[6] == 2 || checkArr[3] == 2 || checkArr[4] == 2 || checkArr[7] == 2)
            {
                checkArr[6] = 1;
                for (int i = 0; i < seventhRegionInhabitant.Count; i++)
                {
                    if (seventhRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[6] = 2;
                    }
                }
            }
            else
            {
                checkArr[6] = 0;
            }

            //Восьмая область
            if (checkArr[7] == 1 || checkArr[7] == 2 || checkArr[3] == 2 || checkArr[4] == 2 ||
                checkArr[5] == 2 || checkArr[6] == 2 || checkArr[8] == 2)
            {
                checkArr[7] = 1;
                for (int i = 0; i < eighthRegionInhabitant.Count; i++)
                {
                    if (eighthRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[7] = 2;
                    }
                }
            }
            else
            {
                checkArr[7] = 0;
            }

            //Девятая область
            if (checkArr[8] == 1 || checkArr[8] == 2 || checkArr[4] == 2 || checkArr[5] == 2 || checkArr[7] == 2)
            {
                checkArr[8] = 1;
                for (int i = 0; i < ninethRegionInhabitant.Count; i++)
                {
                    if (ninethRegionInhabitant[i].Status == (Status)3)
                    {
                        checkArr[8] = 2;
                    }
                }
            }
            else
            {
                checkArr[8] = 0;
            }

            if (!checkArr.Cast<byte>().Any(x => x == 2))
            {
                flag = true;
            }
        }

        private void PersonMovement()
        {
            int newXCoordinate = 0;
            int newYCoordinate = 0;

            int cellSize = 26;

            //Движение в первой сетке
            for (int i = 0; i < firstRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (firstRegionInhabitant[i].XСoordinate == 0 && firstRegionInhabitant[i].YСoordinate == 0)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (firstRegionInhabitant[i].XСoordinate == 0 && firstRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        firstRegionInhabitant[i].AreaId = 4;
                        ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                }
                else if (firstRegionInhabitant[i].XСoordinate == cellSize * 9 && firstRegionInhabitant[i].YСoordinate == 0)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        firstRegionInhabitant[i].AreaId = 2;
                        ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (firstRegionInhabitant[i].XСoordinate == cellSize * 9 && firstRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        firstRegionInhabitant[i].AreaId = 5;
                        ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newXCoordinate > 0)
                    {
                        firstRegionInhabitant[i].AreaId = 2;
                        ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newYCoordinate > 0)
                    {
                        firstRegionInhabitant[i].AreaId = 4;
                        ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                }
                else if (firstRegionInhabitant[i].XСoordinate == 0 && newXCoordinate < 0)
                {
                    newXCoordinate = 0;
                }
                else if (firstRegionInhabitant[i].XСoordinate == cellSize * 9 && newXCoordinate > 0)
                {
                    firstRegionInhabitant[i].AreaId = 2;
                    ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                }
                else if (firstRegionInhabitant[i].YСoordinate == 0 && newYCoordinate < 0)
                {
                    newYCoordinate = 0;
                }
                else if (firstRegionInhabitant[i].YСoordinate == cellSize * 9 && newYCoordinate > 0)
                {
                    firstRegionInhabitant[i].AreaId = 4;
                    ellipses[firstRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                }

                if (firstRegionInhabitant[i].Status != (Status)4)
                {
                    firstRegionInhabitant[i].XСoordinate += newXCoordinate;
                    firstRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[firstRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, firstRegionInhabitant[i].XСoordinate);
                ellipses[firstRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, firstRegionInhabitant[i].YСoordinate);
            }

            //Движение во второй сетке
            for (int i = 0; i < secondRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (secondRegionInhabitant[i].XСoordinate == cellSize * 10 && secondRegionInhabitant[i].YСoordinate == 0)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        secondRegionInhabitant[i].AreaId = 1;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (secondRegionInhabitant[i].XСoordinate == cellSize * 10 && secondRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 4;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                    else if (newXCoordinate < 0)
                    {
                        secondRegionInhabitant[i].AreaId = 1;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                    }
                    else if (newYCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 5;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                }
                else if (secondRegionInhabitant[i].XСoordinate == cellSize * 19 && secondRegionInhabitant[i].YСoordinate == 0)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 3;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (secondRegionInhabitant[i].XСoordinate == cellSize * 19 && secondRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 6;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                    else if (newXCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 3;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                    }
                    else if (newYCoordinate > 0)
                    {
                        secondRegionInhabitant[i].AreaId = 5;
                        ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                }
                else if (secondRegionInhabitant[i].XСoordinate == cellSize * 10 && newXCoordinate < 0)
                {
                    secondRegionInhabitant[i].AreaId = 1;
                    ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                }
                else if (secondRegionInhabitant[i].XСoordinate == cellSize * 19 && newXCoordinate > 0)
                {
                    secondRegionInhabitant[i].AreaId = 3;
                    ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                }
                else if (secondRegionInhabitant[i].YСoordinate == 0 && newYCoordinate < 0)
                {
                    newYCoordinate = 0;
                }
                else if (secondRegionInhabitant[i].YСoordinate == cellSize * 9 && newYCoordinate > 0)
                {
                    secondRegionInhabitant[i].AreaId = 5;
                    ellipses[secondRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                }

                if (secondRegionInhabitant[i].Status != (Status)4)
                {
                    secondRegionInhabitant[i].XСoordinate += newXCoordinate;
                    secondRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[secondRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, secondRegionInhabitant[i].XСoordinate);
                ellipses[secondRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, secondRegionInhabitant[i].YСoordinate);
            }

            //Движение в третьей сетке
            for (int i = 0; i < thirdRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (thirdRegionInhabitant[i].XСoordinate == cellSize * 20 && thirdRegionInhabitant[i].YСoordinate == 0)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        thirdRegionInhabitant[i].AreaId = 2;
                        ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (thirdRegionInhabitant[i].XСoordinate == cellSize * 20 && thirdRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        thirdRegionInhabitant[i].AreaId = 5;
                        ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newXCoordinate < 0)
                    {
                        thirdRegionInhabitant[i].AreaId = 2;
                        ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newYCoordinate > 0)
                    {
                        thirdRegionInhabitant[i].AreaId = 6;
                        ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                }
                else if (thirdRegionInhabitant[i].XСoordinate == cellSize * 29 && thirdRegionInhabitant[i].YСoordinate == cellSize * 0)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (thirdRegionInhabitant[i].XСoordinate == cellSize * 29 && thirdRegionInhabitant[i].YСoordinate == cellSize * 9)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        thirdRegionInhabitant[i].AreaId = 6;
                        ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                }
                else if (thirdRegionInhabitant[i].XСoordinate == cellSize * 20 && newXCoordinate < 0)
                {
                    thirdRegionInhabitant[i].AreaId = 2;
                    ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                }
                else if (thirdRegionInhabitant[i].XСoordinate == cellSize * 29 && newXCoordinate > 0)
                {
                    newXCoordinate = 0;
                }
                else if (thirdRegionInhabitant[i].YСoordinate == 0 && newYCoordinate < 0)
                {
                    newYCoordinate = 0;
                }
                else if (thirdRegionInhabitant[i].YСoordinate == cellSize * 9 && newYCoordinate > 0)
                {
                    thirdRegionInhabitant[i].AreaId = 6;
                    ellipses[thirdRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                }

                if (thirdRegionInhabitant[i].Status != (Status)4)
                {
                    thirdRegionInhabitant[i].XСoordinate += newXCoordinate;
                    thirdRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[thirdRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, thirdRegionInhabitant[i].XСoordinate);
                ellipses[thirdRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, thirdRegionInhabitant[i].YСoordinate);
            }

            //Движение в четвертой сетке
            for (int i = 0; i < fourthRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (fourthRegionInhabitant[i].XСoordinate == 0 && fourthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 1;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                    }
                }
                else if (fourthRegionInhabitant[i].XСoordinate == 0 && fourthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 7;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                    }
                }
                else if (fourthRegionInhabitant[i].XСoordinate == cellSize * 9 && fourthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 2;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newXCoordinate > 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 5;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newYCoordinate < 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 1;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                    }
                }
                else if (fourthRegionInhabitant[i].XСoordinate == cellSize * 9 && fourthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 8;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newXCoordinate > 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 5;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newYCoordinate > 0)
                    {
                        fourthRegionInhabitant[i].AreaId = 7;
                        ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                    }
                }
                else if (fourthRegionInhabitant[i].XСoordinate == 0 && newXCoordinate < 0)
                {
                    newXCoordinate = 0;
                }
                else if (fourthRegionInhabitant[i].XСoordinate == cellSize * 9 && newXCoordinate > 0)
                {
                    fourthRegionInhabitant[i].AreaId = 5;
                    ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                }
                else if (fourthRegionInhabitant[i].YСoordinate == cellSize * 10 && newYCoordinate < 0)
                {
                    fourthRegionInhabitant[i].AreaId = 1;
                    ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                }
                else if (fourthRegionInhabitant[i].YСoordinate == cellSize * 19 && newYCoordinate > 0)
                {
                    fourthRegionInhabitant[i].AreaId = 7;
                    ellipses[fourthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                }

                if (fourthRegionInhabitant[i].Status != (Status)4)
                {
                    fourthRegionInhabitant[i].XСoordinate += newXCoordinate;
                    fourthRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[fourthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, fourthRegionInhabitant[i].XСoordinate);
                ellipses[fourthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, fourthRegionInhabitant[i].YСoordinate);
            }

            //Движение в пятой сетке
            for (int i = 0; i < fifthRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (fifthRegionInhabitant[i].XСoordinate == cellSize * 10 && fifthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 1;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "1";
                    }
                    else if (newXCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 4;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                    else if (newYCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 2;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                }
                else if (fifthRegionInhabitant[i].XСoordinate == cellSize * 10 && fifthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 7;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                    }
                    else if (newXCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 4;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                    else if (newYCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 8;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                }
                else if (fifthRegionInhabitant[i].XСoordinate == cellSize * 19 && fifthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 3;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                    }
                    else if (newXCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 6;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                    else if (newYCoordinate < 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 2;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                }
                else if (fifthRegionInhabitant[i].XСoordinate == cellSize * 19 && fifthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 9;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                    }
                    else if (newXCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 6;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                    else if (newYCoordinate > 0)
                    {
                        fifthRegionInhabitant[i].AreaId = 8;
                        ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                }
                else if (fifthRegionInhabitant[i].XСoordinate == cellSize * 10 && newXCoordinate < 0)
                {
                    fifthRegionInhabitant[i].AreaId = 4;
                    ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                }
                else if (fifthRegionInhabitant[i].XСoordinate == cellSize * 19 && newXCoordinate > 0)
                {
                    fifthRegionInhabitant[i].AreaId = 6;
                    ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                }
                else if (fifthRegionInhabitant[i].YСoordinate == cellSize * 10 && newYCoordinate < 0)
                {
                    fifthRegionInhabitant[i].AreaId = 2;
                    ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                }
                else if (fifthRegionInhabitant[i].YСoordinate == cellSize * 19 && newYCoordinate > 0)
                {
                    fifthRegionInhabitant[i].AreaId = 8;
                    ellipses[fifthRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                }

                if (fifthRegionInhabitant[i].Status != (Status)4)
                {
                    fifthRegionInhabitant[i].XСoordinate += newXCoordinate;
                    fifthRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[fifthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, fifthRegionInhabitant[i].XСoordinate);
                ellipses[fifthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, fifthRegionInhabitant[i].YСoordinate);
            }

            //Движение в шестой сетке
            for (int i = 0; i < sixthRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (sixthRegionInhabitant[i].XСoordinate == cellSize * 20 && sixthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 2;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "2";
                    }
                    else if (newXCoordinate < 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 5;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newYCoordinate < 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 3;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                    }
                }
                else if (sixthRegionInhabitant[i].XСoordinate == cellSize * 20 && sixthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 8;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newXCoordinate < 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 5;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newYCoordinate > 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 9;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                    }
                }
                else if (sixthRegionInhabitant[i].XСoordinate == cellSize * 29 && sixthRegionInhabitant[i].YСoordinate == cellSize * 10)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 3;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                    }
                }
                else if (sixthRegionInhabitant[i].XСoordinate == cellSize * 29 && sixthRegionInhabitant[i].YСoordinate == cellSize * 19)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        sixthRegionInhabitant[i].AreaId = 9;
                        ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                    }
                }
                else if (sixthRegionInhabitant[i].XСoordinate == cellSize * 20 && newXCoordinate < 0)
                {
                    sixthRegionInhabitant[i].AreaId = 5;
                    ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                }
                else if (sixthRegionInhabitant[i].XСoordinate == cellSize * 29 && newXCoordinate > 0)
                {
                    newXCoordinate = 0;
                }
                else if (sixthRegionInhabitant[i].YСoordinate == cellSize * 10 && newYCoordinate < 0)
                {
                    sixthRegionInhabitant[i].AreaId = 3;
                    ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "3";
                }
                else if (sixthRegionInhabitant[i].YСoordinate == cellSize * 19 && newYCoordinate > 0)
                {
                    sixthRegionInhabitant[i].AreaId = 9;
                    ellipses[sixthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                }

                if (sixthRegionInhabitant[i].Status != (Status)4)
                {
                    sixthRegionInhabitant[i].XСoordinate += newXCoordinate;
                    sixthRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[sixthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, sixthRegionInhabitant[i].XСoordinate);
                ellipses[sixthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, sixthRegionInhabitant[i].YСoordinate);
            }

            //Движение в седьмой сетке
            for (int i = 0; i < seventhRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (seventhRegionInhabitant[i].XСoordinate == 0 && seventhRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        seventhRegionInhabitant[i].AreaId = 4;
                        ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                }
                else if (seventhRegionInhabitant[i].XСoordinate == 0 && seventhRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (seventhRegionInhabitant[i].XСoordinate == cellSize * 9 && seventhRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        seventhRegionInhabitant[i].AreaId = 5;
                        ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newXCoordinate > 0)
                    {
                        seventhRegionInhabitant[i].AreaId = 8;
                        ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newYCoordinate < 0)
                    {
                        seventhRegionInhabitant[i].AreaId = 4;
                        ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                }
                else if (seventhRegionInhabitant[i].XСoordinate == cellSize * 9 && seventhRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        seventhRegionInhabitant[i].AreaId = 8;
                        ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }

                }
                else if (seventhRegionInhabitant[i].XСoordinate == 0 && newXCoordinate < 0)
                {
                    newXCoordinate = 0;
                }
                else if (seventhRegionInhabitant[i].XСoordinate == cellSize * 9 && newXCoordinate > 0)
                {
                    seventhRegionInhabitant[i].AreaId = 8;
                    ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                }
                else if (seventhRegionInhabitant[i].YСoordinate == cellSize * 29 && newYCoordinate > 0)
                {
                    newYCoordinate = 0;
                }
                else if (seventhRegionInhabitant[i].YСoordinate == cellSize * 20 && newYCoordinate < 0)
                {
                    seventhRegionInhabitant[i].AreaId = 4;
                    ellipses[seventhRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                }

                if (seventhRegionInhabitant[i].Status != (Status)4)
                {
                    seventhRegionInhabitant[i].XСoordinate += newXCoordinate;
                    seventhRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[seventhRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, seventhRegionInhabitant[i].XСoordinate);
                ellipses[seventhRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, seventhRegionInhabitant[i].YСoordinate);
            }

            //Движение в восьмой сетке
            for (int i = 0; i < eighthRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (eighthRegionInhabitant[i].XСoordinate == cellSize * 10 && eighthRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 4;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "4";
                    }
                    else if (newXCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 7;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                    }
                    else if (newYCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 5;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                }
                else if (eighthRegionInhabitant[i].XСoordinate == cellSize * 10 && eighthRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 7;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (eighthRegionInhabitant[i].XСoordinate == cellSize * 19 && eighthRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 9;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (eighthRegionInhabitant[i].XСoordinate == cellSize * 19 && eighthRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 6;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                    else if (newXCoordinate > 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 9;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                    }
                    else if (newYCoordinate < 0)
                    {
                        eighthRegionInhabitant[i].AreaId = 5;
                        ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                }
                else if (eighthRegionInhabitant[i].XСoordinate == cellSize * 10 && newXCoordinate < 0)
                {
                    eighthRegionInhabitant[i].AreaId = 7;
                    ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "7";
                }
                else if (eighthRegionInhabitant[i].XСoordinate == cellSize * 19 && newXCoordinate > 0)
                {
                    eighthRegionInhabitant[i].AreaId = 9;
                    ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "9";
                }
                else if (eighthRegionInhabitant[i].YСoordinate == cellSize * 29 && newYCoordinate > 0)
                {
                    newYCoordinate = 0;
                }
                else if (eighthRegionInhabitant[i].YСoordinate == cellSize * 20 && newYCoordinate < 0)
                {
                    eighthRegionInhabitant[i].AreaId = 5;
                    ellipses[eighthRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                }


                if (eighthRegionInhabitant[i].Status != (Status)4)
                {
                    eighthRegionInhabitant[i].XСoordinate += newXCoordinate;
                    eighthRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[eighthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, eighthRegionInhabitant[i].XСoordinate);
                ellipses[eighthRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, eighthRegionInhabitant[i].YСoordinate);
            }

            //Движение в девятой сетке
            for (int i = 0; i < ninethRegionInhabitant.Count; i++)
            {
                newXCoordinate = random.Next(-1, 2) * cellSize;
                newYCoordinate = random.Next(-1, 2) * cellSize;

                if (ninethRegionInhabitant[i].XСoordinate == cellSize * 20 && ninethRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate < 0 && newYCoordinate < 0)
                    {
                        ninethRegionInhabitant[i].AreaId = 5;
                        ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "5";
                    }
                    else if (newXCoordinate < 0)
                    {
                        ninethRegionInhabitant[i].AreaId = 8;
                        ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newYCoordinate < 0)
                    {
                        ninethRegionInhabitant[i].AreaId = 6;
                        ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }
                }
                else if (ninethRegionInhabitant[i].XСoordinate == cellSize * 20 && ninethRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate < 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate < 0)
                    {
                        ninethRegionInhabitant[i].AreaId = 8;
                        ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (ninethRegionInhabitant[i].XСoordinate == cellSize * 29 && ninethRegionInhabitant[i].YСoordinate == cellSize * 29)
                {
                    if (newXCoordinate > 0 && newYCoordinate > 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate > 0)
                    {
                        newYCoordinate = 0;
                    }
                }
                else if (ninethRegionInhabitant[i].XСoordinate == cellSize * 29 && ninethRegionInhabitant[i].YСoordinate == cellSize * 20)
                {
                    if (newXCoordinate > 0 && newYCoordinate < 0)
                    {
                        newXCoordinate = 0;
                        newYCoordinate = 0;
                    }
                    else if (newXCoordinate > 0)
                    {
                        newXCoordinate = 0;
                    }
                    else if (newYCoordinate < 0)
                    {
                        ninethRegionInhabitant[i].AreaId = 6;
                        ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                    }

                }
                else if (ninethRegionInhabitant[i].XСoordinate == cellSize * 20 && newXCoordinate < 0)
                {
                    ninethRegionInhabitant[i].AreaId = 8;
                    ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "8";
                }
                else if (ninethRegionInhabitant[i].XСoordinate == 29 * cellSize && newXCoordinate > 0)
                {
                    newXCoordinate = 0;
                }
                else if (ninethRegionInhabitant[i].YСoordinate == 29 * cellSize && newYCoordinate > 0)
                {
                    newYCoordinate = 0;
                }
                else if (ninethRegionInhabitant[i].YСoordinate == 20 * cellSize && newYCoordinate < 0)
                {
                    ninethRegionInhabitant[i].AreaId = 6;
                    ellipses[ninethRegionInhabitant[i].EllipseNumber].ToolTip = "6";
                }

                if (ninethRegionInhabitant[i].Status != (Status)4)
                {
                    ninethRegionInhabitant[i].XСoordinate += newXCoordinate;
                    ninethRegionInhabitant[i].YСoordinate += newYCoordinate;
                }
                ellipses[ninethRegionInhabitant[i].EllipseNumber].SetValue(Canvas.LeftProperty, ninethRegionInhabitant[i].XСoordinate);
                ellipses[ninethRegionInhabitant[i].EllipseNumber].SetValue(Canvas.TopProperty, ninethRegionInhabitant[i].YСoordinate);
            }
        }

        private void SetPersonIllness(double infectProbability)
        {
            double value = 0;
            //I сетка
            for (int i = 0; i < firstRegionInhabitant.Count; i++)
            {
                if (firstRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < firstRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (firstRegionInhabitant[i].XСoordinate == firstRegionInhabitant[j].XСoordinate &&
                            firstRegionInhabitant[i].YСoordinate == firstRegionInhabitant[j].YСoordinate && 
                            firstRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                firstRegionInhabitant[j].Status = (Status)3;
                                ellipses[firstRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //II сетка
            for (int i = 0; i < secondRegionInhabitant.Count; i++)
            {
                if (secondRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < secondRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (secondRegionInhabitant[i].XСoordinate == secondRegionInhabitant[j].XСoordinate &&
                            secondRegionInhabitant[i].YСoordinate == secondRegionInhabitant[j].YСoordinate &&
                            secondRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                secondRegionInhabitant[j].Status = (Status)3;
                                ellipses[secondRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //III сетка
            for (int i = 0; i < thirdRegionInhabitant.Count; i++)
            {
                if (thirdRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < thirdRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (thirdRegionInhabitant[i].XСoordinate == thirdRegionInhabitant[j].XСoordinate &&
                            thirdRegionInhabitant[i].YСoordinate == thirdRegionInhabitant[j].YСoordinate &&
                            thirdRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                thirdRegionInhabitant[j].Status = (Status)3;
                                ellipses[thirdRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //IV сетка
            for (int i = 0; i < fourthRegionInhabitant.Count; i++)
            {
                if (fourthRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < fourthRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (fourthRegionInhabitant[i].XСoordinate == fourthRegionInhabitant[j].XСoordinate &&
                            fourthRegionInhabitant[i].YСoordinate == fourthRegionInhabitant[j].YСoordinate &&
                            fourthRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                fourthRegionInhabitant[j].Status = (Status)3;
                                ellipses[fourthRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //V сетка
            for (int i = 0; i < fifthRegionInhabitant.Count; i++)
            {
                if (fifthRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < fifthRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (fifthRegionInhabitant[i].XСoordinate == fifthRegionInhabitant[j].XСoordinate &&
                            fifthRegionInhabitant[i].YСoordinate == fifthRegionInhabitant[j].YСoordinate &&
                            fifthRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                fifthRegionInhabitant[j].Status = (Status)3;
                                ellipses[fifthRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //VI сетка
            for (int i = 0; i < sixthRegionInhabitant.Count; i++)
            {
                if (sixthRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < sixthRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (sixthRegionInhabitant[i].XСoordinate == sixthRegionInhabitant[j].XСoordinate &&
                            sixthRegionInhabitant[i].YСoordinate == sixthRegionInhabitant[j].YСoordinate &&
                            sixthRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                sixthRegionInhabitant[j].Status = (Status)3;
                                ellipses[sixthRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //VII сетка
            for (int i = 0; i < seventhRegionInhabitant.Count; i++)
            {
                if (seventhRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < seventhRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (seventhRegionInhabitant[i].XСoordinate == seventhRegionInhabitant[j].XСoordinate &&
                            seventhRegionInhabitant[i].YСoordinate == seventhRegionInhabitant[j].YСoordinate &&
                            seventhRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                seventhRegionInhabitant[j].Status = (Status)3;
                                ellipses[seventhRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //VIII сетка
            for (int i = 0; i < eighthRegionInhabitant.Count; i++)
            {
                if (eighthRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < eighthRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (eighthRegionInhabitant[i].XСoordinate == eighthRegionInhabitant[j].XСoordinate &&
                            eighthRegionInhabitant[i].YСoordinate == eighthRegionInhabitant[j].YСoordinate &&
                            eighthRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                eighthRegionInhabitant[j].Status = (Status)3;
                                ellipses[eighthRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }

            //VIII сетка
            for (int i = 0; i < ninethRegionInhabitant.Count; i++)
            {
                if (ninethRegionInhabitant[i].Status == (Status)3)
                {
                    for (int j = 0; j < ninethRegionInhabitant.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (ninethRegionInhabitant[i].XСoordinate == ninethRegionInhabitant[j].XСoordinate &&
                            ninethRegionInhabitant[i].YСoordinate == ninethRegionInhabitant[j].YСoordinate &&
                            ninethRegionInhabitant[j].Status == (Status)1)
                        {
                            value = random.NextDouble();
                            if (value <= infectProbability)
                            {
                                ninethRegionInhabitant[j].Status = (Status)3;
                                ellipses[ninethRegionInhabitant[j].EllipseNumber].Fill = Brushes.Red;
                            }
                        }
                    }
                }
            }
        }

        private void CheckPersonCondition(int daysAmount, double immuneProbability)
        {
            double value = 0;

            //I сетка
            for (int i = 0; i < firstRegionInhabitant.Count; i++)
            {
                if (firstRegionInhabitant[i].Status == (Status)3)
                {
                    if (firstRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            firstRegionInhabitant[i].Status = (Status)2;
                            ellipses[firstRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            firstRegionInhabitant[i].Status = (Status)4;
                            ellipses[firstRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    firstRegionInhabitant[i].IllnessDays++;
                }
            }

            //II сетка
            for (int i = 0; i < secondRegionInhabitant.Count; i++)
            {
                if (secondRegionInhabitant[i].Status == (Status)3)
                {
                    if (secondRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            secondRegionInhabitant[i].Status = (Status)2;
                            ellipses[secondRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            secondRegionInhabitant[i].Status = (Status)4;
                            ellipses[secondRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    secondRegionInhabitant[i].IllnessDays++;
                }
            }

            //III сетка
            for (int i = 0; i < thirdRegionInhabitant.Count; i++)
            {
                if (thirdRegionInhabitant[i].Status == (Status)3)
                {
                    if (thirdRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            thirdRegionInhabitant[i].Status = (Status)2;
                            ellipses[thirdRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            thirdRegionInhabitant[i].Status = (Status)4;
                            ellipses[thirdRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    thirdRegionInhabitant[i].IllnessDays++;
                }
            }

            //IV сетка
            for (int i = 0; i < fourthRegionInhabitant.Count; i++)
            {
                if (fourthRegionInhabitant[i].Status == (Status)3)
                {
                    if (fourthRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            fourthRegionInhabitant[i].Status = (Status)2;
                            ellipses[fourthRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            fourthRegionInhabitant[i].Status = (Status)4;
                            ellipses[fourthRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    fourthRegionInhabitant[i].IllnessDays++;
                }
            }

            //V сетка
            for (int i = 0; i < fifthRegionInhabitant.Count; i++)
            {
                if (fifthRegionInhabitant[i].Status == (Status)3)
                {
                    if (fifthRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            fifthRegionInhabitant[i].Status = (Status)2;
                            ellipses[fifthRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            fifthRegionInhabitant[i].Status = (Status)4;
                            ellipses[fifthRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    fifthRegionInhabitant[i].IllnessDays++;
                }
            }

            //VI сетка
            for (int i = 0; i < sixthRegionInhabitant.Count; i++)
            {
                if (sixthRegionInhabitant[i].Status == (Status)3)
                {
                    if (sixthRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            sixthRegionInhabitant[i].Status = (Status)2;
                            ellipses[sixthRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            sixthRegionInhabitant[i].Status = (Status)4;
                            ellipses[sixthRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    sixthRegionInhabitant[i].IllnessDays++;
                }
            }

            //VII сетка
            for (int i = 0; i < seventhRegionInhabitant.Count; i++)
            {
                if (seventhRegionInhabitant[i].Status == (Status)3)
                {
                    if (seventhRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            seventhRegionInhabitant[i].Status = (Status)2;
                            ellipses[seventhRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            seventhRegionInhabitant[i].Status = (Status)4;
                            ellipses[seventhRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    seventhRegionInhabitant[i].IllnessDays++;
                }
            }

            //VIII сетка
            for (int i = 0; i < eighthRegionInhabitant.Count; i++)
            {
                if (eighthRegionInhabitant[i].Status == (Status)3)
                {
                    if (eighthRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            eighthRegionInhabitant[i].Status = (Status)2;
                            ellipses[eighthRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            eighthRegionInhabitant[i].Status = (Status)4;
                            ellipses[eighthRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    eighthRegionInhabitant[i].IllnessDays++;
                }
            }

            //IX сетка
            for (int i = 0; i < ninethRegionInhabitant.Count; i++)
            {
                if (ninethRegionInhabitant[i].Status == (Status)3)
                {
                    if (ninethRegionInhabitant[i].IllnessDays == daysAmount)
                    {
                        value = random.NextDouble();
                        if (value <= immuneProbability)
                        {
                            ninethRegionInhabitant[i].Status = (Status)2;
                            ellipses[ninethRegionInhabitant[i].EllipseNumber].Fill = Brushes.AliceBlue;
                        }
                        else
                        {
                            ninethRegionInhabitant[i].Status = (Status)4;
                            ellipses[ninethRegionInhabitant[i].EllipseNumber].Fill = Brushes.Black;
                        }
                    }

                    ninethRegionInhabitant[i].IllnessDays++;
                }
            }
        }

        private void ChangeAreaForPerson()
        {
            //Переход из области 1 в область 2, 4 или 5
            int firstAreaPersonAmount = firstRegionInhabitant.Count;
            for (int i = 0; i < firstAreaPersonAmount; i++)
            {
                if (firstRegionInhabitant[i].AreaId == 2)
                {
                    secondRegionInhabitant.Add(firstRegionInhabitant[i]);
                    firstRegionInhabitant.RemoveAt(i);
                    firstAreaPersonAmount--;
                    i--;
                }
                else if (firstRegionInhabitant[i].AreaId == 4)
                {
                    fourthRegionInhabitant.Add(firstRegionInhabitant[i]);
                    firstRegionInhabitant.RemoveAt(i);
                    firstAreaPersonAmount--;
                    i--;
                }
                else if (firstRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(firstRegionInhabitant[i]);
                    firstRegionInhabitant.RemoveAt(i);
                    firstAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 2 в область 1, 3, 4, 5 или 6
            int seondAreaPersonAmount = secondRegionInhabitant.Count;
            for (int i = 0; i < seondAreaPersonAmount; i++)
            {
                if (secondRegionInhabitant[i].AreaId == 1)
                {
                    firstRegionInhabitant.Add(secondRegionInhabitant[i]);
                    secondRegionInhabitant.RemoveAt(i);
                    seondAreaPersonAmount--;
                    i--;
                }
                else if (secondRegionInhabitant[i].AreaId == 3)
                {
                    thirdRegionInhabitant.Add(secondRegionInhabitant[i]);
                    secondRegionInhabitant.RemoveAt(i);
                    seondAreaPersonAmount--;
                    i--;
                }
                else if (secondRegionInhabitant[i].AreaId == 4)
                {
                    fourthRegionInhabitant.Add(secondRegionInhabitant[i]);
                    secondRegionInhabitant.RemoveAt(i);
                    seondAreaPersonAmount--;
                    i--;
                }
                else if (secondRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(secondRegionInhabitant[i]);
                    secondRegionInhabitant.RemoveAt(i);
                    seondAreaPersonAmount--;
                    i--;
                }
                else if (secondRegionInhabitant[i].AreaId == 6)
                {
                    sixthRegionInhabitant.Add(secondRegionInhabitant[i]);
                    secondRegionInhabitant.RemoveAt(i);
                    seondAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 3 в область 2, 5 или 6
            int thirdAreaPersonAmount = thirdRegionInhabitant.Count;
            for (int i = 0; i < thirdAreaPersonAmount; i++)
            {
                if (thirdRegionInhabitant[i].AreaId == 2)
                {
                    secondRegionInhabitant.Add(thirdRegionInhabitant[i]);
                    thirdRegionInhabitant.RemoveAt(i);
                    thirdAreaPersonAmount--;
                    i--;
                }
                else if (thirdRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(thirdRegionInhabitant[i]);
                    thirdRegionInhabitant.RemoveAt(i);
                    thirdAreaPersonAmount--;
                    i--;
                }
                else if (thirdRegionInhabitant[i].AreaId == 6)
                {
                    sixthRegionInhabitant.Add(thirdRegionInhabitant[i]);
                    thirdRegionInhabitant.RemoveAt(i);
                    thirdAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 4 в область 1, 2, 5, 7 или 8
            int fourthAreaPersonAmount = fourthRegionInhabitant.Count;
            for (int i = 0; i < fourthAreaPersonAmount; i++)
            {
                if (fourthRegionInhabitant[i].AreaId == 1)
                {
                    firstRegionInhabitant.Add(fourthRegionInhabitant[i]);
                    fourthRegionInhabitant.RemoveAt(i);
                    fourthAreaPersonAmount--;
                    i--;
                }
                else if (fourthRegionInhabitant[i].AreaId == 2)
                {
                    secondRegionInhabitant.Add(fourthRegionInhabitant[i]);
                    fourthRegionInhabitant.RemoveAt(i);
                    fourthAreaPersonAmount--;
                    i--;

                }
                else if (fourthRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(fourthRegionInhabitant[i]);
                    fourthRegionInhabitant.RemoveAt(i);
                    fourthAreaPersonAmount--;
                    i--;
                }
                else if (fourthRegionInhabitant[i].AreaId == 7)
                {
                    seventhRegionInhabitant.Add(fourthRegionInhabitant[i]);
                    fourthRegionInhabitant.RemoveAt(i);
                    fourthAreaPersonAmount--;
                    i--;
                }
                else if (fourthRegionInhabitant[i].AreaId == 8)
                {
                    eighthRegionInhabitant.Add(fourthRegionInhabitant[i]);
                    fourthRegionInhabitant.RemoveAt(i);
                    fourthAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 5 в область 1, 2, 3, 4, 6, 7, 8 или 9 
            int fifthAreaPersonAmount = fifthRegionInhabitant.Count;
            for (int i = 0; i < fifthAreaPersonAmount; i++)
            {
                if (fifthRegionInhabitant[i].AreaId == 1)
                {
                    firstRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 2)
                {
                    secondRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 3)
                {
                    thirdRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 4)
                {
                    fourthRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 6)
                {
                    sixthRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 7)
                {
                    seventhRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 8)
                {
                    eighthRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
                else if (fifthRegionInhabitant[i].AreaId == 9)
                {
                    ninethRegionInhabitant.Add(fifthRegionInhabitant[i]);
                    fifthRegionInhabitant.RemoveAt(i);
                    fifthAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 6 в область 2, 3, 5, 8 или 9
            int sixthAreaPersonAmount = sixthRegionInhabitant.Count;
            for (int i = 0; i < sixthAreaPersonAmount; i++)
            {
                if (sixthRegionInhabitant[i].AreaId == 2)
                {
                    secondRegionInhabitant.Add(sixthRegionInhabitant[i]);
                    sixthRegionInhabitant.RemoveAt(i);
                    sixthAreaPersonAmount--;
                    i--;
                }
                else if (sixthRegionInhabitant[i].AreaId == 3)
                {
                    thirdRegionInhabitant.Add(sixthRegionInhabitant[i]);
                    sixthRegionInhabitant.RemoveAt(i);
                    sixthAreaPersonAmount--;
                    i--;
                }
                else if (sixthRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(sixthRegionInhabitant[i]);
                    sixthRegionInhabitant.RemoveAt(i);
                    sixthAreaPersonAmount--;
                    i--;
                }
                else if (sixthRegionInhabitant[i].AreaId == 8)
                {
                    eighthRegionInhabitant.Add(sixthRegionInhabitant[i]);
                    sixthRegionInhabitant.RemoveAt(i);
                    sixthAreaPersonAmount--;
                    i--;
                }
                else if (sixthRegionInhabitant[i].AreaId == 9)
                {
                    ninethRegionInhabitant.Add(sixthRegionInhabitant[i]);
                    sixthRegionInhabitant.RemoveAt(i);
                    sixthAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 7 в область 4, 5 или 8
            int seventhAreaPersonAmount = seventhRegionInhabitant.Count;
            for (int i = 0; i < seventhAreaPersonAmount; i++)
            {
                if (seventhRegionInhabitant[i].AreaId == 4)
                {
                    fourthRegionInhabitant.Add(seventhRegionInhabitant[i]);
                    seventhRegionInhabitant.RemoveAt(i);
                    seventhAreaPersonAmount--;
                    i--;
                }
                else if (seventhRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(seventhRegionInhabitant[i]);
                    seventhRegionInhabitant.RemoveAt(i);
                    seventhAreaPersonAmount--;
                    i--;
                }
                else if (seventhRegionInhabitant[i].AreaId == 8)
                {
                    eighthRegionInhabitant.Add(seventhRegionInhabitant[i]);
                    seventhRegionInhabitant.RemoveAt(i);
                    seventhAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 8 в область 4, 5, 6, 7 или 9
            int eighthAreaPersonAmount = eighthRegionInhabitant.Count;
            for (int i = 0; i < eighthAreaPersonAmount; i++)
            {
                if (eighthRegionInhabitant[i].AreaId == 4)
                {
                    fourthRegionInhabitant.Add(eighthRegionInhabitant[i]);
                    eighthRegionInhabitant.RemoveAt(i);
                    eighthAreaPersonAmount--;
                    i--;
                }
                else if (eighthRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(eighthRegionInhabitant[i]);
                    eighthRegionInhabitant.RemoveAt(i);
                    eighthAreaPersonAmount--;
                    i--;
                }
                else if (eighthRegionInhabitant[i].AreaId == 6)
                {
                    sixthRegionInhabitant.Add(eighthRegionInhabitant[i]);
                    eighthRegionInhabitant.RemoveAt(i);
                    eighthAreaPersonAmount--;
                    i--;
                }
                else if (eighthRegionInhabitant[i].AreaId == 7)
                {
                    seventhRegionInhabitant.Add(eighthRegionInhabitant[i]);
                    eighthRegionInhabitant.RemoveAt(i);
                    eighthAreaPersonAmount--;
                    i--;
                }
                else if (eighthRegionInhabitant[i].AreaId == 9)
                {
                    ninethRegionInhabitant.Add(eighthRegionInhabitant[i]);
                    eighthRegionInhabitant.RemoveAt(i);
                    eighthAreaPersonAmount--;
                    i--;
                }
            }

            //Переход из области 9 в область 5, 6 или 8
            int ninethAreaPersonAmount = ninethRegionInhabitant.Count;
            for (int i = 0; i < ninethAreaPersonAmount; i++)
            {
                if (ninethRegionInhabitant[i].AreaId == 5)
                {
                    fifthRegionInhabitant.Add(ninethRegionInhabitant[i]);
                    ninethRegionInhabitant.RemoveAt(i);
                    ninethAreaPersonAmount--;
                    i--;
                }
                else if (ninethRegionInhabitant[i].AreaId == 6)
                {
                    sixthRegionInhabitant.Add(ninethRegionInhabitant[i]);
                    ninethRegionInhabitant.RemoveAt(i);
                    ninethAreaPersonAmount--;
                    i--;
                }
                else if (ninethRegionInhabitant[i].AreaId == 8)
                {
                    eighthRegionInhabitant.Add(ninethRegionInhabitant[i]);
                    ninethRegionInhabitant.RemoveAt(i);
                    ninethAreaPersonAmount--;
                    i--;
                }
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            stopFlag = true;
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowInfo();
        }

        private void bthShowGridInfo_Click(object sender, RoutedEventArgs e)
        {
            taskWindow.Owner.IsHitTestVisible = false;
            taskWindow = new Window1();
            taskWindow.Owner = this;
            taskWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClearAreas();
            Application.Current.Shutdown();
        }
    }
}