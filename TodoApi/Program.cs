//https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

//https://www.educative.io/edpresso/how-to-generate-a-random-string-in-c-sharp
using System.Text;
using System;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskDB>(opt => opt.UseInMemoryDatabase("Calculator"));
builder.Services.AddDbContext<AuthDB>(opt => opt.UseInMemoryDatabase("LoginInfo"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
//https://stackoverflow.com/questions/70554894/asp-net-core-6-0-minimal-apis-what-is-the-proper-way-to-bind-json-body-reques
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()
        ?.Error;
    if (exception is not null)
    {
        var response = new { error = exception.Message };
        context.Response.StatusCode = 400;

        await context.Response.WriteAsJsonAsync(response);
    }
}));
app.MapGet("/", () => "SSPŠ!");

app.MapGet("/tasklist/{token}", async (string token, TaskDB db, AuthDB db2) =>
    {
        var accounts = await db2.Users.ToListAsync();
        if (accounts is null) return Results.NotFound();
        foreach (Auth user in accounts)
        {
            if (user is null) return Results.NotFound();
            if (user.Token == token)
            {
                return Results.Ok(await db.Jobs.ToListAsync());
            }
        }
        return Results.BadRequest("UNAUTHENTICATED");
        
    });

app.MapGet("/tasklist", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
}
);

app.MapPost("/auth", async (Auth inputLogin, AuthDB db) =>
{
    

    if (inputLogin.Email.Contains("@ssps.cz"))
    {
        //https://www.educative.io/edpresso/how-to-generate-a-random-string-in-c-sharp
        int length = 8;

        // creating a StringBuilder object()
        StringBuilder str_build = new StringBuilder();
        Random random = new Random();

        char letter;

        for (int i = 0; i < length; i++)
        {
            double flt = random.NextDouble();
            int shift = Convert.ToInt32(Math.Floor(25 * flt));
            letter = Convert.ToChar(shift + 65);
            str_build.Append(letter);
        }

        inputLogin.Token = str_build.ToString();
    }
    else
    {
        return Results.BadRequest("INVALID E-MAIL");
    }

    db.Users.Add(inputLogin);
    await db.SaveChangesAsync();

    return Results.Created($"/auth/{inputLogin.Id}", inputLogin);
});

app.MapGet("/whoami/{token}", async (string token, AuthDB db) =>
{
    var accounts = await db.Users.ToListAsync();
    if (accounts is null) return Results.NotFound();
    foreach (Auth user in accounts)
    {
        if(user is null) return Results.NotFound();
        if (user.Token == token)
        {
            return Results.Ok(user.Email);
        }
    }
    return Results.BadRequest("UNAUTHENTICATED");
});
app.MapGet("/whoami", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
}
);
app.MapGet("/logout/{token}", async (string token, AuthDB db) =>
{
    var accounts = await db.Users.ToListAsync();
    if (accounts is null) return Results.NotFound();
    foreach (Auth user in accounts)
    {
        if (user is null) return Results.NotFound();
        if (user.Token == token)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.Ok("LOGGED OUT");
        }
    }
    return Results.BadRequest("UNAUTHENTICATED");
});
app.MapGet("/logout", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
}
);
//post new
app.MapPost("/add/{token}", async (string token, Tasks inputTask, TaskDB db, AuthDB db2) =>
{
    var accounts = await db2.Users.ToListAsync();
    if (accounts is null) return Results.NotFound();
    foreach (Auth user in accounts)
    {
        if (user is null) return Results.NotFound();
        if (user.Token == token)
        {
            /*if (inputTask.NumberOne.GetType() != typeof(int))
            {
                return Results.BadRequest(inputTask.NumberOne);
            }*/
            if (inputTask.TaskType != "CIRCLE-PERIMETER" && inputTask.TaskType != "CIRCLE-AREA" && inputTask.TaskType != "RECTANGLE-AREA" && inputTask.TaskType != "RECTANGLE-PERIMETER")
            {
                return Results.BadRequest("SYNTAX ERROR");
            }
            if (inputTask.TaskType == "RECTANGLE-AREA" || inputTask.TaskType == "RECTANLGE-PERIMETER")
            {
                if (inputTask.NumberOne <= 0 || inputTask.NumberTwo <= 0)
                {
                    return Results.BadRequest("SYNTAX ERROR");
                }
            }
            if (inputTask.TaskType == "CIRCLE-AREA" || inputTask.TaskType == "CIRCLE-PERIMETER")
            {
                if (inputTask.NumberOne <= 0)
                {
                    return Results.BadRequest("SYNTAX ERROR");
                }
            }

            db.Jobs.Add(inputTask);
            await db.SaveChangesAsync();

            return Results.Created($"/tasklist/{inputTask.Id}", inputTask);
        }
    }
    return Results.BadRequest("UNAUTHENTICATED");
});

app.MapPost("/add", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
    }
);



app.MapGet("/process/{token}", async (string token, TaskDB db, AuthDB db2) =>
{
    var accounts = await db2.Users.ToListAsync();
    if (accounts is null) return Results.NotFound();
    foreach (Auth user in accounts)
    {
        if (user is null) return Results.NotFound();
        if (user.Token == token)
        {
            //search database for id
            var jobs = await db.Jobs.ToListAsync();
            if (jobs is null) return Results.NotFound();
            var jobs_completed = 0;
            foreach (Tasks job in jobs)
            {
                if (job is null) return Results.NotFound();
                if (job.IsComplete == false)
                {
                    jobs_completed++;
                    if (job.TaskType == "RECTANGLE-AREA")
                    {
                        var area = job.NumberOne * job.NumberTwo;
                        job.Result = area;
                        job.IsComplete = true;
                    }
                    else if (job.TaskType == "RECTANGLE-PERIMETER")
                    {
                        var perimeter = 2 * (job.NumberOne + job.NumberTwo);
                        job.Result = perimeter;
                        job.IsComplete = true;
                    }
                    else if (job.TaskType == "CIRCLE-AREA")
                    {
                        var area = 3.14 * job.NumberOne;
                        job.Result = area;
                        job.IsComplete = true;
                    }
                    else if (job.TaskType == "CIRCLE-PERIMETER")
                    {
                        var perimeter = 2 * 3.14 * job.NumberOne;
                        job.Result = perimeter;
                        job.IsComplete = true;
                    }
                }
            }
            if (jobs_completed == 0)
            {
                return Results.BadRequest("NO TASKS IN QUEUE");
            }

            //commit to db
            await db.SaveChangesAsync();
            var results = await db.Jobs.ToListAsync();
            return Results.Ok(results);
        }
    }
    return Results.BadRequest("UNAUTHENTICATED");
});
app.MapGet("/process", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
}
);
app.MapGet("/status/{token}", async (string token, TaskDB db, AuthDB db2) =>
{
    var accounts = await db2.Users.ToListAsync();
    if (accounts is null) return Results.NotFound();
    foreach (Auth user in accounts)
    {
        if (user is null) return Results.NotFound();
        if (user.Token == token)
        {
            var jobs = await db.Jobs.ToListAsync();
            var jobs_completed = 0;
            var jobs_notCompleted = 0;
            if (jobs is null) return Results.NotFound();
            foreach (Tasks job in jobs)
            {
                if (job is null) return Results.NotFound();
                if (job.IsComplete == false)
                {
                    jobs_notCompleted++;
                }
                else
                {
                    jobs_completed++;
                }
            }

            //commit to db
            await db.SaveChangesAsync();
            var results = await db.Jobs.ToListAsync();
            return Results.Ok($"{jobs_completed} tasks were processed / {jobs_notCompleted} tasks to be processed");
        }
    }
    return Results.BadRequest("UNAUTHENTICATED");
});
app.MapGet("/status", () =>
{
    return Results.BadRequest("UNAUTHENTICATED");
}
);

//extra functionality outside of DVOP task but without authentication
//put = change
//input task = json
/*app.MapPut("/tasklist/{id}", async (int id, Tasks inputTask, TaskDB db) =>
{
    //search database for id
    var job = await db.Jobs.FindAsync(id);

    if (job is null) return Results.NotFound();
    //update db entry
    job.TaskType = inputTask.TaskType;
    job.NumberOne = inputTask.NumberOne;
    job.NumberTwo = inputTask.NumberTwo;
    job.IsComplete = inputTask.IsComplete;
    //commit to db
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tasklist/{id}", async (int id, TaskDB db) =>
{
    if (await db.Jobs.FindAsync(id) is Tasks job)
    {
        db.Jobs.Remove(job);
        await db.SaveChangesAsync();
        return Results.Ok(job);
    }

    return Results.NotFound();
});

app.MapGet("/process/{id}", async (int id, TaskDB db) =>
{
    //search database for id
    var job = await db.Jobs.FindAsync(id);

    if (job is null) return Results.NotFound();
    //RECTANGLE-AREA
    //RECTANGLE-PERIMETER
    //CIRCLE-AREA
    //CIRCLE-PERIMETER
    if (job.IsComplete == false)
    {
        if (job.TaskType == "RECTANGLE-AREA")
        {
            var area = job.NumberOne * job.NumberTwo;
            job.Result = area;
            job.IsComplete = true;
        }
        else if (job.TaskType == "RECTANGLE-PERIMETER")
        {
            var perimeter = 2 * (job.NumberOne + job.NumberTwo);
            job.Result = perimeter;
            job.IsComplete = true;
        }
        else if (job.TaskType == "CIRCLE-AREA")
        {
            var area = 3.14 * job.NumberOne;
            job.Result = area;
            job.IsComplete = true;
        }
        else if (job.TaskType == "CIRCLE-PERIMETER")
        {
            var perimeter = 2 * 3.14 * job.NumberOne;
            job.Result = perimeter;
            job.IsComplete = true;
        }

    }
    else
    {
        return Results.BadRequest("JOB ALREADY COMPLETED");
    }

    //commit to db
    await db.SaveChangesAsync();

    return Results.Ok(job);
});
*/
app.Run();

class Tasks
{
    public int Id { get; set; }
    public string? TaskType { get; set; }
    public int NumberOne { get; set; }
    public int NumberTwo { get; set; }
    public double Result { get; set; }
    public bool IsComplete { get; set; }
}

class Auth
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
}

class AuthDB : DbContext
{
    public AuthDB(DbContextOptions<AuthDB> options)
        : base(options) { }

    public DbSet<Auth> Users => Set<Auth>();
}

class TaskDB : DbContext
{
    public TaskDB(DbContextOptions<TaskDB> options)
        : base(options) { }

    public DbSet<Tasks> Jobs => Set<Tasks>();
}


