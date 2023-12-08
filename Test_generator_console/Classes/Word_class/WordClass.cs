using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace Test_generator_console.Classes.Word_class
{
    internal class WordClass
    {
        public static void WordTable(string txtpath, string savePath)
        {
            string[] lines = File.ReadAllLines(txtpath); //Массив из всех строк файла

            int count = File.ReadAllLines(txtpath).Length;
            int questions = 15;

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
            tbl.Cell(1, 7).Range.Text = "Правильный ответ";

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
            for (int i = 2; i < questions; i++)
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

            wordApplication.ActiveDocument.SaveAs(savePath);

            File.Delete(txtpath);
            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
