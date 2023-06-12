using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace GameRecomendations.Shared;

public class TagsConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return text?.Split(',');
    }
}
