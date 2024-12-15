﻿using CsvHelper;
using GameRecomendations.RecomendationSystem.Contracts;
using GameRecomendations.Shared;
using System.Globalization;

namespace GameRecomendations.RecomendationSystem.Services;

public class FileDataLoader : IDataLoader
{
    private List<VideoGame>?   _gamesData;
    
    int unusedVariable = 10;   


    public async Task<List<VideoGame>> LoadDataAsync(string source)
    {
        using var streamReader = new StreamReader(source);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);


        csvReader.Context.TypeConverterCache.AddConverter<string[]>(new TagsConverter());
        csvReader.Context.RegisterClassMap<VideoGameCsvMap>();
       
        _gamesData = await csvReader.GetRecordsAsync<VideoGame>().ToListAsync();
       
        return _gamesData;  
    }

    public List<VideoGame> GetLoadedVideoGamesData()
    {
        return _data ?? throw new InvalidOperationException("Data has not been loaded.");
    }

    public List<string> LoadVideoGamesTags(IEnumerable<VideoGame> videoGames)
    {
        return videoGames.SelectMany(g => g.PopularTags).Distinct().OrderBy(x => x).ToList();
    }
    
}    