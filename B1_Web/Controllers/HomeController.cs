using B1_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using B1_Web.Data;
using B1_Web.Data.Entities;
using CsvHelper;
using ExcelDataReader;
using Microsoft.Extensions.Primitives;

namespace B1_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string[] _fileNames;
        public HomeController(ILogger<HomeController> logger, IRepositoryWrapper repository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _fileNames = Directory.GetFiles($"{_webHostEnvironment.WebRootPath}\\files");
            for (int i = 0; i < _fileNames.Length; i++)
            {
                _fileNames[i] = _fileNames[i].Substring(_fileNames[i].LastIndexOf(@"\") + 1);
            }
        }

        public IActionResult Index(IndexViewModel model, string fileName = null)
        {
            if(fileName == null)
                model = new IndexViewModel()
                {
                    FileNames = _fileNames
                };
            else
            {
                model = new IndexViewModel()
                {
                    FileNames = _fileNames,
                    DataFileModel = BuildData(fileName)
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            //Получаем путь сохранения файла
            string fileName = $"{_webHostEnvironment.WebRootPath}\\files\\{file.FileName}";
            //Копируем файл в сохранённый путь
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyToAsync(fileStream);
                fileStream.FlushAsync();
            }
            //Добавляем имя файла в бд
            _repository.DataFile.Create(new DataFile { Name = file.FileName });
            _repository.Save();
            //Получаем список загруженных файлов
            _fileNames = Directory.GetFiles($"{_webHostEnvironment.WebRootPath}\\files");
            for (int i = 0; i < _fileNames.Length; i++)
            {
                _fileNames[i] = _fileNames[i].Substring(_fileNames[i].LastIndexOf(@"\") + 1);
            }
            //Добавляем данные из файла в бд
            AddDataFromFileInDb(file.FileName);
            var model = new IndexViewModel()
            {
                FileNames = _fileNames
            };
            return View(model);
        }
        //Считывание данных из excel файла в бд
        public void AddDataFromFileInDb(string fileName)
        {
            var path = Path.Join(_webHostEnvironment.WebRootPath, "files", fileName);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //Открытие файла
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                _logger.LogInformation(fileName);
                _logger.LogInformation(stream.Name);
                //Считывание excel файла
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string value = "";
                        if(reader.GetValue(0) != null)
                            value = reader.GetValue(0).ToString();
                        int num;
                        if (value.Length == 4 && int.TryParse(value, out num))
                        {
                            var account = new BusinessAccount
                            {
                                BalanceId = num,
                                FileNameId = _repository.DataFile.FindByFileName(f => f.Name == fileName).Id,
                                IncomingBalance = new IncomingBalance
                                {
                                    IncomingBalanceId = num,
                                    Active = reader.GetDouble(1),
                                    Passive = reader.GetDouble(2)
                                },
                                Turnover = new Turnover
                                {
                                    TurnoverId = num,
                                    Debit = reader.GetDouble(3),
                                    Credit = reader.GetDouble(4)
                                },
                                OutgoingBalance = new OutgoingBalance
                                {
                                    OutgoingBalanceId = num,
                                    Active = reader.GetDouble(5),
                                    Passive = reader.GetDouble(6)
                                },
                                TypeBusinessAccountId = num / 1000
                            };
                            _repository.BusinessAccount.Create(account);
                        }
                    }
                }
            }
            _repository.Save();
        }
        //Сборка и структурирование данных из бд
        public DataFileModel BuildData(string fileName)
        {
            int lastBalanceId = 0;
            var data = _repository.BusinessAccount.GetDataFileByFileName(fileName);
            double incomingBalanceActiveSum = 0;
            double incomingBalancePassiveSum = 0;
            double turnoverDebitSum = 0;
            double turnoverCreditSum = 0;
            double outgoingBalanceActiveSum = 0;
            double outgoingBalancePassiveSum = 0;

            double incomingBalanceActiveClassSum = 0;
            double incomingBalancePassiveClassSum = 0;
            double turnoverDebitClassSum = 0;
            double turnoverCreditClassSum = 0;
            double outgoingBalanceActiveClassSum = 0;
            double outgoingBalancePassiveClassSum = 0;

            double incomingBalanceActiveFileSum = 0;
            double incomingBalancePassiveFileSum = 0;
            double turnoverDebitFileSum = 0;
            double turnoverCreditFileSum = 0;
            double outgoingBalanceActiveFileSum = 0;
            double outgoingBalancePassiveFileSum = 0;

            List<DataRowModel> rows = new List<DataRowModel>();
            List<DataGroupModel> groups = new List<DataGroupModel>();
            List<DataClassModel> classes = new List<DataClassModel>();
            
            foreach (var account in data)
            {
                
                if (lastBalanceId != 0)
                {
                    if (lastBalanceId / 100 != account.BalanceId / 100)
                    {
                        groups.Add(new DataGroupModel()
                        {
                            GroupId = lastBalanceId / 100,
                            Rows = rows,
                            IncomingBalanceActiveSum = incomingBalanceActiveSum,
                            IncomingBalancePassiveSum = incomingBalancePassiveSum,
                            OutgoingBalanceActiveSum = outgoingBalanceActiveSum,
                            OutgoingBalancePassiveSum = outgoingBalancePassiveSum,
                            TurnoverDebitSum = turnoverDebitSum,
                            TurnoverCreditSum = turnoverCreditSum
                        });
                        rows = new List<DataRowModel>();
                        incomingBalanceActiveClassSum += incomingBalanceActiveSum;
                        incomingBalancePassiveClassSum += incomingBalancePassiveSum;
                        turnoverDebitClassSum += turnoverDebitSum;
                        turnoverCreditClassSum += turnoverCreditSum;
                        outgoingBalanceActiveClassSum += outgoingBalanceActiveSum;
                        outgoingBalancePassiveClassSum += outgoingBalancePassiveSum;

                        incomingBalanceActiveSum = 0;
                        incomingBalancePassiveSum = 0;
                        turnoverDebitSum = 0;
                        turnoverCreditSum = 0;
                        outgoingBalanceActiveSum = 0;
                        outgoingBalancePassiveSum = 0;
                    }
                    if (lastBalanceId / 1000 != account.BalanceId / 1000)
                    {
                        classes.Add(new DataClassModel
                        {
                            ClassId = lastBalanceId / 1000,
                            Name = _repository.TypeBusinessAccount.FindById(a => a.Id == lastBalanceId / 1000).Name,
                            Groups = groups,
                            IncomingBalanceActiveClassSum = incomingBalanceActiveClassSum,
                            IncomingBalancePassiveClassSum = incomingBalancePassiveClassSum,
                            OutgoingBalanceActiveClassSum = outgoingBalanceActiveClassSum,
                            OutgoingBalancePassiveClassSum = outgoingBalancePassiveClassSum,
                            TurnoverDebitClassSum = turnoverDebitClassSum,
                            TurnoverCreditClassSum = turnoverCreditClassSum

                        });
                        groups = new List<DataGroupModel>();
                        incomingBalanceActiveFileSum += incomingBalanceActiveClassSum;
                        incomingBalancePassiveFileSum += incomingBalancePassiveClassSum;
                        turnoverDebitFileSum += turnoverDebitClassSum;
                        turnoverCreditFileSum += turnoverCreditClassSum;
                        outgoingBalanceActiveFileSum += outgoingBalanceActiveClassSum;
                        outgoingBalancePassiveFileSum += outgoingBalancePassiveClassSum;

                        incomingBalanceActiveClassSum = 0;
                        incomingBalancePassiveClassSum = 0;
                        turnoverDebitClassSum = 0;
                        turnoverCreditClassSum = 0;
                        outgoingBalanceActiveClassSum = 0;
                        outgoingBalancePassiveClassSum = 0;
                    }
                }
                
                incomingBalanceActiveSum += account.IncomingBalance.Active;
                incomingBalancePassiveSum += account.IncomingBalance.Passive;
                turnoverDebitSum += account.Turnover.Debit;
                turnoverCreditSum += account.Turnover.Credit;
                outgoingBalanceActiveSum += account.OutgoingBalance.Active;
                outgoingBalancePassiveSum += account.OutgoingBalance.Passive;
                var dataRow = new DataRowModel()
                {
                    BalanceId = account.BalanceId,
                    IncomingBalanceActive = account.IncomingBalance.Active,
                    IncomingBalancePassive = account.IncomingBalance.Passive,
                    TurnoverDebit = account.Turnover.Debit,
                    TurnoverCredit = account.Turnover.Credit,
                    OutgoingBalanceActive = account.OutgoingBalance.Active,
                    OutgoingBalancePassive = account.OutgoingBalance.Passive
                };
                lastBalanceId = account.BalanceId;
                rows.Add(dataRow);
            }
            groups.Add(new DataGroupModel()
            {
                GroupId = lastBalanceId / 100,
                Rows = rows,
                IncomingBalanceActiveSum = incomingBalanceActiveSum,
                IncomingBalancePassiveSum = incomingBalancePassiveSum,
                OutgoingBalanceActiveSum = outgoingBalanceActiveSum,
                OutgoingBalancePassiveSum = outgoingBalancePassiveSum,
                TurnoverDebitSum = turnoverDebitSum,
                TurnoverCreditSum = turnoverCreditSum
            });
            incomingBalanceActiveClassSum += incomingBalanceActiveSum;
            incomingBalancePassiveClassSum += incomingBalancePassiveSum;
            turnoverDebitClassSum += turnoverDebitSum;
            turnoverCreditClassSum += turnoverCreditSum;
            outgoingBalanceActiveClassSum += outgoingBalanceActiveSum;
            outgoingBalancePassiveClassSum += outgoingBalancePassiveSum;

            classes.Add(new DataClassModel
            {
                ClassId = lastBalanceId / 1000,
                Name = _repository.TypeBusinessAccount.FindById(a => a.Id == lastBalanceId / 1000).Name,
                Groups = groups,
                IncomingBalanceActiveClassSum = incomingBalanceActiveClassSum,
                IncomingBalancePassiveClassSum = incomingBalancePassiveClassSum,
                OutgoingBalanceActiveClassSum = outgoingBalanceActiveClassSum,
                OutgoingBalancePassiveClassSum = outgoingBalancePassiveClassSum,
                TurnoverDebitClassSum = turnoverDebitClassSum,
                TurnoverCreditClassSum = turnoverCreditClassSum

            });
            incomingBalanceActiveFileSum += incomingBalanceActiveClassSum;
            incomingBalancePassiveFileSum += incomingBalancePassiveClassSum;
            turnoverDebitFileSum += turnoverDebitClassSum;
            turnoverCreditFileSum += turnoverCreditClassSum;
            outgoingBalanceActiveFileSum += outgoingBalanceActiveClassSum;
            outgoingBalancePassiveFileSum += outgoingBalancePassiveClassSum;

            DataFileModel dataFile = new DataFileModel()
            {
                Classes = classes,
                IncomingBalanceActiveFileSum = incomingBalanceActiveFileSum,
                IncomingBalancePassiveFileSum = incomingBalancePassiveFileSum,
                TurnoverDebitFileSum = turnoverDebitFileSum,
                TurnoverCreditFileSum = turnoverCreditFileSum,
                OutgoingBalanceActiveFileSum = outgoingBalanceActiveFileSum,
                OutgoingBalancePassiveFileSum = outgoingBalancePassiveFileSum,
                FileName = fileName
            };

            return dataFile;
        }

        public void SaveFile(string fileName)
        {
            string pathFile = Path.Join($"{_webHostEnvironment.WebRootPath}", "generedFiles", fileName.Substring(0, fileName.IndexOf("."))+".txt");
            StringBuilder data = new StringBuilder();
            DataFileModel file = BuildData(fileName);
            data.AppendLine($"{"",10}" +
                            $"{"ВХОДЯЩЕЕ САЛЬДО", 40}" +
                            $"{"ОБОРОТЫ", 40}" +
                            $"{"ИСХОДЯЩЕЕ САЛЬДО", 40}");
            data.AppendLine($"{"Б/сч", 10}" +
                            $"{"Актив", 20}" +
                            $"{"Пассив", 20}" +
                            $"{"Дебет", 20}" +
                            $"{"Кредит", 20}" +
                            $"{"Актив", 20}" +
                            $"{"Пассив", 20}");
            //Сборка и структурирование данных из бд в тхт файл 
            foreach (var modelClass in file.Classes)
            {
                foreach (var modelGroup in modelClass.Groups)
                {
                    foreach (var modelRow in modelGroup.Rows)
                    {
                        data.AppendLine($"{modelRow.BalanceId, 10} " +
                                          $"{@modelRow.IncomingBalanceActive,20:F2} " +
                                          $"{@modelRow.IncomingBalancePassive,20:F2} " +
                                          $"{modelRow.TurnoverDebit,20:F2} " +
                                          $"{modelRow.TurnoverCredit,20:F2} " +
                                          $"{modelRow.OutgoingBalanceActive,20:F2} " +
                                          $"{modelRow.OutgoingBalancePassive,20:F2}");
                    }

                    data.AppendLine($"{modelGroup.GroupId,10} " +
                                $"{modelGroup.IncomingBalanceActiveSum,20:F2} " +
                                $"{modelGroup.IncomingBalancePassiveSum,20:F2} " +
                                $"{modelGroup.TurnoverDebitSum,20:F2} " +
                                $"{modelGroup.TurnoverCreditSum,20:F2} " +
                                $"{modelGroup.OutgoingBalanceActiveSum,20:F2} " +
                                $"{modelGroup.OutgoingBalancePassiveSum,20:F2} ");
                }

                data.AppendLine($"{"По классу", 10}" +
                                $"{modelClass.IncomingBalanceActiveClassSum,20:F2} " +
                                $"{modelClass.IncomingBalancePassiveClassSum,20:F2} " +
                                $"{modelClass.TurnoverDebitClassSum,20:F2} " +
                                $"{modelClass.TurnoverCreditClassSum,20:F2} " +
                                $"{modelClass.OutgoingBalanceActiveClassSum,20:F2} " +
                                $"{modelClass.OutgoingBalancePassiveClassSum,20:F2} ");
            }

            data.AppendLine($"{"Баланс", 10}" +
                            $"{file.IncomingBalanceActiveFileSum,20:F2} " +
                            $"{file.IncomingBalancePassiveFileSum,20:F2} " +
                            $"{file.TurnoverDebitFileSum,20:F2} " +
                            $"{file.TurnoverCreditFileSum,20:F2} " +
                            $"{file.OutgoingBalanceActiveFileSum,20:F2} " +
                            $"{file.OutgoingBalancePassiveFileSum,20:F2} ");
            //Запись данных в файл
            System.IO.File.WriteAllText(pathFile, data.ToString());
            DownloadFile(fileName, pathFile);
        }

        public async void DownloadFile(string fileName, string filePath)
        {
            //Задаём имя файлу
            Response.Headers.ContentDisposition = $"attachment; filename={fileName.Substring(0,fileName.IndexOf("."))}.txt";
            //Скачиваем файл
            await Response.SendFileAsync(filePath);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}