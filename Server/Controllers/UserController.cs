#nullable disable
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;
using System.Configuration;
using System.Text;
using task5.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace task5.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;


    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public void Post([FromBody] String msg)
    {
        System.IO.File.WriteAllText("param.txt", msg);
    }

    [HttpGet]
    public string Get()
    {
        if (System.IO.File.Exists("param.txt"))
        {
            String msg = System.IO.File.ReadAllText("param.txt");

            string[] parameters = new string[5];
            parameters = msg.Split(",");

            String Country = parameters[0];
            double Errors = Convert.ToDouble(parameters[1]);
            int Seed = Convert.ToInt32(parameters[2]);
            int Page = Convert.ToInt32(parameters[3]);
            int AmountOfUsers = Convert.ToInt32(parameters[4]);

            System.IO.File.Delete("param.txt");

            var seed = CheckSeed(Seed);

            User[] users = new User[AmountOfUsers];

            for (int j = 0; j < AmountOfUsers; j++)
            {
                var rand = new Random(seed + j + (Page * 10));
                var user = Create.Unit(Country, Errors, rand);

                users[j] = new User();
                users[j].Number = 1 + j + (Page * 10);
                users[j].ID = user[0];
                users[j].Name = user[1];
                users[j].Adress = user[2];
                users[j].Phone = user[3];
            }

            string json = JsonConvert.SerializeObject(users);
            return json;
        }
        else{
            var NullUser = new User();
            NullUser.Number = 0;
            NullUser.ID = "";
            NullUser.Name = "";
            NullUser.Adress = "";
            NullUser.Phone = "";
            string json = JsonConvert.SerializeObject(NullUser);
            return json;
        }
    }

    public static int CheckSeed(int CustomSeed)
    {
        int seed;
        if (CustomSeed != 0)
        {
            seed = CustomSeed;
        }
        else
        {
            seed = 0;
        }
        return seed;
    }

}
