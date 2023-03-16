using LessonsBot_DB.ModelsDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonsBot_SBot
{
    public class SBot
    {
        private DbProvider _ef;
        public SBot()
        {
            _ef = new DbProvider();
        }
        public Task TrainFromDataset(string path)
        {
            if(!File.Exists(path))
            {
                SLogger.WriteDanger("[SBOT] Файл не существует");
            }

            SLogger.WriteWarning("[SBOT] Открываем файл");

            var dataset = File.ReadAllText("dataset.txt");

            SLogger.WriteWarning("[SBOT] Обработка дата сета");

            var exp_word = dataset.Split('\\');

            foreach (var item in exp_word)
            {
                var word_ans = item.Replace(",", string.Empty).Replace(".", string.Empty).Replace("!", string.Empty).Replace("?", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Split(';');

                if (word_ans.Length <= 1)
                    continue;

                Dicktionary dick = new Dicktionary()
                {
                    Word = word_ans[0].ToLower(),
                    Answer = word_ans[1]
                };

                SLogger.Write($"{word_ans[0]} {word_ans[1]}");
                _ef.Add(dick);
                _ef.SaveChanges();
            }

            return Task.CompletedTask;
        } 

        public Task TrainExpect(string data, string expected)
        {
            //var find = 
            return Task.CompletedTask;
        }

        public Task<string> GetResponce(string input)
        {
            input = input.Replace(",", string.Empty).Replace(".", string.Empty).Replace("!", string.Empty).Replace("?", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);

            var word = _ef.Dicktionaries.Where(u => EF.Functions.Like(u.Word.ToLower(), $"%{input.ToLower()}%")).ToList();

            return Task.FromResult<string>(word[new Random().Next(0, word.Count)].Answer);
        }
    }
}
