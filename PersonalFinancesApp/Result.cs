namespace PersonalFinances;

class Result
{
	public bool Succeeded { get; } = false;
	public string Error { get; }

	private Result(bool succeeded) => Succeeded = succeeded;

	private Result(string error) => Error = error;

	public static Result Success() => new(true);

	public static Result Failure(string error) => new(error);
}
