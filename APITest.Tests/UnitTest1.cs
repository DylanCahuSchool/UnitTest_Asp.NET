namespace APITest.Tests
{
    [TestFixture]
    public class TestObjectTests
    {
        [Test]
        public void CreateTestObjectAndSaveToDatabase()
        {
            // Créez une configuration à partir du fichier appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Récupérez la chaîne de connexion à partir de la configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configurez les options de base de données avec la chaîne de connexion
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Créez une instance de votre contexte de base de données avec une base de données en mémoire pour les tests
            using (var dbContext = new ApplicationDbContext(options))
            {
                // Act : Créez un objet TestObject
                var testObject = new TestObject
                {
                    Name = "Exemple",
                };

                // Ajoutez l'objet à la base de données
                dbContext.TestObjects.Add(testObject);
                dbContext.SaveChanges();

                // Assert : Vérifiez que l'objet a été ajouté avec succès
                var savedTestObject = dbContext.TestObjects.Find(testObject.Id);
                Assert.NotNull(savedTestObject);
                Assert.AreEqual("Exemple", savedTestObject.Name);

                // Supprimez l'objet de la base de données
                dbContext.TestObjects.Remove(savedTestObject);
                dbContext.SaveChanges();
            }
        }
    }
}