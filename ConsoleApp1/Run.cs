using Newtonsoft.Json;
using System.Xml.Serialization;
using Aspose.Cells;
namespace PW6
{
    public class Run
    {
        public string ReadText;
        string Name, Age, Description;
        string way;
        ConsoleKeyInfo Key;
        Boolean Exist;
        List<Model> jsonList;
        string[] wayFormat, wayFormat2, words_example;
        public void MainMenu()
        {
            Console.WriteLine("Введите путь до файла (вместе с названием), который хочется открыть: ");
            Console.WriteLine("-----------------------------------------------------------------------");
            way = Console.ReadLine();

            OpenedFileAsync(way);
        }
        private async Task OpenedFileAsync(string way)
        {
            Exist = false;
            do
            {
                if (File.Exists(way))
                {
                    Exist = true;
                    Console.Clear();
                    Console.WriteLine("Сохранить файл в одном из 3-ёх форматов: (txt, json, xml) - F1. Закрыть программу - Escape.");
                    Console.WriteLine("-----------------------------------------------------------------------");
                    wayFormat = way.Split(".");

                    ReadText = File.ReadAllText(way);

                    if (wayFormat[1] == "txt")
                    {
                        Console.WriteLine(ReadText);
                    }
                    else if (wayFormat[1] == "json")
                    {
                        jsonList = JsonConvert.DeserializeObject<List<Model>>(ReadText);
                        foreach (var item in jsonList)
                        {
                            Console.WriteLine(item.Name);
                            Console.WriteLine(item.Age);
                            Console.WriteLine(item.Description);
                        }
                    }
                    else if (wayFormat[1] == "xml")
                    {
                        string[] massivName = ReadText.Split("<Name>");
                        Name = Convert.ToString(massivName[1]);
                        massivName = Name.Split("</Name>");
                        Name = massivName[0];

                        string[] massivAge = ReadText.Split("<Age>");
                        Age = Convert.ToString(massivAge[1]);
                        massivAge = Age.Split("</Age>");
                        Age = massivAge[0];

                        string[] massivDescription = ReadText.Split("<Description>");
                        Description = Convert.ToString(massivDescription[1]);
                        massivDescription = Description.Split("</Description>");
                        Description = massivDescription[0];

                        Console.WriteLine(Name);
                        Console.WriteLine(Age);
                        Console.WriteLine(Description);
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка. Файла по этому пути не существует");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                }
            } while (Exist == false);

            Buttons(wayFormat);
        }
        private void Buttons(string[] wayFormat)
        {
            do
            {
                Key = Console.ReadKey(true);

                if (Key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Выход...");
                    Environment.Exit(0);
                }
                else if (Key.Key == ConsoleKey.F1)
                {
                    Console.Clear();
                    Console.WriteLine("Введите путь до файла (вместе с форматом), куда вы хотите сохранить текст: ");
                    Console.WriteLine("-----------------------------------------------------------------------");

                    way = Console.ReadLine();
                    wayFormat2 = way.Split(".");

                    if (wayFormat2[1] == "txt")
                    {
                        if (wayFormat[1] == "txt")
                        {
                            File.WriteAllText(way, ReadText);
                        }
                        else if (wayFormat[1] == "json")
                        {
                            foreach (var item in jsonList)
                            {
                                File.WriteAllText(way, item.Name + "\n");
                                File.AppendAllText(way, item.Age + "\n");
                                File.AppendAllText(way, item.Description + "\n");
                            }
                        }
                        else if (wayFormat[1] == "xml")
                        {
                            string Text = Name + "\n" + Age + "\n" + Description;
                            File.WriteAllText(way, Text);
                        }
                    }
                    else if (wayFormat2[1] == "json")
                    {
                        if (wayFormat[1] == "json")
                        {
                            File.WriteAllText(way, ReadText);
                        }
                        else if (wayFormat[1] == "txt")
                        {
                            string[] words = ReadText.Split("\r\n");

                            Model Something = new Model(words[0], Convert.ToInt32(words[1]), words[2]);
                            List<Model> InformationAbout = new List<Model>();
                            InformationAbout.Add(Something);

                            string json = JsonConvert.SerializeObject(InformationAbout);
                            File.WriteAllText(way, json);
                        }
                        else if (wayFormat[1] == "xml")
                        {
                            Model Something = new Model(Name, Convert.ToInt32(Age), Description);
                            List<Model> InformationAbout = new List<Model>();
                            InformationAbout.Add(Something);

                            string json = JsonConvert.SerializeObject(InformationAbout);
                            File.WriteAllText(way, json);
                        }
                    }
                    else if (wayFormat2[1] == "xml")
                    {
                        if (wayFormat[1] == "xml")
                        {
                            File.WriteAllText(way, ReadText);
                        }
                        else if (wayFormat[1] == "txt")
                        {
                            string[] words = ReadText.Split("\n");

                            Model Something = new Model(words[0], Convert.ToInt32(words[1]), words[2]);
                            List<Model> InformationAbout = new List<Model>();
                            InformationAbout.Add(Something);

                            XmlSerializer xml = new XmlSerializer(typeof(Model));
                            using (FileStream fs = new FileStream(way, FileMode.OpenOrCreate))
                            {
                                xml.Serialize(fs, InformationAbout);
                            }
                        }
                        else if (wayFormat[1] == "json")
                        {

                        }
                    }
                    string[] textFormat = way.Split(".");
                    Console.WriteLine("Всё чётко!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            } while (true);

        }
    }
}
