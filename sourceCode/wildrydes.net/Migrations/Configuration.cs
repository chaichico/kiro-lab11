namespace wildrydes.net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Security.Cryptography;
    using System.Text;
    using wildrydes.net.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<wildrydes.net.Context.DefaultContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(wildrydes.net.Context.DefaultContext context)
        {
            // make a quick list of default unicorns
            context.Unicorns.AddOrUpdate(new UnicornModel[]
            {
                new UnicornModel
                {
                    Id = new Guid("d4bb97cd-f5e9-4e98-bc59-a21241aefdbc"),
                    Name = "Bucephalus",
                    Color = "Gold",
                    Description = "Bucephalus joined Wild Rydes in February 2016 and has been giving rydes almost daily. He says he most enjoys getting to know each of his ryders, which makes the job more interesting for him. In his spare time, Bucephalus enjoys watching sunsets and playing Pokemon Go.",
                    Rating = 4
                },
                new UnicornModel
                {
                    Id = new Guid("eef1efdd-552a-4e1d-8287-d104c2925572"),
                    Name = "Shadowfox",
                    Color = "White",
                    Description = "Shadowfox joined Wild Rydes after completing a distinguished career in the military, where he toured the world in many critical missions. Shadowfox enjoys impressing his ryders with magic tricks that he learned from his previous owner.",
                    Rating = 5
                },
                new UnicornModel
                {
                    Id = new Guid("5dcf9469-588f-4d80-9183-eca28c8d0e2a"),
                    Name = "Rocinante",
                    Color = "Brown",
                    Description = "Rocinante recently joined the Wild Rydes team in Madrid, Spain. She was instrumental in forming Wild Rydes' Spanish operations after a long, distinguished acting career in windmill shadow-jousting.",
                    Rating = 4
                }
            });

            // make a generic authenticated user
            context.Users.AddOrUpdate(new UserModel
            {
                Id = new Guid("4aff9469-588f-4a80-9183-eca28c8d0f7d"),
                Email = "user@unicornrides.aws",
                Password = GetHash("Passw0rd")
            });
        }

        public static string GetHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
