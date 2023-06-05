namespace TDDSample.Shared.ProblemDetail;

public interface IProblemDetailMapper
{
    int GetMappedStatusCodes(Exception exception);
}
