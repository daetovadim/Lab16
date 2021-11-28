using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;

namespace Lab16
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\tРабота с товарами\n");

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            
            int arraysLength = 5;
            Product[] products = new Product[arraysLength];
            string[] jsonStrings = new string[arraysLength];
            try
            {
                for (int i = 0; i < arraysLength; i++)
                {
                    products[i] = new Product();

                    Console.Write($" Введите наименование товара {i + 1}: ");
                    products[i].ProductName = Console.ReadLine();
                    Console.Write($" Введите код товара {i + 1}: ");
                    products[i].ProductCode = Convert.ToInt32(Console.ReadLine());
                    Console.Write($" Введите стоимость товара {i + 1}: ");
                    products[i].ProductPrice = Convert.ToInt32(Console.ReadLine());

                    jsonStrings[i] = JsonSerializer.Serialize(products[i], options);

                    Console.WriteLine();
                }

                string path = "Example";
                DirectoryInfo directory = new DirectoryInfo(path);
                if (!directory.Exists)
                    directory.Create();

                string filePath = "Example/Products.json";
                if (!File.Exists(filePath))
                    File.Create(filePath);

                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    for (int i = 0; i < products.Length; i++)
                    {
                        streamWriter.Write(jsonStrings[i]);
                        streamWriter.WriteLine();
                    }
                }

                Product[] products1 = new Product[arraysLength];
                string[] jsonStrings1 = new string[arraysLength];

                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    for (int i = 0; i < arraysLength; i++)
                    {
                        jsonStrings1[i] = streamReader.ReadLine();
                        products1[i] = new Product();
                        products1[i] = JsonSerializer.Deserialize<Product>(jsonStrings1[i]);
                    }
                }

                int maxPrice = products1[0].ProductPrice;
                int number = 0;

                for (int i = 0; i < arraysLength; i++)
                {
                    if (products1[i].ProductPrice > maxPrice)
                    {
                        maxPrice = products1[i].ProductPrice;
                        number = i;
                    }
                }
                Console.WriteLine($" Наибольшая стоимость ({maxPrice}) у товара \"{products1[number].ProductName}\"");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n\tОшибка! Введённое значение не является целым числом. Попробуйте снова.");                
            }

            Console.ReadKey();
        }
    }

    class Product
    {
        int productCode;
        int productPrice;

        [JsonPropertyName("productCode")]
        public int ProductCode
        {
            set
            {
                if (value > 0)
                    productCode = value;
                else
                    Console.WriteLine("\n\tЗначение должно быть больше 0!");
            }
            get
            {
                return productCode;
            }
        }
        [JsonPropertyName("productPrice")]
        public int ProductPrice
        {
            set
            {
                if (value > 0)
                    productPrice = value;
                else
                    Console.WriteLine("\n\tЗначение должно быть больше 0!");
            }
            get
            {
                return productPrice;
            }
        }
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
    }
}
