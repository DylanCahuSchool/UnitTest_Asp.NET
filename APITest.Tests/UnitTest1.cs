namespace APITest.Tests
{
    [TestFixture]
    public class TestObjectTests
    {
        [Test]
        public void CreateTestObjectAndSaveToDatabase()
        {
            // Cr�ez une configuration � partir du fichier appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // R�cup�rez la cha�ne de connexion � partir de la configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configurez les options de base de donn�es avec la cha�ne de connexion
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Cr�ez une instance de votre contexte de base de donn�es avec une base de donn�es en m�moire pour les tests
            using (var dbContext = new ApplicationDbContext(options))
            {
                // Act : Cr�ez un objet TestObject
                var testObject = new TestObject
                {
                    Name = "Exemple",
                };

                // Ajoutez l'objet � la base de donn�es
                dbContext.TestObjects.Add(testObject);
                dbContext.SaveChanges();

                // Assert : V�rifiez que l'objet a �t� ajout� avec succ�s
                var savedTestObject = dbContext.TestObjects.Find(testObject.Id);
                Assert.NotNull(savedTestObject);
                Assert.AreEqual("Exemple", savedTestObject.Name);

                // Supprimez l'objet de la base de donn�es
                dbContext.TestObjects.Remove(savedTestObject);
                dbContext.SaveChanges();
            }
        }
    }
}