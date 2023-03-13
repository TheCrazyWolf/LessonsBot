using LessonsBot_DB.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LessonsBot_Vk.ExpDataset
{
    internal class TrainBot
    {
        public void Start()
        {
            DbProvider _ef = new();

            var dataset = File.ReadAllText("dataset.txt");

            var exp_word = dataset.Split('\\');

            foreach (var item in exp_word)
            {
                var word_ans = item.Replace("\r", string.Empty).Replace("\n", string.Empty).Split(';');

                if (word_ans.Length <= 1)
                    continue;

                Dicktionary dick = new Dicktionary()
                {
                    Word = word_ans[0],
                    Answer = word_ans[1]
                };

                SLogger.Write($"{word_ans[0]} {word_ans[1]}");
                _ef.Add(dick);
                _ef.SaveChanges();
            }

        }

        public string ExpGetAnswer(string input)
        {
            DbProvider _ef = new();

            var data = _ef.Dicktionaries.Where(t => t.Word.ToLower().StartsWith(input.ToLower())).ToList();
            var sDOP = _ef.Dicktionaries.Where(t => t.Word.ToLower().Contains(input.ToLower())).ToList();
            data.AddRange(sDOP);
            var noDupes = data.Distinct().ToList();

            return noDupes[new Random().Next(0, noDupes.Count())].Answer;
        }
    }
}
