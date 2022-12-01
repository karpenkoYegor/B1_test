using System;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using B1_test.Data;
using B1_test.Data.Entities;

namespace B1_test;

public class Task1
{
    private int _countOfStrings = 100_000;
    private int _countOfFiles;
    private int _countOfSymbols = 10;
    private int _yearRange = 5;
    private int _minIntNum = 1;
    private int _maxIntNum = 100_000_000;
    private int _minDecNum = 1;
    private int _maxDecNum = 20;
    private int _numsAfterPoints = 8;
    private Random _random = new ();

    private DateTime _date;
    private string _engSymbols = "";
    private string _rusSymbols = "";
    private int _intNum;
    private double _decNumber;
    private object obj;
    private int _deletedStrings = 0;
    private int _addedStrings = 0;
    private int _remainingStrings;
    public Task1(int countOfFiles)
    {
        _countOfFiles = countOfFiles;
        _remainingStrings = _countOfFiles * _countOfStrings;
    }

    //Задание 1.1 Многопоточное создание файлов
    public void CreateFiles()
    {
        //Проверка на наличие папки
        if (!Directory.Exists("files"))
        {
            //Создание папки
            Directory.CreateDirectory("files");
        }
        else
        {
            //Удаление старых файлов
            Directory.Delete("files", true);
            Directory.CreateDirectory("files");
        }
        var option = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10
        };
        //Параллельный запуск процедуры создания файлов
        Parallel.For(0, _countOfFiles,
            option, i => CreateFile(i));
    }
    //Задание 1.1 Создание одного файла
    public void CreateFile(int num)
    {
        //Создание файла
        using (var sw = new StreamWriter($"files/file{num}.txt", true))
        {
            //Генерация данных
            for (int j = 0; j < _countOfStrings; j++)
            {
                _date = GetRandomDate(_yearRange);
                _engSymbols = GetEngRandomString(_countOfSymbols);
                _rusSymbols = GetRusRandomString(_countOfSymbols);
                _intNum = GetEvenNumber(_minIntNum, _maxIntNum);
                _decNumber = GetFracNumber(_minDecNum, _maxDecNum);
                //Добавление в файл генерируемых данных
                sw.WriteLine(
                    $"{_date.ToShortDateString()}||{_engSymbols}||{_rusSymbols}||{_intNum}||{_decNumber.ToString($"F{_numsAfterPoints}")}");
            }
        }
    }
    private DateTime GetRandomDate(int yearRange)
    {
        int daysDiff = (DateTime.Now - DateTime.Now.AddYears(-yearRange)).Days;
        return DateTime.Now.AddYears(-yearRange).AddDays(_random.Next(daysDiff));
    }
    private string GetEngRandomString(int countOfSymbols)
    {
        int indexSymbol;
        string engAlphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
        string symbols = "";
        for (int i = 0; i < countOfSymbols; i++)
        {
            indexSymbol = _random.Next(0, engAlphabet.Length);
            symbols += Convert.ToChar(engAlphabet[indexSymbol]);
        }

        return symbols;
    }
    private string GetRusRandomString(int countOfSymbols)
    {
        int indexSymbol;
        string rusAlphabet = "ЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬХЪЖЭБЮйцукенгшщзхъфывапролджэячсмитьбю";
        string symbols = "";
        for (int i = 0; i < countOfSymbols; i++)
        {
            indexSymbol = _random.Next(0, rusAlphabet.Length);
            symbols += Convert.ToChar(rusAlphabet[indexSymbol]);
        }

        return symbols;
    }
    private double GetFracNumber(int from, int to)
    {
        return (double)_random.Next(from, to) + _random.NextDouble();
    }
    private int GetEvenNumber(int from, int to)
    {
        int number = _random.Next(from, to);
        if (number % 2 == 0)
            return number;
        else
            return number + 1;
    }
    //Задание 1.2 Процедура соединения файлов 
    public void JoinFilesAsync(string subString)
    {
        //Проверка наличия файла с данными
        if (File.Exists("joinedFiles.txt"))
        {
            File.Delete("joinedFiles.txt");
        }
        //Объект-заглушка для синхронизации потоков
        obj = new();
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < _countOfFiles; i++)
        {
            tasks.Add(JoinFileAsync(i, subString));
        }
        Task.WaitAll(tasks.ToArray());
        Console.WriteLine($"Удалено {_deletedStrings}");
    }
    //Задание 1.2 Асинхронная обёртка процедуры добавления данных одного файла в общий файл
    public async Task JoinFileAsync(int num, string subString)
    {
        //Асинхронный запуск метода
        await Task.Run(() => JoinFile(num, subString));
    }
    //Задание 1.2 Процедура добавления данных одного файла в общий файл
    public void JoinFile(int num, string subString)
    {
        int deletedStrings = 0;
        //Буфер для хранения данных из файла
        StringBuilder sb = new StringBuilder();
        Console.WriteLine($"file{num}.txt");
        //Считывания данных
        using (StreamReader sr = new StreamReader($"files/file{num}.txt"))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (!line.Contains(subString))
                {
                    sb.Append(line + "\n");
                }
                else
                {
                    deletedStrings++;
                }
            }
        }
        //Синхронизация потоков
        lock (obj)
        {
            //Запись данных одного файла в общий файл
            using (var sw = new StreamWriter($"joinedFiles.txt", true))
            {
                sw.WriteLine(sb.ToString());
                _deletedStrings += deletedStrings;
            }
        }
    }
    //Задание 1.3 добавление файлов в бд
    public void AddStringsInDb()
    {
        //Ограничивание кол-ва потоков
        //Наиболее оптимальное 10 параллелей
        //Тестирование проводилось на процессоре core i7-8650h, добавление файлов проходило около 5-7 минут
        //Памяти потреблялось около 3 гб в пике загрузки
        var option = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10
        };
        //Параллельный запуск добавления данных в бд
        Parallel.For(0, _countOfFiles,
            option, i => ReadFile(i));
    }
    //Задание 1.3 Считывание и добавление данных из файла в бд
    public void ReadFile(int num)
    {
        //Подключение к бд
        using (B1Repository repository = new B1Repository())
        {
            //Считывание файла
            using (StreamReader sr = new StreamReader($"files/file{num}.txt"))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] str = line.Split("||");
                    repository.AddString(new MyString
                    {
                        Date = DateTime.Parse(str[0]),
                        EngSymbols = str[1],
                        RusSymbols = str[2],
                        EvenNumber = Convert.ToInt32(str[3]),
                        FracNumber = Convert.ToDouble(str[4])
                    });
                }
            }
            //Сохранение изменений в бд
            repository.Save();
        }
        _addedStrings += _countOfStrings;
        Console.WriteLine($"Добавлено {_addedStrings} строк, осталось {_remainingStrings - _addedStrings}");
    }
}