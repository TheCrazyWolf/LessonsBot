using LessonBot_SBot.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonBot_SBot
{
    public class SimpleBot
    {
        protected DictionaryContext _dc;

        public SimpleBot()
        {
            _dc = new DictionaryContext();
        }

        public async Task<Task> Train(string data_path)
        {
            if (!File.Exists(data_path))
                return Task.CompletedTask;

            string[] open_data = File.ReadAllText(data_path)
                .Replace("\\1", string.Empty)
                .Replace("\\2", string.Empty)
                .Replace("\\3", string.Empty)
                .Split('\n');

            foreach (var item in open_data)
            {
                string[] new_phrase = item.Split('\\');

                if (new_phrase.Length < 1)
                    continue;

                Task.Run(() => Train(new_phrase[0], new_phrase[1]));
            }
            
            //IEnumerable<Dictionary> = 


            return Task.CompletedTask;
        }

        public async Task<Task> Train(string word_question, string word_expected_answer)
        {
            word_question = await PrepareStringWord(word_question);

            var find = await _dc.Dictionaries
                .FirstOrDefaultAsync(x => x.Word == word_question && x.Answer == word_expected_answer);

            if (find == null)
                return Task.CompletedTask;

            Dictionary dictionary = new Dictionary() { Word = word_question, Answer = word_expected_answer };
            _dc.Add(dictionary);
            await _dc.SaveChangesAsync();

            return Task.CompletedTask;
        }

        public async Task<string> GetResult(string message)
        {
            message = await PrepareStringWord(message);

            var find_data =
                _dc.Dictionaries
                .Where(u => EF.Functions.Like(u.Word.ToLower(), $"%{message}%"))
                .ToList();

            if (find_data.Count() == 0)
                await Task.FromResult<string>("");

            string answer = find_data[new Random().Next(0, find_data.Count)].Answer;
            return await Task.FromResult<string>(answer);
        }

        protected async Task<string> PrepareStringWord(string inpute)
        {
            string new_word = inpute
                .ToLower()
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("!", string.Empty)
                .Replace("?", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);
            return await Task<string>.FromResult(new_word);
        }

    }
}
