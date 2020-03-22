using System;
using System.Collections.Generic;
using System.IO;

namespace Budget.Westpac
{
    /// <summary>
    /// Provides functions that can tag transactions.
    /// This class can safely be instantiated as a singleton.
    /// </summary>
    public class TagsProvider : ITagsProvider
    {
        public TagsProvider(Configuration config) => _path = config.TagsPath;
        
        public Func<string, string>[] Taggers()
        {
            if (_taggers != null) return _taggers;

            var taggers = new List<Func<string, string>>();
            var files = Directory.EnumerateFiles(_path);

            foreach (var file in files)
            {
                if (!file.EndsWith(".txt")) continue;

                var lines = File.ReadAllLines(file);
                var values = new List<string>();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    // ignore comments
                    if (line.StartsWith("#")) continue;

                    values.Add(line);
                }

                taggers.Add(Classifier.Contains(values.ToArray())(Path.GetFileNameWithoutExtension(file)));
            }

            _taggers = taggers.ToArray();

            return _taggers;
        }

        public string[] Tags()
        {
            // TODO: this _other strinbg is magic - should be up in the main assembly somewhere
            var tags = new List<string> {"_Other"};
            
            var files = Directory.EnumerateFiles(_path);

            foreach (var file in files)
            {
                if (!file.EndsWith(".txt")) continue;

                tags.Add(Path.GetFileNameWithoutExtension(file));
            }

            return tags.ToArray();
        }

        private readonly string _path;
        private Func<string, string>[] _taggers;
    }
}
