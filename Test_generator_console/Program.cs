using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Test_generator_console.Classes.GPT_classes;
using System.IO;
using Test_generator_console.Classes.Word_class;

namespace Test_generator_console
{
    internal class Program
    {
        static string apiKey = "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";
        // адрес api для взаимодействия с чат-ботом
        static string endpoint = "https://api.openai.com/v1/chat/completions";
        //путь для сохранения .docx-файла
        static string txtPath = @"C:\Users\minin\OneDrive\Рабочий стол\temp_test.txt";
        //путь для сохранения временного txt-файла с тестами
        static string savePath = @"C:\Users\minin\OneDrive\Рабочий стол\готовый_тест.docx";

        static async Task Main(string[] args)
        {
            // набор соообщений диалога с чат-ботом
            List<MessageClass> messages = new List<MessageClass>();
            // HttpClient для отправки сообщений
            var httpClient = new HttpClient();
            // устанавливаем отправляемый в запросе токен
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            while (true)
            {
                // ввод сообщения пользователя
                Console.Write("Введите тему теста: ");
                var topic = Console.ReadLine();

                var content = $"сгенерируй тест из 15 вопросов по теме \"{topic}\" с пятью вариантами ответов и отметь правильные ответы без лишних слов " +
                    $"после каждого вопроса";
                    

                // если введенное сообщение имеет длину меньше 1 символа
                // то выходим из цикла и завершаем программу
                if (content is not { Length: > 0 }) break;
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
                var responseText = responseMessage.Content.Trim();

                //сохраняем сообщение gpt в txt
                File.WriteAllText(txtPath, responseText);

                Console.WriteLine($"ChatGPT: {responseText}");

                string[] lines = File.ReadAllLines(txtPath);

                WordClass.WordTable(txtPath, savePath);

            }
        }
    }
}
