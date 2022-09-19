using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using System.Configuration;

namespace TestTask
{
    internal class Program
    {
        // Генеротор листа длинной от 20 до 100 элементов, в диапазоне значений от -100 до 100
        public static List<int> GenerateNumbers()
        {
            Random random = new Random();
            List<int> numbers = new List<int>();
            for (int i = 0; i < random.Next(20, 101); i++)
            {
                numbers.Add(random.Next(-100, 101));
            }
            return numbers;    
        }

        // Конвертация листа в строку
        public static void PrintList(List<int> list)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                result.AppendFormat("{0} ", list[i]);
            }
            Console.WriteLine(result.ToString());
        }

        // Вспомогательный метод для сортировок (меняет значения под индексами i и j местами)
        static void Swap(List<int> numbers, int i, int j)
        {
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        // Метод сортировки пузырьком
        public static void BubbleSort(List<int> numbers)
        {
            for(int i = 0; i < numbers.Count; i++)
            {
                for(int j = i + 1; j < numbers.Count; j++)
                {
                    if(numbers[j] < numbers[i])
                    {
                        Swap(numbers, i, j);
                    }
                }
            }
        }

        // Метод сортировки перемешиванием
        public static void ShakeSort(List<int> numbers)
        {
            int left = 0, right = numbers.Count - 1;

            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        Swap(numbers, i, i + 1);
                    }
                }
                right--;
                for (int i = right; i > left; i--)
                {
                    if (numbers[i - 1] > numbers[i])
                    {
                        Swap(numbers, i - 1, i);
                    }
                }
                left++;
            }
        }

        // Метод сортировки вставкой
        public static void InsertionSort(List<int> numbers)
        {
            for(int i = 1; i < numbers.Count; i++)
            {
                int k = i;
                while (k > 0)
                {
                    if (numbers[k - 1] > numbers[k])
                    {
                        Swap(numbers, k - 1, k);
                    }
                    k--;
                }
            }
        }

        // Метод сортировки по умолчанию
        public static void DefaultSort(List<int> numbers) => numbers.Sort();

        // Случайный выбор метода сортировки
        public static void SortList(List<int> numbers)
        {
            Random random = new Random();
            int randomChoise = random.Next(1, 5);
            switch(randomChoise)
            {
                case 1:
                    BubbleSort(numbers);
                    break;
                case 2:
                    ShakeSort(numbers);
                    break;
                case 3:
                    InsertionSort(numbers);
                    break;
                case 4:
                    DefaultSort(numbers);
                    break;
            }
        }

        static void Main(string[] args)
        {
            List<int> numbers = GenerateNumbers(); // Сгенерирован лист numbers

            PrintList(numbers);          // Вывод numbers на консоль
            SortList(numbers);           // Сортировка numbers
            PrintList(numbers);          // Вывод отсортированного numbers на консоль

            // Отправка листа numbers по указанному в App.config адресу на rest api сервер:

            string url = ConfigurationManager.AppSettings["url"];
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddJsonBody(numbers);
            var response = client.Post(request);

            Console.ReadKey(); // Ожидание ввода
        }
    }
}
