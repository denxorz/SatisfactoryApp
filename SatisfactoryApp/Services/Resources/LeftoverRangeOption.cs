namespace SatisfactoryApp.Services.Resources;

public record LeftoverRangeOption(string Title, float Min, float Max)
{
    public override string ToString() => Title;
}
