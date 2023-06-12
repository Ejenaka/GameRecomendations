using CsvHelper.Configuration;

namespace GameRecomendations.Shared;

public class VideoGameCsvMap : ClassMap<VideoGame>
{
    public VideoGameCsvMap()
    {
        Map(g => g.AppId).Name("app_id");

        Map(g => g.Name).Name("name");

        Map(g => g.Url).Name("url");

        Map(g => g.ReleaseDate).Name("release_date");

        Map(g => g.Developer).Name("developer");

        Map(g => g.PopularTags).Name("popular_tags").TypeConverter<TagsConverter>();

        Map(g => g.Genre).Name("genre");

        Map(g => g.Description).Name("desc_snippet");

        Map(g => g.Price).Name("original_price");
    }
}
