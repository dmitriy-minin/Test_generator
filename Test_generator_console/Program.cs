using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Test_generator_console.Classes.GPT_classes;
using System.IO;
using Test_generator_console.Classes.Word_class;
using System.Collections;
using System.Globalization;

namespace Test_generator_console
{
    internal class Program
    {
        static string apiKey = "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";
        static string endpoint = "https://api.openai.com/v1/chat/completions";

        //путь для сохранения временного txt-файла с тестами
        static string txtPath = @"C:\Users\minin\OneDrive\Рабочий стол\temp_test.txt";

        //путь для сохранения .docx-файла
        static string savePath = @"C:\Users\minin\OneDrive\Рабочий стол\";


        static async Task Main(string[] args)
        {
            // набор соообщений диалога с чат-ботом
            List<MessageClass> messages = new List<MessageClass>();
            // HttpClient для отправки сообщений
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            // устанавливаем отправляемый в запросе токен
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            while (true)
            {
                repeat:
                Console.Clear();
                // ввод параметров пользователем
                Console.Write("Введите тему теста: ");
                var topic = Console.ReadLine();
                Console.Write("Введите количество вопросов: ");
                var count = Convert.ToInt32(Console.ReadLine());

                var content = $"сгенерируй тест из {count} вопросов по теме \"{topic}\" с пятью вариантами ответов и отметь правильные ответы без лишних слов " +
                    $"после каждого вопроса";

                // если введенное сообщение имеет длину меньше 1 символа
                // то откатываем программу в начало
                if (topic is not { Length: > 0 })
                {
                    goto repeat;
                }

                Console.WriteLine("\nЖдите ответа...\n");
                // формируем отправляемое сообщение
                var message = new MessageClass() { Role = "user", Content = content };
                // добавляем сообщение в список сообщений
                messages.Add(message);

                // формируем отправляемые данные
                var requestData = new RequestClass()
                {
                    ModelId = "gpt-3.5-turbo",
                    Messages = messages
                };
                // отправляем запрос
                using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);

                // если произошла ошибка, выводим сообщение об ошибке на консоль
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
                    break;
                }
                // получаем данные ответа
                ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

                var choices = responseData?.Choices ?? new List<ChoiceClass>();
                if (choices.Count == 0)
                {
                    Console.WriteLine("No choices were returned by the API");
                    continue;
                }

                var choice = choices[0];
                var responseMessage = choice.Message;
                // добавляем полученное сообщение в список сообщений
                messages.Add(responseMessage);

                //ArrayList sms = new ArrayList();

                var responseText = responseMessage.Content.Trim();

                Console.WriteLine($"ChatGPT: {responseText} \n");

                File.WriteAllText(txtPath, responseText); 
                Console.ReadKey();

                string[] lines = File.ReadAllLines(txtPath);
                File.Delete(txtPath);

                WordClass.WordTable(lines, savePath, topic, count+1);
                Console.ReadKey();
            }
        }
    }
}
