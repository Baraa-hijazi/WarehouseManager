using FluentValidation.Results;

namespace WarehouseManager.Exceptions
{
    public class ValidationException : Exception
    {
        private IDictionary<string, string[]> Failures { get; } = null!;

        public ValidationException()
        : base("validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }
    }
}
