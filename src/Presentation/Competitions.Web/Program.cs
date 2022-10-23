using Competitions.Persistence.Data.Initializer.Interfaces;
using Competitions.Web;

var builder = WebApplication.CreateBuilder(args);

var startUp = new Startup(builder.Configuration);
startUp.ConfigureServices(builder.Services , builder.Environment);

var app = builder.Build();


var scoped = app.Services.CreateScope();
var dbInitializer = scoped.ServiceProvider.GetService<IDbInitializer>();

startUp.Configure(app , builder.Environment , dbInitializer);
app.Run();
