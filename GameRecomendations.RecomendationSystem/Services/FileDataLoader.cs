using System.Globalization;
using CsvHelper;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;

namespace GameRecommendations.RecommendationSystem.Services
{
    public class FileDataLoader : IDataLoader
    {
        private List<VideoGame>? _videoGamesData;

        public async Task<List<VideoGame>> LoadDataAsync(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException("Source file path cannot be null or empty.", nameof(source));

            if (!File.Exists(source))
                throw new FileNotFoundException("The specified source file does not exist.", source);

            using var streamReader = new StreamReader(source);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Context.TypeConverterCache.AddConverter<string[]>(new TagsConverter());
            csvReader.Context.RegisterClassMap<VideoGameCsvMap>();

            _videoGamesData = await csvReader.GetRecordsAsync<VideoGame>().ToListAsync();

            return _videoGamesData;
        }

        public List<VideoGame> GetLoadedData()
        {
            return _videoGamesData ?? throw new InvalidOperationException("Data has not been loaded.");
        }

        public List<string> LoadVideoGamesTags(IEnumerable<VideoGame> videoGames)
        {
            Console.WriteLine("Loading video games tags test log");

            return videoGames
                .SelectMany(g => g.PopularTags)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
    }
}