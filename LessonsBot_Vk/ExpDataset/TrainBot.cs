using LessonsBot_DB.ModelsDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using VkNet.Model;

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

        }

        public string ExpGetAnswer(string input)
        {

            input = input.Replace(",", string.Empty).Replace(".", string.Empty).Replace("!", string.Empty).Replace("?", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
            DbProvider _ef = new();

            string[] filters = input.Split(new[] { ' ' });
            //var objects = from x in _ef.Dicktionaries

            var s = _ef.Dicktionaries.FirstOrDefault(x => x.Word == "Ты владеешь английским?");

            var word = _ef.Dicktionaries.Where(u => EF.Functions.Like(u.Word.ToLower(), $"%{input.ToLower()}%")).ToList();


            //var word
            //    = _ef.Dicktionaries.Where(o => o.Word.ToLower().Contains(input.Trim().ToLower())).ToList();

            //var word = _ef.Dicktionaries
            //    .Where(x => EF.Functions.Like(x.Word, $"%{input}%")).ToList();


            //var data = _ef.Dicktionaries.Where(t => t.Word.ToLower().StartsWith(input.ToLower())).ToList();
            //var sDOP = _ef.Dicktionaries.Where(t => t.Word.ToLower().Contains(input.ToLower())).ToList();
            //data.AddRange(sDOP);
            //var noDupes = data.Distinct().ToList();





            //foreach (var s in _ef.Dicktionaries.ToList())
            //{
            //    // Calculate the Levenshtein-distance:
            //    int levenshteinDistance =
            //            LevenshteinDistance(s.Word, input);

            //    // Length of the longer string:
            //    int length = Math.Max(s.Word.Length, input.Length);

            //    // Calculate the score:
            //    double score = 1 0 - (double)levenshteinDistance / length;

            //    // Match?
            //    if (score > fuzzyness)
            //        foundWords.Add(s);
            //}


            if (word.Count == 0)
                return "Я тупой";

            //if (word.Count >= 25)
            //    return word[1].Answer;

            //if (word.Count <= 24)
                return word[new Random().Next(0, word.Count)].Answer;


            //return "";
        }

        public static int LevenshteinDistance(string src, string dest)
        {
            int[,] d = new int[src.Length + 1, dest.Length + 1];
            int i, j, cost;
            char[] str1 = src.ToCharArray();
            char[] str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
            {
                d[i, 0] = i;
            }
            for (j = 0; j <= str2.Length; j++)
            {
                d[0, j] = j;
            }
            for (i = 1; i <= str1.Length; i++)
            {
                for (j = 1; j <= str2.Length; j++)
                {

                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    d[i, j] =
                    Math.Min(
                    d[i - 1, j] + 1, // Deletion
                    Math.Min(
                    d[i, j - 1] + 1, // Insertion
                    d[i - 1, j - 1] + cost)); // Substitution

                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
         str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[str1.Length, str2.Length];
        }


    }
}
