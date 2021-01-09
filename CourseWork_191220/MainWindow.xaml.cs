using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Radix_SecondTry
{
	public partial class MainWindow : Window
	{
		const int Digit = 10;					//Количество разрядов в десятичной системе счисления;
		List<int> Num = new List<int>();		//Список элементов, которые отображаются в верхней панели формы;
		private StackPanel[] stackPanel;		//Массив stackpanel-ей бирюзового цвета(представление лин. списка);
		private Label[] labels;					//Массив label-ов, кадый из которых в визуализации представляет собой элемент массива; 
		private Label[] verticalColumn;			//Массив для визуализации столбца с номерами разрядов;
		private Thread thread;					//Класс для работы с потоками;
		int[] arr;								//Массив для считывания элементов из формы;
		private double value;					//Значение скорости отрисовки визуализации;
		//Метод вывода информации
		private void InfoMethod()
		{
			ShowInfo();
		}
		private void ShowInfo()
		{
			MessageBoxButton buttons = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Information;
			MessageBox.Show("В данной программе реализована цифровая сортировка (LSD (сортировка элементов, начиная с меньшего разряда) Radix sort).\nДля начала, требуется выбрать один из методов " +
					"ввода данных\n(массива целых чисел).\n - При выборе первого способа нажмите кнопку \"Загрузить...\" и в проводнике выберите нужный файл с расширением \".txt\".\n" +
					" - При выборе второго способа введите  в соответствующее поле целочисленные значения через пробел, после нажмите кнопку \"Ввести\".\n" +
					" - При выборе третьего способа станет доступно поле для ввода размерности массива (целочисленное значение, < 0), поля для ввода пределов генерации " +
					"псевдослучайных чисел и кнопка \"Генерировать...\", по нажатию которой будет производится генерация случайных элементов массива" +
					" и отображение их в верхней панели.\n\n1) Кнопка \"Сортировать\" запускает сортировку массива.\n2) Шкала с ползунком задает интервал смены кадров визуализации.\n" +
					"3) Кнопка \"Загрузить...\" вызывает проводник, для указания пути к файлу с входными данными.\n4) Кнопка \"Ввести\" выводит " +
					"считанный массив в верхний текстовый элемент формы.\n5) Кнопка \"Генерировать...\" производит генерацию ПСЧ (псевдослучайных чисел) в заданном диапазоне.\n\n" +
					"Бирюзовым цветом подсвечена визуализация линейных связных списков.\nЭлементы по умолчанию являются белыми, при выборе и занесении элемена " +
					"в какую-либо бирюзовую строку, он выделяется красным цветом. Столбец с цифрами является представлением номера разряда числа, по которым " +
					"и производится данная сортировка.", "Справка", buttons, icon);
		}
		public MainWindow()
		{
			MessageBoxButton buttons = MessageBoxButton.OK;			//Стиль MessegeBox-а;
			MessageBoxImage icon = MessageBoxImage.Information;
			InitializeComponent();
			ChengedStateVisability(StateDisplayed.NoneData);		//Мтеод отображения элементов управления, в зависимости от
																	//выбора пользователем пункта меню;
			//Создание потока;
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
			{
				Thread.Sleep(300);									//Остановка потока на 300мс.;
				InfoMethod();
				MessageBox.Show("Выберите один из пунктов меню в левом верхнем углу формы.", "Помощник ʕ ᵔᴥᵔ ʔ", buttons, icon);
			});
			StartButton.IsEnabled = false;							//Отмена кликабельности кнопки "Сортировать"
			SliderValue.IsEnabled = false;							//и ползунка установки скорости отображения визуализации;
			Closed += EndProgram;
		}
		//RadioButton генерация случайных чисел в диапазоне;
		private void RandomNumber(object sender, RoutedEventArgs e)
		{
			RadioButton pressedRandom = sender as RadioButton;
			if (pressedRandom.IsChecked == true)
			{
				ChengedStateVisability(StateDisplayed.RandomData);
			}
		}
		//RadioButton загрузка информации из файла;
		private void DownloadFromFile(object sender, RoutedEventArgs e)
		{
			RadioButton pressedFromFile = sender as RadioButton;
			if (pressedFromFile.IsChecked == true)
			{
				ChengedStateVisability(StateDisplayed.DownloadFileData);
			}
		}
		//RadioButton ввод данных в форме;
		private void InputInForm(object sender, RoutedEventArgs e)
		{
			RadioButton pressedInput = sender as RadioButton;
			if (pressedInput.IsChecked == true)
			{
				ChengedStateVisability(StateDisplayed.EnterManualData);
			}
		}
		//Метод установки свойств отображения элементов в зависимости от того, какой RadioButton выбран;
		private void ChengedStateVisability(StateDisplayed state)
		{
			//DownloadButton - кнопка вызова проводника, для выбора файла с данными;

			//InputBox - поле ввода элементов непосредственно в самой форме;
			//ReadingInformationButton - кнопка считывания элементов из InputBox;

			//ArrSize_TextBlock - надпись "кол-во элементов";
			//ArrSize_TextBox - поле для ввода размерности генерируемого массива;
			//Limits_TextBlock - надпись "Пределы генерации чисел:";
			//LowerLimit_TextBlock - надпись "от:";
			//LowerLimit_TextBox - поле для ввода нижнего предела генерирования значений;
			//UpperLimit_TextBlock - надпись "до";
			//UpperLimit_TextBox - поле для ввода верхнего предела генерирования значений;
			//RandomButton - кнопка генерации псевдослучайных чисел;

			//Visability.Collapsed - объект не отображается на MainWindow;
			//Visibility.Visible - объект отображается на MainWindow;
			switch (state)
			{
				//Никакой из RadioButtons не был нажат;
				case StateDisplayed.NoneData:
					{
						CountOfDigit.Visibility = Visibility.Collapsed;
						DownloadButton.Visibility = Visibility.Collapsed;
						ReadingInformationButton.Visibility = Visibility.Collapsed;
						InputBox.Visibility = Visibility.Collapsed;
						LowerLimit_TextBox.Visibility = Visibility.Collapsed;
						LowerLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBox.Visibility = Visibility.Collapsed;
						ArrSize_TextBlock.Visibility = Visibility.Collapsed;
						ArrSize_TextBox.Visibility = Visibility.Collapsed;
						Limits_TextBlock.Visibility = Visibility.Collapsed;
						RandomButton.Visibility = Visibility.Collapsed;
						SpeedTextBlock.Visibility = Visibility.Collapsed;
						SliderValue.Visibility = Visibility.Collapsed;
						StartButton.Visibility = Visibility.Collapsed;
						ShowInfoButton.Visibility = Visibility.Collapsed;
						break;
					}
				//Выбран RadioButton считывания из файла;
				case StateDisplayed.DownloadFileData:
					{
						DownloadButton.Visibility = Visibility.Visible;
						InputBox.Visibility = Visibility.Collapsed;
						ReadingInformationButton.Visibility = Visibility.Collapsed;
						LowerLimit_TextBox.Visibility = Visibility.Collapsed;
						LowerLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBox.Visibility = Visibility.Collapsed;
						ArrSize_TextBlock.Visibility = Visibility.Collapsed;
						ArrSize_TextBox.Visibility = Visibility.Collapsed;
						Limits_TextBlock.Visibility = Visibility.Collapsed;
						RandomButton.Visibility = Visibility.Collapsed;
						StartButton.Visibility = Visibility.Visible;
						SpeedTextBlock.Visibility = Visibility.Visible;
						SliderValue.Visibility = Visibility.Visible;
						StartButton.Visibility = Visibility.Visible;
						ShowInfoButton.Visibility = Visibility.Visible;
						break;
					}
				//Выбран RadioButton ручного ввода;
				case StateDisplayed.EnterManualData:
					{
						InputBox.Visibility = Visibility.Visible;
						DownloadButton.Visibility = Visibility.Collapsed;
						ReadingInformationButton.Visibility = Visibility.Visible;
						LowerLimit_TextBox.Visibility = Visibility.Collapsed;
						LowerLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBlock.Visibility = Visibility.Collapsed;
						UpperLimit_TextBox.Visibility = Visibility.Collapsed;
						ArrSize_TextBlock.Visibility = Visibility.Collapsed;
						ArrSize_TextBox.Visibility = Visibility.Collapsed;
						Limits_TextBlock.Visibility = Visibility.Collapsed;
						RandomButton.Visibility = Visibility.Collapsed;
						SpeedTextBlock.Visibility = Visibility.Visible;
						SliderValue.Visibility = Visibility.Visible;
						StartButton.Visibility = Visibility.Visible;
						ShowInfoButton.Visibility = Visibility.Visible;
						break;
					}
				//Выбран RadioButton генерации псевдослучайных чисел;
				case StateDisplayed.RandomData:
					{
						InputBox.Visibility = Visibility.Collapsed;
						DownloadButton.Visibility = Visibility.Collapsed;
						ReadingInformationButton.Visibility = Visibility.Collapsed;
						LowerLimit_TextBox.Visibility = Visibility.Visible;
						LowerLimit_TextBlock.Visibility = Visibility.Visible;
						UpperLimit_TextBlock.Visibility = Visibility.Visible;
						UpperLimit_TextBox.Visibility = Visibility.Visible;
						ArrSize_TextBlock.Visibility = Visibility.Visible;
						ArrSize_TextBox.Visibility = Visibility.Visible;
						Limits_TextBlock.Visibility = Visibility.Visible;
						RandomButton.Visibility = Visibility.Visible;
						SpeedTextBlock.Visibility = Visibility.Visible;
						SliderValue.Visibility = Visibility.Visible;
						StartButton.Visibility = Visibility.Visible;
						ShowInfoButton.Visibility = Visibility.Visible;
						break;
					}
				default: break;
			}
		}
		//Обработчик события нажатия на кнопку "Считать..." (загрузки данных из файла);
		private void DownloadButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog() == true)
			{
				MessageBoxButton buttons = MessageBoxButton.OK;
				MessageBoxImage icon = MessageBoxImage.Warning;
				using (var reader = new StreamReader(openFileDialog.FileName))
				{
					var line = reader.ReadLine();               //Считывание элементов из файла
					string[] values = line.Split(' ');          //Перезапись строки line, как массива строк
																//отбрасывая пробелы, между элементами;
					arr = new int[values.Length];               //Создание массива целых чисел;
					for (int i = 0; i < values.Length; i++)
					{
						try
						{
							arr[i] = Convert.ToInt32(values[i]);    //Преобразование элементов из массива строк в целочисленный массив;
						}
						catch
						{
							MessageBox.Show("Неверный формат введенных данных!\nПовторите попытку ввода.", "Warning!", buttons,icon);
							return;
						}
					}
					TopTextBlock.Text = line;                   //Вывод считанного массива;
				}
				StartButton.IsEnabled = true;
				SliderValue.IsEnabled = true;
			}
		}
		//Обработчик события нажатия на кнопку "Считать" (считывание элементов из поля для ввода, находящегося на MainWindow;
		private void ReadingInformation_Click(object sender, RoutedEventArgs e)
		{
			bool flag = true;
			string str = InputBox.Text;
			MessageBoxButton buttons = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Warning;
			foreach (char item in str)
			{
				if ((item < '0' || item > '9') && !char.IsWhiteSpace(item))
				{
					MessageBox.Show("Неверный формат входной строки!", "Ошибка!",buttons,icon);
					flag = false;
					break;
				}

			}
			if (flag == true)
			{
				//Проверка правильности ввода;
				if (str == "")
				{
					MessageBox.Show("Неверный формат входной строки!", "Ошибка!",buttons, icon);
				}
				else
				{
					//регулярное выражение, заменяющее скопление пробелов одним символом " "
					str = Regex.Replace(str, @"\s+", " ");
					TopTextBlock.Text = str;
					StartButton.IsEnabled = true;
					SliderValue.IsEnabled = true;
				}
			}
		}
		//Обработчик события нажатия на кнопку "Генерировать..." (генерация псевдослучайных числел в заданном диапазоне);
		private void RandomButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxButton buttons = MessageBoxButton.OK;
			MessageBoxImage icon = MessageBoxImage.Warning;
			int randomAmount = 0,       //Количество генерируемых значений;
				upperLimit = 0,         //Нижний предел генерации;
				lowerLimit = 0;         //Верхний предел генерации;
			try
			{
				lowerLimit = Convert.ToInt32(LowerLimit_TextBox.Text);
				upperLimit = Convert.ToInt32(UpperLimit_TextBox.Text);
				randomAmount = Convert.ToInt32(ArrSize_TextBox.Text);
				if (lowerLimit > upperLimit)			//Проверка правильности ввода нижнего и верхнего предела;
				{
					int temp = lowerLimit;
					lowerLimit = upperLimit;
					upperLimit = temp;
				}
				if (lowerLimit == upperLimit)
				{
					throw new Exception("Значение верхнего и нижнего пределов\nне могут совпадать!");
				}
				if (lowerLimit < 0)
				{
					throw new Exception("Данная сортировка не поддерживает\nотрицательные значения!");
				}
				if (randomAmount <= 0)
				{
					throw new Exception("Количество генерируемых значений\nне может быть меньше или равное нулю!");
				}
				Random rand = new Random();                         //Класс генерации псевдослучайных чисел;
				arr = new int[randomAmount];
				StringBuilder line = new StringBuilder("");         //Класс создания динамической строки;
																	//Используется, так как неизвестно начальное количество элементов, которые будут добвлены в строку;
				for (int i = 0; i < arr.Length; i++)
				{
					arr[i] = rand.Next(lowerLimit, upperLimit);     //Генерация псевдослучайных чисел;
					line.Append($"{arr[i]} ");                      //Добавление элемента в строку line;
																	//В данном случае используется метод Append, вместо конкатенации строк; 
				}
				TopTextBlock.Text = line.ToString();                //Вывод сгенерированного массива в TopTextBlock;
				StartButton.IsEnabled = true;
				SliderValue.IsEnabled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "Ошибка!", buttons, icon);
			}
		}
		//Метод, которые в зависимости от действий пользователя, включит или отключит кликабельность кнопок;
		private void DisabledEnabled(bool flag)
		{
			FirstRadioButton.IsEnabled = flag;
			SecondRadioButton.IsEnabled = flag;
			ThirdRadioButton.IsEnabled = flag;
			DownloadButton.IsEnabled = flag;
			RandomButton.IsEnabled = flag;
			ReadingInformationButton.IsEnabled = flag;
			StartButton.IsEnabled = flag;
			SliderValue.IsEnabled = flag;
		}
		//Метод получает значение ползунка скорости отображения, заданное пользователем;
		private void SliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			value = Convert.ToDouble(e.NewValue);
		}
		private void StartProgram_Click(object sender, RoutedEventArgs e)
		{
			Button StartProgram = sender as Button;
			var line = TopTextBlock.Text;
			Num.Clear();											//Очистка верхней панели;
			NumPanel.Children.Clear();								//Отвязка всех дочерних элементов;
			MainPanel.Children.Clear();									
			VerticalColumn.Children.Clear();
			//Преобразование строки в массив чисел;
			arr = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Int32.Parse(s)).ToArray();
			for (int i = 0; i < arr.Length; i++)
			{
				Num.Add(arr[i]);
			}
			AddColumn();								//Метод отображения столбца разрядов;
			PrintArray();								//Вывод массива в верхнюю панель формы;
			AddPanel();									//Добавление визуализации связных списков;
			StartProgram.Visibility = Visibility.Visible;
			DisabledEnabled(false);
			CountOfDigit.Visibility = Visibility.Visible;
			int length = GetmaxLength();
			thread = new Thread(delegate ()
			{
				int temp = 0;							//Номер списка, в который будет добавлено число;
				int amountOfPasses = 0;					//Количество итераций;
				int longestNumber = GetmaxLength();		//Максимальное количество разрядов;
				while (temp != longestNumber)
				{
					Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
					{
						CountOfDigit.Text = ("Сортировка производится\n по " + (temp+1).ToString() + "-му разряду (с конца).");
					});
					RadixSortVisual(temp, value);
					temp++;
				}
				RadixSort(arr, ref amountOfPasses, length);
				StringBuilder result = new StringBuilder("");         //Класс создания динамической строки;										  
				//Используется, так как неизвестно начальное количество элементов, которые будут добвлены в строку;
				for (int i = 0; i < arr.Length; i++)
				{
					result.Append($"{arr[i]} ");                      //Добавление элемента в строку line;					  
											//В данном случае используется метод Append, вместо конкатенации строк; 
				}
				MessageBox.Show("Массив отсортирован:\n" + result.ToString() + "\nКоличество итераций: " +
					amountOfPasses.ToString(), "Результат");
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
				{
					DisabledEnabled(true);
					CountOfDigit.Visibility = Visibility.Collapsed;
				});
			});
			thread.Start();

		}
		//Метод добавления столбца с номерами разрядов;
		private void AddColumn()
		{
			verticalColumn = new Label[Digit];
			for (int i = 0; i < Digit; i++)
			{
				verticalColumn[i] = new Label();					
				verticalColumn[i].Background = Brushes.White;		
				verticalColumn[i].FontSize = 10;
				verticalColumn[i].BorderBrush = Brushes.Black;
				verticalColumn[i].Height = 45;
				verticalColumn[i].HorizontalContentAlignment = HorizontalAlignment.Center;
				verticalColumn[i].VerticalContentAlignment = VerticalAlignment.Center;
				verticalColumn[i].FontSize = 14;
				verticalColumn[i].Margin = new Thickness(5, 5, 0, 0);
				verticalColumn[i].Width = 30;
				verticalColumn[i].HorizontalAlignment = HorizontalAlignment.Center;
				verticalColumn[i].Content = i.ToString();
				verticalColumn[i].BorderThickness = new Thickness(1);
				VerticalColumn.Children.Add(verticalColumn[i]);
			}
		}
		//Добавление StackPanel-ей в MainWondow;
		private void AddPanel()
		{
			MainPanel.Children.Clear();
			stackPanel = new StackPanel[Digit];
			for (int i = 0; i < Digit; i++)
			{
				stackPanel[i] = new StackPanel();
				stackPanel[i].Name = ("panel_" + i.ToString());
				stackPanel[i].Orientation = Orientation.Horizontal;
				stackPanel[i].Height = 45;
				stackPanel[i].Margin = new Thickness(5, 5, 0, 0);
				stackPanel[i].VerticalAlignment = VerticalAlignment.Top;
				stackPanel[i].Width = 650;
				stackPanel[i].Background = Brushes.Aqua;
				stackPanel[i].RenderSize = new Size(50, 50);
				stackPanel[i].Visibility = Visibility.Visible;
				stackPanel[i].ToolTip = "Визуализация линейного списка, с помощью которого производится поразрядная сортировка элементов.";
				MainPanel.Children.Add(stackPanel[i]);
			}
		}
		//Отображение элементов в верхней панели;
		private void PrintArray()
		{
			NumPanel.Children.Clear();
			labels = new Label[Num.Count];
			for (int i = 0; i < Num.Count; i++)
			{
				labels[i] = new Label();
				labels[i].Background = Brushes.White;
				labels[i].FontSize = 10;
				labels[i].Height = 35;
				labels[i].HorizontalContentAlignment = HorizontalAlignment.Center;
				labels[i].BorderBrush = Brushes.Black;
				labels[i].HorizontalAlignment = HorizontalAlignment.Center;
				labels[i].Content = Num[i];
				labels[i].FontSize = 14;
				labels[i].Width = GetmaxLength() * 20;
				labels[i].BorderThickness = new Thickness(1);
				NumPanel.Children.Add(labels[i]);
			}
		}
		//Метод перерисовки объектов формы; 
		private Label LabelRegenerate(Label label)
		{
			return new Label()
			{
				Background = Brushes.White,
				FontSize = 10,
				Height = 40,
				HorizontalAlignment = HorizontalAlignment.Center,
				Content = label.Content,
				Width = 40,
				BorderThickness = new Thickness(1)
			};
		}
		//Метод, производящий расчет количества разрядов самого большого числа;
		private int GetmaxLength()
		{
			int length = 0;
			foreach (var item in arr)
			{
				var l = item.ToString().Length;
				if (l > length)
				{
					length = l;
				}
			}
			return length;
		}
		//Реализациия сортировки;
		private void RadixSort(int[] arr, ref int amountOfPasses, int length)
		{
			ArrayList[] lists = new ArrayList[Digit];
			for (int i = 0; i < Digit; ++i)
			{
				lists[i] = new ArrayList();
			}
			for (int step = 0; step < length; ++step)
			{
				//распределение по спискам
				for (int i = 0; i < arr.Length; ++i)
				{
					int temp = arr[i] % (int)Math.Pow(Digit, step + 1) / (int)Math.Pow(Digit, step);
					lists[temp].Add(arr[i]);
					amountOfPasses++;
				}
				//сборка
				int k = 0;
				for (int i = 0; i < Digit; ++i)
				{
					for (int j = 0; j < lists[i].Count; ++j)
					{
						arr[k++] = (int)lists[i][j];
					}
					//Удаление элементов из списков;
					lists[i].Clear();
				}
			}
		}
		//Визуализация сортировки;
		private void RadixSortVisual(int step, double speed)
		{
			//Num.Count
			int temp;
			for (int i = 0; i < Num.Count; i++)
			{
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
				{
					temp = (arr[i] % (int)Math.Pow(Digit, step + 1) / (int)Math.Pow(Digit, step));
					var parent = labels[i].Parent as StackPanel;
					labels[i].Background = Brushes.Red;
				});
				Thread.Sleep((int)(500 * speed));
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
				{
					temp = (arr[i] % (int)Math.Pow(Digit, step + 1) / (int)Math.Pow(Digit, step));
					var parent = labels[i].Parent as StackPanel;
					parent.Children.Remove(labels[i]);          //Удаление элемента из верхней панели;
					labels[i].Background = Brushes.Red;
					stackPanel[temp].Children.Add(labels[i]);	//Добавление элемента в представление списка;
				});
				Thread.Sleep((int)(speed * 600));
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
				{
					temp = (arr[i] % (int)Math.Pow(Digit, step + 1) / (int)Math.Pow(Digit, step));
					var parent = labels[i].Parent as StackPanel;
					if (parent is null)
					{
						stackPanel[temp].Children.Add(LabelRegenerate(labels[i]));
					}
					labels[i].Background = Brushes.White;
				});
				Thread.Sleep((int)(800 * speed));
			}
			for (int i = 0; i < Digit; i++)
			{
				List<Label> Temt = null;
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
				{
					var list = stackPanel[i].Children.OfType<Label>().AsQueryable();
					list = list.OrderBy(x => int.Parse(x.Content.ToString()));// LINQ	
					Temt = list.ToList();

				});
				Thread.Sleep((int)(1000 * speed));
				if (Temt.Count > 0)
				{
					for (int j = 0; j < Temt.Count; j++)
					{

						Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
						{
							stackPanel[i].Children.Remove(Temt[j]);
							NumPanel.Children.Add(Temt[j]);
						});
						Thread.Sleep((int)(400 * speed));
					}
				}
			}
		}
		//Метод отображения информации по нажатию кнопки;
		private void InfoInForm_Click(object sender, RoutedEventArgs e)
		{
			ShowInfo();
		}
		//Завершение программы, закрытие потоков;
		private void EndProgram(object sender, EventArgs e)
		{
			if (thread != null)
			{
				thread.Abort();
			}
			Close();
		}
	}
}