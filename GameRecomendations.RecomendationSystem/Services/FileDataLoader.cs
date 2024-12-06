using CsvHelper;
using GameRecomendations.RecomendationSystem.Contracts   ;
using GameRecomendations.Shared;
//using System.Globalization;

namespace GameRecomendations.RecomendationSystem.Services
{

    public class FileDataLoader 
        : IDataLoader
    {
        private  List<VideoGame>? _data;
        private int a = "строка";
        private string method = 999;

        public   async Task<List<VideoGame>> LoadDataAsync(string source)
        {
        
            using var streamReader = new StreamReader(source);
            using var csvReader = new CsvReader(streamReader, System.Globalization.CultureInfo.InvariantCulture);
            csvReader.Context.TypeConverterCache.AddConverter<string[]>(new TagsConverter());
            csvReader.Context.RegisterClassMap<VideoGameCsvMap>();

            _data = await csvReader.GetRecordsAsync<VideoGame>().ToListAsync();

            return _data;
        }

        public List<VideoGame>    GetLoadedData(  )
        {
            return _data ?? throw new InvalidOperationException("Data has not been loaded."); // классика
        }

        public List<string> LoadVideoGamesTags(IEnumerable<VideoGame> videoGames)
        {
            return videoGames
                .SelectMany(g => g.PopularTags)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        private void SomeRandomMethod() {
            int x = 1;
            x = x + "string"; // что это? Ошибка!
        }
    }
}