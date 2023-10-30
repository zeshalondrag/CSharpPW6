using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к файлу (вместе с названием), который вы хотите открыть");
        Console.WriteLine("--------------------------------------------------------------------");
        string filePath = Console.ReadLine();

        TextEditor textEditor = new TextEditor(filePath);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Сохранить файл в одном из трёх форматов (txt, json, xml) - F1. Закрыть программу - Escape");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            textEditor.DisplayText();
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.F1)
            {
                string fileType = textEditor.ChooseFileType();
                if (fileType != null)
                {
                    textEditor.Save(fileType);
                    Console.WriteLine("Файл успешно сохранен на вашем рабочем столе.");
                    Console.ReadKey();
                }
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }
}

[Serializable]
public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    public Figure()
    {
    }

    public Figure(string name, double width, double height)
    {
        Name = name;
        Width = width;
        Height = height;
    }
}

public class TextEditor
{
    private string filePath;
    private List<Figure> figures;

    public TextEditor(string filePath)
    {
        this.filePath = filePath;
        this.figures = new List<Figure>();

        if (File.Exists(filePath))
        {
            Load();
        }
        else
        {
            Console.WriteLine("Файл не существует. Создание новой модели.");
        }
    }

    

    private void LoadFromTxtFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 3)
        {
            string name = lines[i];
            double width = Convert.ToDouble(lines[i + 1]);
            double height = Convert.ToDouble(lines[i + 2]);
            figures.Add(new Figure(name, width, height));
        }
    }

    public string ChooseFileType()
    {
        Console.WriteLine("Выберите тип файла для сохранения (1 - txt, 2 - json, 3 - xml):");
        Console.WriteLine("---------------------------------------------------------------");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                return "txt";
            case "2":
                return "json";
            case "3":
                return "xml";
            default:
                Console.WriteLine("Неверный выбор. Файл не будет сохранен.");
                return null;
        }
    }

    public void Save(string fileType)
    {
        Console.WriteLine("Введите название файла для сохранения (без расширения):");
        string fileName = Console.ReadLine();

        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("Название файла не может быть пустым. Файл не будет сохранен.");
            return;
        }

        string fileExtension = "." + fileType;
        string savePath = Path.Combine(Path.GetDirectoryName(filePath), fileName + fileExtension);

        if (fileType == "txt")
        {
            SaveToTxtFile(savePath);
        }
        else if (fileType == "json")
        {
            string json = JsonConvert.SerializeObject(figures);
            File.WriteAllText(savePath, json);
        }
        else if (fileType == "xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));
            using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, figures);
            }
        }
    }

    private void SaveToTxtFile(string filePath)
    {
        List<string> lines = new List<string>();
        foreach (var figure in figures)
        {
            lines.Add(figure.Name);
            lines.Add(figure.Width.ToString());
            lines.Add(figure.Height.ToString());
        }
        File.WriteAllLines(filePath, lines);
    }

    public void Load()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();
        if (fileExtension == ".txt")
        {
            LoadFromTxtFile(filePath);
        }
        else if (fileExtension == ".json")
        {
            string json = File.ReadAllText(filePath);
            figures = JsonConvert.DeserializeObject<List<Figure>>(json);
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                figures = (List<Figure>)serializer.Deserialize(fileStream);
            }
        }
    }

    public void DisplayText()
    {
        if (figures != null)
        {
            foreach (var figure in figures)
            {
                Console.WriteLine($"Имя: {figure.Name}");
                Console.WriteLine($"Ширина: {figure.Width}");
                Console.WriteLine($"Высота: {figure.Height}");
                Console.WriteLine("----------");
            }
        }
    }
}