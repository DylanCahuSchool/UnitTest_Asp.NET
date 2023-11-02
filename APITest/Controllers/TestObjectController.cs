using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest.Controllers
{
    [ApiController]
    public class TestObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestObjectController(ApplicationDbContext planifBACContext)
        {
            _context = planifBACContext;
        }

        [HttpGet("getAllTestObjects")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TestObject>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> getAllEstimats(int page = 1, int pagesize = 10)
        {
            List<TestObject> testObjects = await _context.TestObjects.ToListAsync();

            if (testObjects.Count() == 0 || testObjects == null) NotFound();

            return Ok(testObjects);
        }

        [HttpGet("getTestObjectById/{id}")]
        public async Task<ActionResult> GetTestObjectById(int id)
        {
            var testObject = await _context.TestObjects.FirstOrDefaultAsync(e => e.Id == id);

            if (testObject == null)
            {
                return NotFound("Devis non trouvée.");
            }

            return Ok(testObject);
        }

        [HttpPost("createTestObject")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestObject))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTestObject([FromBody] TestObject testObject)
        {
            if (testObject == null)
            {
                return BadRequest("Le devis ne peut pas être null.");
            }

            try
            {
                _context.TestObjects.Add(testObject);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTestObjectById), new { id = testObject.Id }, testObject);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la création du devis : {ex.Message}");
            }
        }

        [HttpPost("UpdateTestObject")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TestObject))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTestObject(int id, [FromBody] TestObject updatedTestObject)
        {
            if (updatedTestObject == null)
            {
                return BadRequest("Les données de mise à jour ne peuvent pas être nulles.");
            }

            try
            {
                // Utilisez la méthode GetTestObjectById pour rechercher le devis existante par son ID
                var existingTestObject = await GetTestObjectById(id);

                if (existingTestObject == null)
                {
                    return NotFound("Devis non trouvée.");
                }

                var existingProperties = existingTestObject.GetType().GetProperties();

                foreach (var propertyInfo in existingProperties)
                {
                    // Vérifiez si la propriété existe dans l'objet de mise à jour
                    var updatedProperty = updatedTestObject.GetType().GetProperty(propertyInfo.Name);
                    if (updatedProperty != null)
                    {
                        // Obtenez la valeur de la propriété de l'objet de mise à jour
                        var updatedValue = updatedProperty.GetValue(updatedTestObject);

                        // Mettez à jour la valeur de la propriété de l'estimation existante
                        propertyInfo.SetValue(existingTestObject, updatedValue);
                    }
                }
                // Enregistrez les modifications dans la base de données
                await _context.SaveChangesAsync();

                // Retournez le devis mise à jour en tant que réponse
                return Ok(existingTestObject);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la mise à jour du devis : {ex.Message}");
            }
        }

        [HttpDelete("DeleteTestObject/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTestObject(int id)
        {
            var testObject = await _context.TestObjects.FirstOrDefaultAsync(e => e.Id == id);

            if (testObject == null)
            {
                return NotFound("Devis non trouvé.");
            }

            try
            {
                _context.TestObjects.Remove(testObject);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors de la suppression du devis : {ex.Message}");
            }
        }

    }
}

