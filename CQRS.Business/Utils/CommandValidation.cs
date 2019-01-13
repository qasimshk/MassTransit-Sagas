using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CQRS.Business.Utils
{
    public abstract class CommandValidation : Results
    {
        public Results TryValidateCommand(object command)
        {
            var context = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(command, context, results, true);
            return SetResponse(State.unProcessableEntity, valid, 
                (results.Count == 0) ? null :
                results.FirstOrDefault().ErrorMessage.Trim());
        }
    }
}