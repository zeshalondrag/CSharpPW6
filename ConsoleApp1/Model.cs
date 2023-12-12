namespace PW6
{
    public class Model
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }

        public Model(string name, int age, string description)
        {
            Name = name;
            Age = age;
            Description = description;
        }
    }
}
