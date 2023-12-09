using Word = Microsoft.Office.Interop.Word;

namespace Test_generator_console.Classes.Word_class
{
    internal class WordClass
    {
        public static void WordTable(string[] lines, string savePath, string topic, int questcount)
        {
            int count = lines.Length;
            int questions = questcount;

            Word.Application wordApplication = new Word.Application();  //объявили переменную типа Word
            Object template = Type.Missing;
            Object newTemplate = Type.Missing;
            Object documentType = Type.Missing;
            Object visible = Type.Missing;
            wordApplication.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);    //добавили в проложение документ
            Word.Document doc = wordApplication.ActiveDocument;
            wordApplication.Visible = false; //делаем что бы word не работал в фоновом режиме
            Object r = Type.Missing;
            Word.Paragraph par = doc.Content.Paragraphs.Add(ref r);    //добавляем в документ параграф
            Object missing = Type.Missing;
            Word.Range rng = doc.Range(ref missing, ref missing);    //получаем текстовую область параграфа
            rng.Tables.Add(doc.Paragraphs[doc.Paragraphs.Count].Range, questions, 7, ref missing, ref missing);     //вставляем в текстовую область таблицу
            Word.Table tbl = doc.Tables[doc.Tables.Count];    //для удобства таблицу пихаем в переменную
            tbl.Cell(1, 1).Range.Text = "Вопрос";
            tbl.Cell(1, 2).Range.Text = "А";
            tbl.Cell(1, 3).Range.Text = "B";
            tbl.Cell(1, 4).Range.Text = "C";
            tbl.Cell(1, 5).Range.Text = "D";
            tbl.Cell(1, 6).Range.Text = "E";
            tbl.Cell(1, 7).Range.Text = "Верный ответ";

            for (int i = 1; i < 8; i++)
            {
                tbl.Cell(1, i).Shading.BackgroundPatternColor = Word.WdColor.wdColorLightYellow;
            }

            Word.Border[] borders =
            [
                tbl.Borders[Word.WdBorderType.wdBorderLeft],
                tbl.Borders[Word.WdBorderType.wdBorderRight],
                tbl.Borders[Word.WdBorderType.wdBorderTop],
                tbl.Borders[Word.WdBorderType.wdBorderBottom],
                tbl.Borders[Word.WdBorderType.wdBorderHorizontal],
                tbl.Borders[Word.WdBorderType.wdBorderVertical],
            ];
            foreach (Word.Border border in borders)
            {
                border.LineStyle = Word.WdLineStyle.wdLineStyleSingle;//ставим стиль границы 
                border.Color = Word.WdColor.wdColorBlack;//задаем цвет границы
            }

            int quest = 0;
            //Заполняет столбцы с вопросами и ответами
            for (int i = 2; i < questions+1; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    if (lines[quest] == "")
                    {
                        quest++;
                    }
                    tbl.Cell(i, j).Range.Text = lines[quest];
                    quest++;
                }
            }

            wordApplication.ActiveDocument.SaveAs(savePath + "Тест по теме " + topic + ".docx");

            Console.WriteLine("\nГотово! Файл сохранён.");
        }
    }
}
