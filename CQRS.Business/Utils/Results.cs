namespace CQRS.Business.Utils
{
    public enum State
    {
        ok,
        badRequest,
        notFound,
        unProcessableEntity,
        noContent,
        NotDeclared
    }

    public class Results
    {
        public State Status { get; private set; }
        public bool IsSuccessful { get; private set; }
        public dynamic ResponseMessage { get; private set; }

        public Results SetResponse(State state, bool isSuccessful, dynamic response)
        {
            return new Results {
                 Status = state,
                 IsSuccessful = isSuccessful,
                 ResponseMessage = response
            };
        }

        public Results SetResponse(bool isSuccessful, dynamic response)
        {
            return new Results
            {
                Status = State.NotDeclared,
                IsSuccessful = isSuccessful,
                ResponseMessage = response
            };
        }

        public Results SetResponse(bool isSuccessful)
        {
            return new Results
            {
                Status = State.NotDeclared,
                IsSuccessful = isSuccessful,
                ResponseMessage = null
            };
        }
    }
}
