#nullable disable
using System;

namespace task5.Server;
public class Create
{
    public static String[] Unit(string country, double Error, Random rand)
    {

        List<String> Towns = File.ReadAllLines($"Sets/{country}/Towns.csv").ToList();
        List<String> Streets = File.ReadAllLines($"Sets/{country}/Streets.csv").ToList();
        List<String> Names = File.ReadAllLines($"Sets/{country}/Names.csv").ToList();
        List<String> Surnames = File.ReadAllLines($"Sets/{country}/Surnames.csv").ToList();
        List<String> Regions = File.ReadAllLines($"Sets/{country}/Regions.csv").ToList();
        List<String> Phone = File.ReadAllLines($"Sets/{country}/Phone.csv").ToList();



        var UserName = GenerateName(Names, Surnames, rand);
        var Adress = GenerateAdress(Regions, Towns, Streets, rand);
        var PhoneNumber = GeneratePhone(Phone, rand);

        var bytesID = new byte[16];
        rand.NextBytes(bytesID);
        Guid ID = new Guid(bytesID);

        String[] User = { ID.ToString(), UserName, Adress, PhoneNumber };
        User = GenerateErrors(User, Error, rand);
        return User;
    }

    public static String GeneratePhone(List<String> Phone, Random rand)
    {
        var StateCode = Phone[0];
        var PhoneGeneratablePart = Convert.ToInt32(Phone[1]);
        var PhoneMin = 1;
        var PhoneMax = 1;

        for (int j = 0; j < PhoneGeneratablePart - 1; j++) { PhoneMin *= 10; }
        for (int j = 0; j < PhoneGeneratablePart; j++) { PhoneMax *= 10; }

        var PhoneNumber = $"{StateCode}{Phone[rand.Next(2, Phone.Count)]}{rand.Next(PhoneMin, PhoneMax)}";
        return PhoneNumber;
    }

    public static String GenerateAdress(List<String> Regions,
            List<String> Towns,
            List<String> Streets,
            Random rand)
    {
        String Adress = "";
        var ZipCode = GenerateZipCode(Towns, rand);
        if (ZipCode.Length > 0) Adress += ZipCode + ", ";
        if (Coin(rand)) Adress += Regions[rand.Next(0, Regions.Count)] + ", ";

        Adress += $"{Towns[rand.Next(1, Towns.Count)]}, {Streets[rand.Next(0, Streets.Count)]}, {rand.Next(0, 100)}";
        return Adress;
    }

    public static String GenerateZipCode(List<String> Towns, Random rand)
    {
        string ZipCode = "";
        if (Coin(rand))
        {
            var ZipCodeLength = Convert.ToDouble(Towns[0]);
            int ZipCodeMin = 1;
            int ZipCodeMax = 1;

            for (int j = 0; j < ZipCodeLength - 1; j++) { ZipCodeMin *= 10; }
            for (int j = 0; j < ZipCodeLength; j++) { ZipCodeMax *= 10; }

            ZipCode = rand.Next(ZipCodeMin, ZipCodeMax).ToString();
        }
        return ZipCode;
    }

    public static string GenerateName(List<String> Names, List<String> Surnames, Random rand)
    {
        var Name = Names[rand.Next(0, Names.Count)];
        var Surname = Surnames[rand.Next(0, Surnames.Count)];
        var UserName = $"{Name} {Surname}";
        return UserName;
    }

    public static bool Coin(Random rand)
    {
        var side = rand.Next(0, 2);
        if (side == 1)
        {
            return true;
        }
        else return false;
    }


    public static String[] GenerateErrors(String[] User, double Error, Random rand)
    {
        if (Error % 1 != 0)
        {
            if (Coin(rand))
            {
                int index = rand.Next(1, 4);

                int action = rand.Next(1, 4);
                switch (action)
                {
                    case 1: User[index] = SwapCharacters(User[index], rand); break;
                    case 2: User[index] = DeleteCharater(User[index], rand); break;
                    case 3: User[index] = AddCharacter(User[index], rand); break;
                }

            }
            Error -= 0.5;
        }

        for (int j = 0; j < Error; j++)
        {
            int index = rand.Next(1, 4);
            int action = rand.Next(1, 4);
            switch (action)
            {
                case 1: User[index] = SwapCharacters(User[index], rand); break;
                case 2: User[index] = DeleteCharater(User[index], rand); break;
                case 3: User[index] = AddCharacter(User[index], rand); break;
            }
        }

        return User;
    }

    public static String SwapCharacters(String Property, Random rand)
    {
        char[] prop = Property.ToCharArray();

        int index = rand.Next(1, prop.Length);

        char letter = prop[index];
        prop[index] = prop[index - 1];
        prop[index - 1] = letter;
        var result = new string(prop);

        return result;
    }

    public static String DeleteCharater(String Property, Random rand)
    {
        var prop = Property.ToCharArray();
        int index = rand.Next(1, prop.Length);

        prop[index] = ' ';
        var result = new string(prop);

        return result;
    }

    public static String AddCharacter(String Property, Random rand)
    {
        int index = rand.Next(1, Property.Length);

        var FirstHalf = Property[0..index];
        var SecondHalf = Property[index..Property.Length];
        string inserted = Convert.ToChar(rand.Next(48, 123)).ToString();
        string result = FirstHalf + inserted + SecondHalf;

        return result;
    }

}

