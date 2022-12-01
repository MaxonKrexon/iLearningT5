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


            User[] users = new User[AmountOfUsers];

            foreach (var num in Enumerable.Range(0, AmountOfUsers))
            {
                var rand = new Random(num + (Page * AmountOfUsers) + (Seed * 31415));
                var user = Create.Unit(Country, Errors, rand);

                users[num] = new User();
                users[num].Number = num + (Page * AmountOfUsers);
                users[num].ID = user[0];
                users[num].Name = user[1];
                users[num].Adress = user[2];
                users[num].Phone = user[3];
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
}
