namespace GameRecomendations.Shared;

public class VideoGame
{
    public int AppId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string ReleaseDate { get; set; }
    public string Developer { get; set; }
    public string PopularTags { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? RecommendationScore { get; set; }
}
