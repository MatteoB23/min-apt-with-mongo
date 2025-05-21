using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class MongoMovieService : IMovieService
{
    private readonly IMongoCollection<Movie> _movieCollection;
    private readonly IMongoClient _mongoClient;

    // Constructor.
    public MongoMovieService(IOptions<DatabaseSettings> options)
    {
        _mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = _mongoClient.GetDatabase(options.Value.DatabaseName);
        _movieCollection = database.GetCollection<Movie>(options.Value.CollectionName);

    }
    public string Check()
    {
        try
        {
            var databaseNames = _mongoClient.ListDatabaseNames().ToList();

            return "Zugriff auf MongoDB ok. Vorhandene DBs: " + string.Join(",", databaseNames);
        }
        catch (System.Exception e)
        {
            return "Zugriff auf MongoDB funktioniert nicht: " + e.Message;
        }
    }

    public void Create(Movie movie)
    {
        _movieCollection.InsertOne(movie);
    }

    public IEnumerable<Movie> Get()
    {
        return _movieCollection.Find(movie => true).ToList();
    }

    public Movie Get(string id)
    {
        return _movieCollection.Find(movie => movie.Id == id).FirstOrDefault();
    }

    public void Update(string id, Movie updatedMovie)
    {
        _movieCollection.ReplaceOne(movie => movie.Id == id, updatedMovie);
    }

    public void Remove(string id)
    {
        _movieCollection.DeleteOne(movie => movie.Id == id);
    }
}
